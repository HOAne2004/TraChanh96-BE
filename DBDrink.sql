--DROP DATABASE DBDrink
--create database DBDrink
--go
--use DBDrink
--go

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TenBang]') AND type in (N'U'))
BEGIN
-- Categories
CREATE TABLE Category (
    id INT IDENTITY(1,1) PRIMARY KEY,
    parent_id INT NULL,
    slug VARCHAR(100) UNIQUE NOT NULL,
    name NVARCHAR(100) NOT NULL,
    is_active BIT DEFAULT 1,
    sort_order TINYINT DEFAULT 0,


    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (parent_id) REFERENCES Category(id)
);

-- Products
CREATE TABLE Product (
    -- Khóa chính
    id int IDENTITY(1,1) PRIMARY KEY, -- Mã SP, tự tăng
    
    -- Định danh và SEO
    public_id VARCHAR(36) UNIQUE NOT NULL, -- UUID/GUID công khai
    category_id INT NOT NULL,              -- FK: Mã Danh mục (Liên kết với bảng Categories)
    slug NVARCHAR(255) UNIQUE NOT NULL,     -- URL thân thiện, hỗ trợ Tiếng Việt
    
    -- Thông tin cơ bản
    name NVARCHAR(255) NOT NULL,            -- Tên sản phẩm, hỗ trợ Tiếng Việt
    product_type NVARCHAR(20) NOT NULL,     -- Loại sản phẩm: 'Beverage', 'Topping' (Đã chuẩn hóa)
    
    -- Giá & Mô tả
    base_price DECIMAL(18, 2) NOT NULL,     -- Giá cơ bản (10 chữ số, 2 chữ số thập phân)
    image_url VARCHAR(500),                 -- Đường dẫn ảnh
    description NVARCHAR(MAX),              -- Mô tả chi tiết (Văn bản dài, hỗ trợ Tiếng Việt)
    ingredient NVARCHAR(MAX),               -- Thành phần (Hỗ trợ Tiếng Việt)
    
    -- Phân tích & Trạng thái
    status NVARCHAR(20) NOT NULL,           -- Trạng thái: 'Active', 'Draft', 'Archived'
    total_rating FLOAT DEFAULT 0,           -- Tổng sao đánh giá trung bình
    total_sold INT DEFAULT 0,               -- Tổng số lượng đã bán (Dữ liệu tính toán)
    search_vector VARBINARY(MAX),

    -- Dấu thời gian
    launch_date_time DATETIME,              -- Ngày giờ ra mắt/mở bán
    created_at DATETIME DEFAULT GETDATE(),  -- Thời gian tạo bản ghi
    updated_at DATETIME

    FOREIGN KEY (category_id) REFERENCES Category(id)
);

ALTER TABLE Product
ADD CONSTRAINT FK_Product_Category
FOREIGN KEY (category_id) REFERENCES Category(id);

-- Brand Information
CREATE TABLE Brand (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    logo_url VARCHAR(500),
    address NVARCHAR(255),
    hotline VARCHAR(20),
    email_support VARCHAR(100),
    tax_code VARCHAR(30),
    company_name NVARCHAR(100),
    slogan NVARCHAR(255),
    copyright_text NVARCHAR(255),

    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME,
);

-- Social media
create table Social_media(
    id INT IDENTITY(1,1) PRIMARY KEY,
    brand_id int not null,
    platform_name varchar(30) not null,
    url varchar(500) not null,
    icon_url varchar(500),
    sort_order tinyint,
    is_active bit default 1,

    FOREIGN KEY (brand_id) REFERENCES Brand(id),
);

-- Stores 
create table Store(
    id INT IDENTITY(1,1) PRIMARY KEY,
    public_id UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID() UNIQUE,
    slug nvarchar(200) unique,
    brand_id int not null,

    name nvarchar(200) not null,
    image_url varchar(255),
    address nvarchar(200) not null,
    latitude FLOAT,
    longitude FLOAT,
    open_time TIME,
    close_time TIME,
    is_active BIT DEFAULT 1,
    sort_order TINYINT DEFAULT 0,
    map_verified BIT DEFAULT 0,

    created_at DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (brand_id) REFERENCES Brand(id)
);

