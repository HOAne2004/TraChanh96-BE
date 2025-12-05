using AutoMapper;
using drinking_be.Dtos.CartDtos;
using drinking_be.Interfaces;
using drinking_be.Interfaces.OptionInterfaces;
using drinking_be.Interfaces.ProductInterfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore; // Cần để dùng Include nếu viết query trực tiếp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drinking_be.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IProductRepository _productRepo;
        private readonly ISizeRepository _sizeRepo;
        private readonly IGenericRepository<CartItem> _cartItemRepo;
        private readonly IMapper _mapper;

        public CartService(
            ICartRepository cartRepo,
            IProductRepository productRepo,
            ISizeRepository sizeRepo,
            IGenericRepository<CartItem> cartItemRepo,
            IMapper mapper)
        {
            _cartRepo = cartRepo;
            _cartItemRepo = cartItemRepo;
            _productRepo = productRepo;
            _sizeRepo = sizeRepo;
            _mapper = mapper;
        }

        // --- PRIVATE HELPER ---

        private async Task<Cart> GetOrCreateCartAsync(int userId)
        {
            // Đảm bảo hàm này trong Repository đã Include(c => c.CartItems).ThenInclude(...) đầy đủ
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _cartRepo.AddAsync(cart);
                await _cartRepo.SaveChangesAsync();

                // Tạo xong thì trả về cart rỗng, không cần load lại
                return cart;
            }
            return cart;
        }

        private CartReadDto MapCartToReadDto(Cart cart)
        {
            // 1. Map cơ bản
            var dto = _mapper.Map<CartReadDto>(cart);
            dto.Items = new List<CartItemReadDto>(); // Reset list để fill thủ công
            dto.TotalAmount = 0;

            // 2. Lọc món chính (ParentId == null)
            if (cart.CartItems != null)
            {
                var mainItems = cart.CartItems.Where(ci => ci.ParentItemId == null).ToList();

                foreach (var mainItem in mainItems)
                {
                    var itemDto = _mapper.Map<CartItemReadDto>(mainItem);

                    // Map Topping (InverseParentItem)
                    if (mainItem.InverseParentItem != null && mainItem.InverseParentItem.Any())
                    {
                        itemDto.Toppings = _mapper.Map<List<CartToppingReadDto>>(mainItem.InverseParentItem);
                    }

                    dto.Items.Add(itemDto);

                    // Tính tổng tiền = FinalPrice của món chính + FinalPrice của các topping
                    // (Lưu ý: FinalPrice trong DB đã được tính toán = UnitPrice * Quantity rồi)
                    decimal itemTotal = mainItem.FinalPrice;
                    if (mainItem.InverseParentItem != null)
                    {
                        itemTotal += mainItem.InverseParentItem.Sum(t => t.FinalPrice);
                    }

                    dto.TotalAmount += itemTotal;
                }
            }

            return dto;
        }

        // --- PUBLIC METHODS ---

        public async Task<CartReadDto> GetMyCartAsync(int userId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            return MapCartToReadDto(cart);
        }

        public async Task<CartReadDto> AddItemToCartAsync(int userId, CartItemCreateDto itemDto)
        {
            var cart = await GetOrCreateCartAsync(userId);

            // 1. Validate & Lấy giá
            var productIds = itemDto.Toppings?.Select(t => t.ProductId).ToList() ?? new List<int>();
            productIds.Add(itemDto.ProductId);

            var allProducts = (await _productRepo.GetProductsByIdsAsync(productIds)).ToDictionary(p => p.Id);
            if (!allProducts.ContainsKey(itemDto.ProductId)) throw new Exception("Sản phẩm không tồn tại.");

            var size = await _sizeRepo.GetByIdAsync(itemDto.SizeId);
            if (size == null) throw new Exception("Size không hợp lệ.");

            // 2. Tính toán
            var product = allProducts[itemDto.ProductId];
            decimal basePrice = product.BasePrice;
            decimal sizeModifier = (decimal)size.PriceModifier; // Decimal 18,2

            // Giá 1 đơn vị món chính = Giá gốc + Giá size
            decimal itemUnitPrice = basePrice + sizeModifier;
            decimal itemTotalPrice = itemUnitPrice * itemDto.Quantity;

            // 3. Tạo Entity món chính
            var mainCartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                BasePrice = basePrice, // Lưu giá gốc SP
                FinalPrice = itemTotalPrice, // Lưu tổng giá sau khi nhân số lượng
                SizeId = (short)itemDto.SizeId, // Ép kiểu nếu DB là smallint
                SugarLevelId = (short?)itemDto.SugarLevelId,
                IceLevelId = (short?)itemDto.IceLevelId,
                ParentItemId = null
            };

            // 4. Tạo Entity Topping (nếu có)
            var toppingItems = new List<CartItem>();
            if (itemDto.Toppings != null)
            {
                foreach (var toppingDto in itemDto.Toppings)
                {
                    if (!allProducts.ContainsKey(toppingDto.ProductId)) continue;

                    var toppingProduct = allProducts[toppingDto.ProductId];
                    // Số lượng topping = Số lượng topping trên 1 ly * Số ly
                    int totalToppingQty = toppingDto.Quantity * itemDto.Quantity;
                    decimal toppingTotalPrice = toppingProduct.BasePrice * totalToppingQty;

                    toppingItems.Add(new CartItem
                    {
                        CartId = cart.Id,
                        ProductId = toppingDto.ProductId,
                        Quantity = totalToppingQty,
                        BasePrice = toppingProduct.BasePrice,
                        FinalPrice = toppingTotalPrice,
                        ParentItem = mainCartItem // EF Core tự hiểu liên kết
                    });
                }
            }

            // 5. Lưu vào DB
            // Thêm món chính (EF Core sẽ tự thêm luôn toppingItems vì có liên kết ParentItem)
            await _cartItemRepo.AddAsync(mainCartItem);

            // ⭐️ SỬA LỖI CŨ: Không cần AddRange toppingItems riêng nếu đã gán vào ParentItem.
            // Nhưng nếu muốn chắc chắn hoặc EF không tự track, dùng AddRange.
            // Tuyệt đối KHÔNG dùng DeleteRange(cart.CartItems) ở đây!

            await _cartRepo.SaveChangesAsync();

            // 6. Load lại đầy đủ để trả về
            // (Do biến cart ở trên có thể chưa có data mới hoặc chưa include đủ)
            return await GetMyCartAsync(userId);
        }

        public async Task<CartReadDto> UpdateItemQuantityAsync(int userId, CartItemUpdateDto updateDto)
        {
            var cart = await GetOrCreateCartAsync(userId);

            // Lấy item cần sửa (kèm Size và Topping)
            var cartItem = await _cartRepo.GetCartItemByIdAsync(updateDto.CartItemId);

            if (cartItem == null || cartItem.CartId != cart.Id)
            {
                throw new Exception("Sản phẩm không tồn tại trong giỏ.");
            }

            // Logic tính toán lại
            int oldQuantity = cartItem.Quantity;
            int newQuantity = updateDto.Quantity;

            if (newQuantity <= 0)
            {
                // Nếu số lượng <= 0 thì xóa luôn
                _cartRepo.DeleteCartItem(cartItem);
            }
            else
            {
                // Cập nhật món chính
                decimal sizeModifier = cartItem.Size?.PriceModifier ?? 0m;
                decimal unitPrice = cartItem.BasePrice + sizeModifier;

                cartItem.Quantity = newQuantity;
                cartItem.FinalPrice = unitPrice * newQuantity;
                _cartItemRepo.Update(cartItem);

                // Cập nhật topping con
                if (cartItem.InverseParentItem != null)
                {
                    foreach (var topping in cartItem.InverseParentItem)
                    {
                        // Tính lại số lượng topping gốc trên 1 đơn vị
                        // (Tránh chia cho 0 nếu oldQuantity bị lỗi)
                        int toppingUnitQty = oldQuantity > 0 ? topping.Quantity / oldQuantity : 1;

                        topping.Quantity = toppingUnitQty * newQuantity;
                        topping.FinalPrice = topping.BasePrice * topping.Quantity;
                        _cartItemRepo.Update(topping);
                    }
                }
            }

            await _cartRepo.SaveChangesAsync();
            return await GetMyCartAsync(userId);
        }

        public async Task<CartReadDto> RemoveItemFromCartAsync(int userId, long cartItemId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var cartItem = await _cartRepo.GetCartItemByIdAsync(cartItemId);

            if (cartItem != null && cartItem.CartId == cart.Id)
            {
                _cartRepo.DeleteCartItem(cartItem);
                await _cartRepo.SaveChangesAsync();
            }

            return await GetMyCartAsync(userId);
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart != null && cart.CartItems.Any())
            {
                // Xóa tất cả
                // Cần đảm bảo Repository hỗ trợ xóa range
                foreach (var item in cart.CartItems)
                {
                    // Xóa từng cái hoặc dùng RemoveRange của DbSet
                    // Ở đây giả định Repo có hàm xóa range hoặc dùng context trực tiếp
                    // _cartItemRepo.Delete(item); 
                }
                // Tốt nhất là dùng _context.CartItems.RemoveRange(cart.CartItems) trong Repository
                await _cartRepo.ClearCartItemsAsync(cart.Id);
            }
        }
    }
}