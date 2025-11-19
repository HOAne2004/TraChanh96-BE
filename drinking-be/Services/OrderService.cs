// Services/OrderService.cs
using drinking_be.Dtos.OrderDtos;
using drinking_be.Models;
using drinking_be.Enums; // Giả định có OrderStatusEnum
using AutoMapper;
using System.Collections.Generic;
using drinking_be.Interfaces.ProductInterfaces;
using drinking_be.Interfaces.OptionInterfaces;
using drinking_be.Interfaces.OrderInterfaces;

namespace drinking_be.Services
{
    public class OrderService : IOrderService
    {
        // Inject các Repository cần thiết
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _productRepo;
        private readonly ISizeRepository _sizeRepo; // Cần để tra cứu giá size
        private readonly IMapper _mapper;
        private readonly ISugarLevelRepository _sugarRepo;
        private readonly IIceLevelRepository _iceRepo;

        public OrderService(IOrderRepository orderRepo,
                            IProductRepository productRepo,
                            ISizeRepository sizeRepo,
                            ISugarLevelRepository sugarRepo, // Thêm
                    IIceLevelRepository iceRepo,
                            IMapper mapper)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _sizeRepo = sizeRepo;
            _mapper = mapper;
            _sugarRepo = sugarRepo;
            _iceRepo = iceRepo;
        }

        public async Task<OrderReadDto> CreateOrderAsync(OrderCreateDto orderDto)
        {
            // --- 1. Lấy dữ liệu Sản phẩm & Tùy chọn ---

            // 1.1. Lấy tất cả ID Sản phẩm (Món chính & Topping)
            var productIds = orderDto.Items.Select(i => i.ProductId)
                                           .Union(orderDto.Items.SelectMany(i => i.Toppings).Select(t => t.ToppingId))
                                           .Distinct()
                                           .ToList();

            // 1.2. Lấy tất cả ID Size, sugar, ice 
            var sizeIds = orderDto.Items.Select(i => i.SizeId).Distinct().ToList();

            var sugarIds = orderDto.Items.Select(i => i.SugarLevelId).Distinct().ToList();
            var iceIds = orderDto.Items.Select(i => i.IceLevelId).Distinct().ToList();
            var allSugars = (await _sugarRepo.GetSugarLevelsByIdsAsync(sugarIds)).ToDictionary(s => s.Id);
            var allIces = (await _iceRepo.GetIceLevelsByIdsAsync(iceIds)).ToDictionary(i => i.Id);

            // 1.3. Tra cứu dữ liệu gốc từ DB
            var allProducts = (await _productRepo.GetProductsByIdsAsync(productIds)).ToDictionary(p => p.Id);
            var allSizes = (await _sizeRepo.GetSizesByIdsAsync(sizeIds)).ToDictionary(s => s.Id);

            // TODO: Bổ sung logic kiểm tra tồn kho (nếu có)
            if (allProducts.Count != productIds.Count || allSizes.Count != sizeIds.Count)
            {
                throw new Exception("Thông tin một hoặc nhiều sản phẩm/size không hợp lệ hoặc không tồn tại.");
            }

            // --- 2. Ánh xạ và Tính toán Giá (Logic nghiệp vụ cốt lõi) ---

            // Khởi tạo Order Entity
            var order = new Order
            {
                UserId = orderDto.UserId,
                StoreId = orderDto.StoreId,
                PaymentMethodId = orderDto.PaymentMethodId,
                DeliveryAddress = orderDto.DeliveryAddress,
                CustomerName = orderDto.CustomerName,
                CustomerPhone = orderDto.CustomerPhone,
                OrderDate = DateTime.UtcNow,
                Status = (byte)OrderStatusEnum.New, // Mặc định là đơn hàng mới
                VoucherCodeUsed = orderDto.VoucherCodeUsed
                // Các trường tổng tiền sẽ được tính sau
            };

            var allOrderItems = new List<OrderItem>();
            decimal subTotalAmount = 0; // Tổng tiền trước khi áp dụng phí/chiết khấu

            // Xử lý từng Món chính trong đơn hàng
            foreach (var itemDto in orderDto.Items)
            {
                var product = allProducts[itemDto.ProductId];
                var size = allSizes[itemDto.SizeId];

                // 2.1. Tính toán giá cho Món chính (Beverage)
                decimal basePrice = product.BasePrice;
                decimal sizeModifier = size.PriceModifier ?? 0m; // Sử dụng 0m để chỉ định Decimal

                // Final Price 1 đơn vị = Giá cơ bản + Giá phụ thu Size
                decimal itemUnitPrice = basePrice + sizeModifier;
                decimal itemTotalPrice = itemUnitPrice * itemDto.Quantity;

                // 2.2. Tạo OrderItem Entity cho Món chính
                var mainOrderItem = new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    BasePrice = basePrice,
                    FinalPrice = itemTotalPrice,

                    SizeId = itemDto.SizeId,
                    SugarLevelId = itemDto.SugarLevelId,
                    IceLevelId = itemDto.IceLevelId,
                    ParentItemId = null // Món chính
                };

                allOrderItems.Add(mainOrderItem);
                subTotalAmount += mainOrderItem.FinalPrice;

                // 2.3. Xử lý Topping (Order Items con)
                foreach (var toppingDto in itemDto.Toppings)
                {
                    var toppingProduct = allProducts[toppingDto.ToppingId];

                    decimal toppingUnitPrice = toppingProduct.BasePrice;
                    decimal toppingTotalPrice = toppingUnitPrice * toppingDto.Quantity;

                    var toppingItem = new OrderItem
                    {
                        ProductId = toppingDto.ToppingId,
                        Quantity = toppingDto.Quantity,
                        BasePrice = toppingProduct.BasePrice,
                        FinalPrice = toppingTotalPrice,

                        // Liên kết với Món chính: EF Core sẽ tự gán ParentItemId
                        ParentItem = mainOrderItem
                    };

                    allOrderItems.Add(toppingItem);
                    subTotalAmount += toppingItem.FinalPrice;
                }
            }