-- Policies
create table Policy(
    id INT IDENTITY(1,1) PRIMARY KEY,
    slug nvarchar(100) unique not null,
    brand_id int not null,

    title nvarchar(100) not null,
    content nvarchar(max) not null,
    is_active BIT DEFAULT 1,

    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (brand_id) REFERENCES Brand(id),
);

-- Users
create table [User](
    id INT IDENTITY(1,1) PRIMARY KEY,
    public_id UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID() UNIQUE,
    role_id TINYINT NOT NULL DEFAULT 1 CHECK (role_id IN (1, 2, 3)),

    username VARCHAR(50) NOT NULL,
    thumbnail_url varchar(200),
    email VARCHAR(100) UNIQUE NOT NULL,
    phone VARCHAR(20),
    password_hash VARCHAR(255) NOT NULL,
    current_coins INT DEFAULT 0 CHECK (current_coins >= 0),
    email_verified BIT DEFAULT 0,
    status TINYINT NOT NULL DEFAULT 1 CHECK (status IN (1, 2, 3)),

    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    last_login DATETIME,
);

-- News
create table News(
    id INT IDENTITY(1,1) PRIMARY KEY,
    public_id UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID() UNIQUE,
    slug nvarchar(200) unique,
    
   type NVARCHAR(50) NOT NULL,
    user_id int not null,

    title nvarchar(255) not null,
    content nvarchar(max) not null,
    thumbnail_url varchar(500),
    status NVARCHAR(20) DEFAULT 'Draft' CHECK (status IN ('Draft', 'Published', 'Archived')),
    
    is_featured BIT DEFAULT 0,
    seo_description NVARCHAR(255),

    published_date DATETIME,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (user_id) REFERENCES [User](id)
);

-- User Addresses
CREATE TABLE User_Address (
    id int IDENTITY(1,1) PRIMARY KEY,
    user_id int NOT NULL,
    full_address NVARCHAR(500) NOT NULL,
    is_default BIT DEFAULT 0,

    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),


    FOREIGN KEY (user_id) REFERENCES [User](id) ON DELETE CASCADE
);

-- Product Reviews
CREATE TABLE Review (
    id int IDENTITY(1,1) PRIMARY KEY,
    product_id int NOT NULL,
    user_id int NOT NULL,
    content NVARCHAR(MAX),
    rating TINYINT NOT NULL CHECK (rating >= 1 AND rating <= 5),
    status NVARCHAR(20) DEFAULT 'Pending' CHECK (status IN ('Pending', 'Approved', 'Rejected')),
    media_url VARCHAR(500),
    admin_response NVARCHAR(MAX),

    created_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (product_id) REFERENCES Product(id),
    FOREIGN KEY (user_id) REFERENCES [User](id)
);


-- News Comments
CREATE TABLE Comment (
    id int IDENTITY(1,1) PRIMARY KEY,
    parent_id int NULL,
    user_id int NOT NULL,
    news_id INT NOT NULL,
    content NVARCHAR(500) NOT NULL,
    status NVARCHAR(20) DEFAULT 'Pending',
    created_at DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (parent_id) REFERENCES Comment(id),
    FOREIGN KEY (user_id) REFERENCES [User](id),
    FOREIGN KEY (news_id) REFERENCES News(id)
);

-- Sizes
CREATE TABLE Size (
    id SMALLINT IDENTITY(1,1) PRIMARY KEY,
    label NVARCHAR(20) NOT NULL,
    price_modifier DECIMAL(18, 2) DEFAULT 0 CHECK (price_modifier >= 0),
    is_active BIT DEFAULT 1,
);

-- Ice Levels
CREATE TABLE Ice_Level (
    id SMALLINT IDENTITY(1,1) PRIMARY KEY,
    label NVARCHAR(20) NOT NULL,
    value SMALLINT NOT NULL CHECK (value >= -1 AND value <= 100),
    is_active BIT DEFAULT 1,
);

