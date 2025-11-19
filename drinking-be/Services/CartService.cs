// Services/CartService.cs
using AutoMapper;
using drinking_be.Dtos.CartDtos;
using drinking_be.Interfaces;
using drinking_be.Interfaces.OptionInterfaces;
using drinking_be.Interfaces.ProductInterfaces;
using drinking_be.Models;
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
        private readonly ISugarLevelRepository _sugarRepo; // Cần cho Mapping
        private readonly IIceLevelRepository _iceRepo; // Cần cho Mapping
        private readonly IMapper _mapper;
        private readonly IGenericRepository<CartItem> _cartItemRepo;
        public CartService(
            ICartRepository cartRepo,
            IProductRepository productRepo,
            ISizeRepository sizeRepo,
            ISugarLevelRepository sugarRepo,
            IIceLevelRepository iceRepo,
            IGenericRepository<CartItem> cartItemRepo,
            IMapper mapper)
        {
            _cartRepo = cartRepo;
            _cartItemRepo = cartItemRepo;
            _productRepo = productRepo;
            _sizeRepo = sizeRepo;
            _sugarRepo = sugarRepo;
            _iceRepo = iceRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Hàm nội bộ: Lấy giỏ hàng của User, nếu chưa có thì tạo mới.
        /// </summary>
        private async Task<Cart> GetOrCreateCartAsync(int userId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _cartRepo.AddAsync(cart);
                await _cartRepo.SaveChangesAsync();
            }
            return cart;
        }

        /// <summary>
        /// Logic mapping thủ công từ Cart (Entity) sang CartReadDto (để tính tổng tiền)
        /// </summary>
        private CartReadDto MapCartToReadDto(Cart cart)
        {
            var dto = _mapper.Map<CartReadDto>(cart);
            dto.TotalAmount = 0;

            // Lọc ra các món chính (không phải topping)
            var mainItems = cart.CartItems.Where(ci => ci.ParentItemId == null).ToList();

            foreach (var mainItem in mainItems)
            {
                var itemDto = _mapper.Map<CartItemReadDto>(mainItem);

                // Lấy các topping con của món chính
                var toppings = mainItem.InverseParentItem.ToList();
                itemDto.Toppings = _mapper.Map<List<CartToppingReadDto>>(toppings);

                dto.Items.Add(itemDto);

                // Tính tổng tiền (bao gồm cả món chính và topping)
                dto.TotalAmount += itemDto.FinalPrice;
                dto.TotalAmount += itemDto.Toppings.Sum(t => t.FinalPrice);
            }

            return dto;
        }

        public async Task<CartReadDto> GetMyCartAsync(int userId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            return MapCartToReadDto(cart);
        }

        public async Task<CartReadDto> AddItemToCartAsync(int userId, CartItemCreateDto itemDto)
        {
            var cart = await GetOrCreateCartAsync(userId);

            // --- Lấy dữ liệu gốc (Tương tự OrderService) ---
            var productIds = itemDto.Toppings.Select(t => t.ProductId).ToList();
            productIds.Add(itemDto.ProductId);

            var allProducts = (await _productRepo.GetProductsByIdsAsync(productIds)).ToDictionary(p => p.Id);
            var size = await _sizeRepo.GetByIdAsync(itemDto.SizeId);

            if (!allProducts.ContainsKey(itemDto.ProductId) || size == null)
            {
                throw new Exception("Sản phẩm hoặc Size không hợp lệ.");
            }

            // --- Tính toán giá (Tương tự OrderService) ---
            var product = allProducts[itemDto.ProductId];
            decimal basePrice = product.BasePrice;
            decimal sizeModifier = size.PriceModifier ?? 0m;
            decimal itemUnitPrice = basePrice + sizeModifier;
            decimal itemTotalPrice = itemUnitPrice * itemDto.Quantity;

            // 1. Tạo CartItem (Món chính)
            var mainCartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                BasePrice = basePrice,
                FinalPrice = itemTotalPrice,
                SizeId = itemDto.SizeId,
                SugarLevelId = itemDto.SugarLevelId,
                IceLevelId = itemDto.IceLevelId,
                ParentItemId = null
            };

            // 2. Tạo CartItem (Topping)
            var toppingItems = new List<CartItem>();
            foreach (var toppingDto in itemDto.Toppings)
            {
                if (!allProducts.ContainsKey(toppingDto.ProductId))
                {
                    throw new Exception($"Topping ID {toppingDto.ProductId} không hợp lệ.");
                }
                var toppingProduct = allProducts[toppingDto.ProductId];
                decimal toppingPrice = toppingProduct.BasePrice * toppingDto.Quantity;

                toppingItems.Add(new CartItem
                {
                    CartId = cart.Id,
                    ProductId = toppingDto.ProductId,
                    Quantity = toppingDto.Quantity,
                    BasePrice = toppingProduct.BasePrice,
                    FinalPrice = toppingPrice,
                    ParentItem = mainCartItem // Liên kết với món chính
                });
            }

            // 3. Thêm vào DB
            await _cartItemRepo.AddAsync(mainCartItem); // Thêm món chính
            if (toppingItems.Any())
            {
                _cartItemRepo.DeleteRange(cart.CartItems); // Thêm các topping (cần IGenericRepository<CartItem>)
            }
            await _cartRepo.SaveChangesAsync();

            // 4. Trả về giỏ hàng mới
            return await GetMyCartAsync(userId);
        }

        public async Task<CartReadDto> RemoveItemFromCartAsync(int userId, long cartItemId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var cartItem = await _cartRepo.GetCartItemByIdAsync(cartItemId);

            if (cartItem == null || cartItem.CartId != cart.Id)
            {
                throw new Exception("Sản phẩm không tồn tại trong giỏ hàng.");
            }

            // Repository sẽ xử lý việc xóa item và các topping con
            _cartRepo.DeleteCartItem(cartItem);
            await _cartRepo.SaveChangesAsync();

            return await GetMyCartAsync(userId);
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart != null && cart.CartItems.Any())
            {
                _cartItemRepo.DeleteRange(cart.CartItems); // Cần IGenericRepository<CartItem>
                await _cartItemRepo.SaveChangesAsync();
            }
        }

        public async Task<CartReadDto> UpdateItemQuantityAsync(int userId, CartItemUpdateDto updateDto)
        {
            // 1. Lấy giỏ hàng để đảm bảo tính bảo mật
            var cart = await GetOrCreateCartAsync(userId);

            // 2. Lấy Item cần sửa (Kèm theo Size và Topping con để tính giá lại)
            // Lưu ý: Chúng ta cần Include Size để lấy PriceModifier
            var cartItem = await _cartItemRepo.FindAsync(
                ci => ci.Id == updateDto.CartItemId && ci.CartId == cart.Id,
                includeProperties: "Size,InverseParentItem" // Giả định GenericRepo hỗ trợ string include hoặc bạn dùng Queryable
            ).ContinueWith(t => t.Result.FirstOrDefault());

            // *Nếu GenericRepo của bạn không hỗ trợ Include chuỗi, 
            // bạn nên thêm hàm GetCartItemWithDetailsAsync vào ICartRepository tương tự như GetCartByUserIdAsync
            // Ở đây tôi giả định bạn sẽ dùng _cartRepo.GetCartItemByIdAsync(cartItemId) đã có trong code cũ

            cartItem = await _cartRepo.GetCartItemByIdAsync(updateDto.CartItemId);

            if (cartItem == null || cartItem.CartId != cart.Id)
            {
                throw new Exception("Sản phẩm không tồn tại trong giỏ hàng.");
            }

            // 3. Tính toán thay đổi (Ratio)
            int oldQuantity = cartItem.Quantity;
            int newQuantity = updateDto.Quantity;

            // 4. Cập nhật món chính
            // Giá đơn vị = BasePrice (giá gốc SP) + Size Modifier (nếu có)
            decimal sizeModifier = cartItem.Size?.PriceModifier ?? 0m;
            decimal unitPrice = cartItem.BasePrice + sizeModifier;

            cartItem.Quantity = newQuantity;
            cartItem.FinalPrice = unitPrice * newQuantity;
            _cartItemRepo.Update(cartItem);

            // 5. Cập nhật các Topping con (nếu có)
            // Logic: Nếu mua 1 ly có 2 trân châu -> mua 2 ly sẽ có 4 trân châu
            if (cartItem.InverseParentItem != null && cartItem.InverseParentItem.Any())
            {
                foreach (var topping in cartItem.InverseParentItem)
                {
                    // Tính số lượng topping trên mỗi đơn vị món chính
                    int toppingUnitQty = topping.Quantity / oldQuantity;

                    // Cập nhật số lượng topping mới
                    topping.Quantity = toppingUnitQty * newQuantity;

                    // Cập nhật giá topping (BasePrice của topping là giá gốc)
                    topping.FinalPrice = topping.BasePrice * topping.Quantity;

                    _cartItemRepo.Update(topping);
                }
            }

            await _cartRepo.SaveChangesAsync();

            // 6. Trả về giỏ hàng mới
            return await GetMyCartAsync(userId);
        }
    }
}