            // --- 3. Tính toán Tổng tiền cuối cùng ---

            order.TotalAmount = subTotalAmount; // Tổng tiền hàng

            // TODO: Triển khai các hàm sau
            // order.ShippingFee = await CalculateShippingFee(order.DeliveryAddress, order.StoreId);
            // order.DiscountAmount = await ApplyVoucher(order.VoucherCodeUsed, subTotalAmount, order.UserId);

            // Giả định: Discount và Shipping Fee là 0 cho ví dụ này
            order.ShippingFee = 0m;
            order.DiscountAmount = 0m;

            order.GrandTotal = order.TotalAmount + (order.ShippingFee ?? 0m) - (order.DiscountAmount ?? 0m);

            // --- 4. Lưu Order (Sử dụng Repository với Transaction) ---
            var createdOrder = await _orderRepo.CreateOrderWithDetails(order, allOrderItems);

            // --- 5. Ánh xạ sang OrderReadDto và trả về ---

            // Cần ánh xạ thủ công các tên (Name/Label) cho DTO trả về (vì không có Navigation Properties)
            var readDto = _mapper.Map<OrderReadDto>(createdOrder);
            readDto.Items = new List<OrderItemReadDto>();

            // Lấy lại các OrderItem đã được lưu (bao gồm cả ID chính xác)
            var mainItemsFromRepo = allOrderItems.Where(i => i.ParentItemId == null).ToList();


            foreach (var mainItem in mainItemsFromRepo)
            {
                var mainItemDto = _mapper.Map<OrderItemReadDto>(mainItem);

                // Gán tên/label thủ công
                var product = allProducts[mainItem.ProductId];

                mainItemDto.ProductName = product.Name;

                // Gán Label cho Options (LƯU Ý: Phải kiểm tra Nullable)
                if (mainItem.SizeId.HasValue && allSizes.TryGetValue(mainItem.SizeId.Value, out var size))
                {
                    mainItemDto.SizeLabel = size.Label;
                }
                if (mainItem.SugarLevelId.HasValue && allSugars.TryGetValue(mainItem.SugarLevelId.Value, out var sugar))
                {
                    mainItemDto.SugarLabel = sugar.Label; // TODO: Cần IProductRepository
                }
                if (mainItem.IceLevelId.HasValue && allIces.TryGetValue(mainItem.IceLevelId.Value, out var ice))
                {
                    mainItemDto.IceLabel = ice.Label; // TODO: Cần IProductRepository
                }

                // Ánh xạ Topping (Lấy các item con có ParentItemId trỏ về ID của món chính này)
                // LƯU Ý: Do EF chưa gán ID chính xác, ta dùng ParentItem Navigation Property
                var toppings = allOrderItems.Where(i => i.ParentItem == mainItem).ToList();

                mainItemDto.Toppings = toppings.Select(t =>
                {
                    var toppingDto = _mapper.Map<OrderToppingReadDto>(t);
                    toppingDto.ProductName = allProducts[t.ProductId].Name;
                    return toppingDto;
                }).ToList();

                readDto.Items.Add(mainItemDto);
            }

            return readDto; ;
        }

        public async Task<IEnumerable<OrderReadDto>> GetOrdersAsync(int? userId)
        {
            // 1. Lấy dữ liệu thô từ DB (đã Include đầy đủ)
            var orders = await _orderRepo.GetOrdersAsync(userId);
            var orderDtos = new List<OrderReadDto>();

            // 2. Duyệt và ánh xạ thủ công
            foreach (var order in orders)
            {
                // Map các thông tin cơ bản của Order (Status, TotalAmount...)
                var orderDto = _mapper.Map<OrderReadDto>(order);
                orderDto.Items = new List<OrderItemReadDto>();

                // Tách Món chính và Topping
                // Món chính là món có ParentItemId == null
                var mainItems = order.OrderItems.Where(i => i.ParentItemId == null).ToList();

                foreach (var mainItem in mainItems)
                {
                    var itemDto = _mapper.Map<OrderItemReadDto>(mainItem);

                    // Gán tên và Label từ các bảng liên kết (đã được Include ở Repo)
                    itemDto.ProductName = mainItem.Product?.Name;
                    itemDto.SizeLabel = mainItem.Size?.Label;
                    itemDto.SugarLabel = mainItem.SugarLevel?.Label;
                    itemDto.IceLabel = mainItem.IceLevel?.Label;

                    // Tìm các Topping thuộc về món chính này
                    var childToppings = order.OrderItems.Where(i => i.ParentItemId == mainItem.Id).ToList();

                    itemDto.Toppings = childToppings.Select(t =>
                    {
                        var toppingDto = _mapper.Map<OrderToppingReadDto>(t);
                        toppingDto.ProductName = t.Product?.Name;
                        return toppingDto;
                    }).ToList();

                    orderDto.Items.Add(itemDto);
                }

                orderDtos.Add(orderDto);
            }

            return orderDtos;
        }

    }
}