-- Sugar Levels
CREATE TABLE Sugar_Level (
    id SMALLINT IDENTITY(1,1) PRIMARY KEY,
    label NVARCHAR(20) NOT NULL,
    value SMALLINT NOT NULL CHECK ( value >= -1 AND value <= 100),
    is_active BIT DEFAULT 1,
);

-- Payment Methods
CREATE TABLE Payment_Method (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(50) NOT NULL,
    image_url VARCHAR(500),
    is_active BIT DEFAULT 1,
    sort_order TINYINT DEFAULT 0,
    processing_fee DECIMAL(5, 2) DEFAULT 0,
);

-- Order
create table [Order](
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    order_code VARCHAR(50) UNIQUE NOT NULL,
    user_id int NULL, -- NULL for guest orders
    store_id INT NOT NULL,
    payment_method_id INT NULL,
    
    order_date DATETIME DEFAULT GETDATE(),
    delivery_date DATETIME,
    total_amount DECIMAL(10,2) NOT NULL CHECK (total_amount >= 0),
    discount_amount DECIMAL(10,2) DEFAULT 0 CHECK (discount_amount >= 0),
    shipping_fee DECIMAL(8,2) DEFAULT 0 CHECK (shipping_fee >= 0),
    grand_total DECIMAL(10,2) NOT NULL CHECK (grand_total >= 0),
    coins_earned INT DEFAULT 0 CHECK (coins_earned >= 0),
    status TINYINT DEFAULT 1 CHECK (status BETWEEN 1 AND 6), -- 1:New, 2:Confirmed, 3:Preparing, 4:Ready, 5:Delivering, 6:Completed
    delivery_address NVARCHAR(500) NOT NULL,
    customer_phone VARCHAR(20) NOT NULL,
    customer_name NVARCHAR(100) NOT NULL,
    voucher_code_used VARCHAR(20),
    store_name NVARCHAR(200),
    user_notes NVARCHAR(500),

    created_at DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (user_id) REFERENCES [User](id),
    FOREIGN KEY (store_id) REFERENCES Store(id),
    FOREIGN KEY (payment_method_id) REFERENCES Payment_Method(id)
);

-- Order items
create table Order_item(
    id bigint identity(1,1) primary key,
    order_id bigint not null,
    product_id int not null,
    quantity int not null CHECK (quantity > 0),
    
    -- Đã sửa kiểu DECIMAL
    base_price decimal(10, 2) not null, 
    final_price decimal(10, 2) not null, 
    note nvarchar(255),
    
    -- Đã sửa sang NULLABLE
    parent_item_id bigint NULL, 
    size_id smallint NULL,
    sugar_level_id smallint NULL,
    ice_level_id smallint NULL,

    -- Bổ sung Khóa Ngoại
    FOREIGN KEY (order_id) REFERENCES [Order](id),
    FOREIGN KEY (product_id) REFERENCES Product(id),
    FOREIGN KEY (parent_item_id) REFERENCES Order_item(id), -- Tự tham chiếu
    FOREIGN KEY (size_id) REFERENCES Size(id),
    FOREIGN KEY (sugar_level_id) REFERENCES Sugar_Level(id),
    FOREIGN KEY (ice_level_id) REFERENCES Ice_Level(id)
);

CREATE TABLE Cart (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    user_id int UNIQUE NOT NULL,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (user_id) REFERENCES [User](id) ON DELETE CASCADE
);

-- Cart items
create table Cart_item(
    id bigint identity(1,1) primary key,
    cart_id bigint not null,
    product_id int not null,
    quantity int not null CHECK (quantity > 0),
    
    -- Đã sửa kiểu DECIMAL
    base_price decimal(10, 2) not null, 
    final_price decimal(10, 2) not null, 
    note nvarchar(255),
    
    -- Đã sửa sang NULLABLE
    parent_item_id bigint NULL, 
    size_id smallint NULL,
    sugar_level_id smallint NULL,
    ice_level_id smallint NULL,

    -- Bổ sung Khóa Ngoại
    FOREIGN KEY (cart_id) REFERENCES Cart(id),
    FOREIGN KEY (product_id) REFERENCES Product(id),
    FOREIGN KEY (parent_item_id) REFERENCES Cart_item(id), -- Tự tham chiếu
    FOREIGN KEY (size_id) REFERENCES Size(id),
    FOREIGN KEY (sugar_level_id) REFERENCES Sugar_Level(id),
    FOREIGN KEY (ice_level_id) REFERENCES Ice_Level(id)
);

-- Membership Levels
CREATE TABLE Membership_Level (
    id TINYINT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(35) NOT NULL UNIQUE CHECK (name IN (N'Đồng', N'Bạc', N'Vàng', N'Kim Cương')),
    min_spend_required DECIMAL(10,2) NOT NULL CHECK (min_spend_required >= 0),
    duration_days SMALLINT NOT NULL CHECK (duration_days > 0),
    benefits NVARCHAR(MAX), 
    created_at DATETIME DEFAULT GETDATE()
);

-- Memberships
CREATE TABLE Membership (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    user_id int UNIQUE NOT NULL,
    card_code VARCHAR(50) UNIQUE NOT NULL,
    level_id TINYINT NOT NULL,
    total_spent DECIMAL(12,2) DEFAULT 0 CHECK (total_spent >= 0),
    level_start_date DATE DEFAULT CAST(GETDATE() AS DATE),
    level_end_date DATE NOT NULL,
    last_level_spent_reset DATE NULL,
    status TINYINT DEFAULT 1,

    created_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (user_id) REFERENCES [User](id) ON DELETE CASCADE,
    FOREIGN KEY (level_id) REFERENCES Membership_Level(id)
);

-- Voucher Templates
CREATE TABLE Voucher_Template (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    level_id TINYINT NULL, -- NULL if voucher is for all levels
    
    discount_value DECIMAL(5,2) NOT NULL CHECK (discount_value > 0),
    discount_type VARCHAR(10) NOT NULL CHECK (discount_type IN ('Percent', 'Fixed')),
    min_order_value DECIMAL(10,2) DEFAULT 0 CHECK (min_order_value >= 0),
    max_discount_amount DECIMAL(10,2) NULL, -- For percentage discounts
    quantity_per_level TINYINT NULL CHECK (quantity_per_level >= 0),
    usage_limit INT NULL CHECK (usage_limit >= 0),
    used_count INT DEFAULT 0 CHECK (used_count >= 0),
    is_active BIT DEFAULT 1,
    usage_limit_per_user TINYINT NULL,
    coupon_code	VARCHAR(20) NULL UNIQUE,
    
    start_date DATETIME NOT NULL,
    end_date DATETIME NOT NULL,
    created_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (level_id) REFERENCES Membership_Level(id)
);

-- User Vouchers
CREATE TABLE User_Voucher (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    user_id int NOT NULL,
    voucher_template_id INT NOT NULL,
    voucher_code VARCHAR(20) UNIQUE NOT NULL,
    issued_date DATETIME DEFAULT GETDATE(),
    expiry_date DATETIME NOT NULL,
    status TINYINT DEFAULT 1 CHECK (status IN (1, 2, 3)), -- 1:Unused, 2:Used, 3:Expired
    used_date DATETIME NULL,
    order_id_used	BIGINT NULL
    
    FOREIGN KEY (user_id) REFERENCES [User](id),
    FOREIGN KEY (voucher_template_id) REFERENCES Voucher_Template(id)
);

CREATE TABLE Product_Size (
    product_id INT NOT NULL,
    size_id SMALLINT NOT NULL,

    -- Định nghĩa Khóa chính kép (Composite Primary Key)
    -- Đảm bảo một sản phẩm không thể có 2 lần cùng 1 size
    PRIMARY KEY (product_id, size_id),

    -- Định nghĩa Khóa ngoại
    FOREIGN KEY (product_id) REFERENCES Product(id),
    FOREIGN KEY (size_id) REFERENCES Size(id)
);

CREATE TABLE Product_Ice_Level (
    product_id INT NOT NULL,
    ice_level_id SMALLINT NOT NULL,

    -- Khóa chính kép
    PRIMARY KEY (product_id, ice_level_id),

    -- Khóa ngoại
    FOREIGN KEY (product_id) REFERENCES Product(id),
    FOREIGN KEY (ice_level_id) REFERENCES Ice_Level(id)
);

CREATE TABLE Product_Sugar_Level (
    product_id INT NOT NULL,
    sugar_level_id SMALLINT NOT NULL,

    -- Khóa chính kép
    PRIMARY KEY (product_id, sugar_level_id),

    -- Khóa ngoại
    FOREIGN KEY (product_id) REFERENCES Product(id),
    FOREIGN KEY (sugar_level_id) REFERENCES Sugar_Level(id)
);

CREATE TABLE Shop_Table (
    -- Khóa chính
    id INT IDENTITY(1,1) PRIMARY KEY, 
    
    -- Thông tin cơ bản
    store_id INT NOT NULL,              -- FK: Mã Cửa hàng
    name NVARCHAR(50) NOT NULL,         -- Tên/Số hiệu bàn (Ví dụ: "Bàn A1", "Bàn VIP 3")
    capacity TINYINT NOT NULL CHECK (capacity > 0), -- Sức chứa tối đa (người)
    
    -- Quản lý ghép bàn
    can_be_merged BIT DEFAULT 1,        -- Có thể được ghép với bàn khác không (1=Có, 0=Không)
    merged_with_table_id INT NULL,      -- FK: Mã bàn đang được ghép (NULL nếu không ghép)
    
    -- Trạng thái
    is_active BIT DEFAULT 1,            -- Trạng thái hoạt động (1=Hoạt động, 0=Tạm ẩn)

    -- Dấu thời gian
    created_at DATETIME DEFAULT GETDATE(),

    -- Khóa ngoại
    FOREIGN KEY (store_id) REFERENCES Store(id),
    -- Khóa ngoại tự tham chiếu cho Ghép bàn
    FOREIGN KEY (merged_with_table_id) REFERENCES [Table](id)
);

CREATE TABLE Reservation (
    -- Khóa chính
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    
    -- Định danh và liên kết
    reservation_code VARCHAR(50) UNIQUE NOT NULL, -- Mã đặt chỗ duy nhất
    user_id INT NULL,                           -- FK: Mã Khách hàng (NULL cho khách vãng lai)
    store_id INT NOT NULL,                      -- FK: Mã Cửa hàng
    
    -- Thời gian & Số lượng
    reservation_datetime DATETIME NOT NULL,     -- Thời gian đặt bàn (Ngày và Giờ)
    number_of_guests TINYINT NOT NULL CHECK (number_of_guests > 0), -- Số lượng khách
    deposit_amount DECIMAL(18, 2) DEFAULT 0, -- Giá cọc
    is_deposit_paid BIT DEFAULT 0 NOT NULL, -- Chuyển hay chưa
    
    -- Thông tin khách hàng
    customer_name NVARCHAR(100) NOT NULL,       -- Tên khách hàng
    customer_phone VARCHAR(20) NOT NULL,        -- SĐT khách hàng
    note NVARCHAR(500),                         -- Ghi chú từ khách hàng
    
    -- Trạng thái
    status TINYINT DEFAULT 1 CHECK (status BETWEEN 1 AND 6), -- 1:Pending, 2:Confirmed, 3:Arrived, 4:No-Show, 5:Cancelled, 6:Completed
    
    -- Bàn được gán (sau khi xác nhận)
    assigned_table_id INT NULL,                 -- FK: Mã bàn được gán (Có thể là bàn đơn hoặc bàn ghép)
    
    -- Dấu thời gian
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),

    -- Khóa ngoại
    FOREIGN KEY (user_id) REFERENCES [User](id),
    FOREIGN KEY (store_id) REFERENCES Store(id),
    FOREIGN KEY (assigned_table_id) REFERENCES [Table](id)
);
End;

-- Thêm thuộc tính
ALTER TABLE Reservation
ADD is_deposit_paid BIT DEFAULT 0 NOT NULL;

-- Tra cứu bảng
Select *from Size

-- Nới rộng cột giá trong bảng Size
ALTER TABLE Size
ALTER COLUMN price_modifier DECIMAL(18, 2);

-- Tiện tay nới luôn cho các bảng khác kẻo sau này lại lỗi tương tự
ALTER TABLE Product
ALTER COLUMN base_price DECIMAL(18, 2);