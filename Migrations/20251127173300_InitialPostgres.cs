using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace drinking_be.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "brand",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    logo_url = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: true),
                    address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    hotline = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    email_support = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    tax_code = table.Column<string>(type: "character varying(30)", unicode: false, maxLength: 30, nullable: true),
                    company_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    slogan = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    copyright_text = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Brand__3213E83F3D8FA9ED", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    parent_id = table.Column<int>(type: "integer", nullable: true),
                    slug = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    sort_order = table.Column<byte>(type: "smallint", nullable: true, defaultValue: (byte)0),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Category__3213E83F5A39BDEA", x => x.id);
                    table.ForeignKey(
                        name: "FK__Category__parent__3C69FB99",
                        column: x => x.parent_id,
                        principalTable: "category",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ice_level",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    value = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ice_Leve__3213E83FD11018A5", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "membership_level",
                columns: table => new
                {
                    id = table.Column<byte>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(35)", maxLength: 35, nullable: false),
                    min_spend_required = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    duration_days = table.Column<short>(type: "smallint", nullable: false),
                    benefits = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Membersh__3213E83F3901FC68", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "payment_method",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    image_url = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    sort_order = table.Column<byte>(type: "smallint", nullable: true, defaultValue: (byte)0),
                    processing_fee = table.Column<decimal>(type: "numeric(5,2)", nullable: true, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payment___3213E83F7896F946", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "size",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    price_modifier = table.Column<decimal>(type: "numeric(5,2)", nullable: true, defaultValue: 0m),
                    is_active = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Size__3213E83F985D1A04", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sugar_level",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    value = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Sugar_Le__3213E83F86537186", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    public_id = table.Column<Guid>(type: "uuid", nullable: true, defaultValueSql: "(newsequentialid())"),
                    role_id = table.Column<byte>(type: "smallint", nullable: false, defaultValue: (byte)1),
                    username = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    thumbnail_url = table.Column<string>(type: "character varying(200)", unicode: false, maxLength: 200, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    password_hash = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: false),
                    current_coins = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    email_verified = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    status = table.Column<byte>(type: "smallint", nullable: false, defaultValue: (byte)1),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    last_login = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__3213E83F0D985B4B", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "policy",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    brand_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Policy__3213E83FB49E512D", x => x.id);
                    table.ForeignKey(
                        name: "FK__Policy__brand_id__5BE2A6F2",
                        column: x => x.brand_id,
                        principalTable: "brand",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "social_media",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    brand_id = table.Column<int>(type: "integer", nullable: false),
                    platform_name = table.Column<string>(type: "character varying(30)", unicode: false, maxLength: 30, nullable: false),
                    url = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: false),
                    icon_url = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: true),
                    sort_order = table.Column<byte>(type: "smallint", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_social_media", x => x.id);
                    table.ForeignKey(
                        name: "FK_social_media_brand_brand_id",
                        column: x => x.brand_id,
                        principalTable: "brand",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "store",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    public_id = table.Column<Guid>(type: "uuid", nullable: true, defaultValueSql: "(newsequentialid())"),
                    slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    brand_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    image_url = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: true),
                    address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: true),
                    longitude = table.Column<double>(type: "double precision", nullable: true),
                    open_time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    close_time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    sort_order = table.Column<byte>(type: "smallint", nullable: true, defaultValue: (byte)0),
                    map_verified = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Store__3213E83FCF8EAB0E", x => x.id);
                    table.ForeignKey(
                        name: "FK__Store__brand_id__5535A963",
                        column: x => x.brand_id,
                        principalTable: "brand",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    public_id = table.Column<string>(type: "character varying(36)", unicode: false, maxLength: 36, nullable: false),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    product_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    base_price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    image_url = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    ingredient = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    total_rating = table.Column<double>(type: "double precision", nullable: true, defaultValue: 0.0),
                    total_sold = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    search_vector = table.Column<byte[]>(type: "bytea", nullable: true),
                    launch_date_time = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Product__3213E83F0F2383EA", x => x.id);
                    table.ForeignKey(
                        name: "FK__Product__updated__440B1D61",
                        column: x => x.category_id,
                        principalTable: "category",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "voucher_template",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    level_id = table.Column<byte>(type: "smallint", nullable: true),
                    discount_value = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    discount_type = table.Column<string>(type: "character varying(10)", unicode: false, maxLength: 10, nullable: false),
                    min_order_value = table.Column<decimal>(type: "numeric(10,2)", nullable: true, defaultValue: 0m),
                    max_discount_amount = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    quantity_per_level = table.Column<byte>(type: "smallint", nullable: true),
                    usage_limit = table.Column<int>(type: "integer", nullable: true),
                    used_count = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    is_active = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    usage_limit_per_user = table.Column<byte>(type: "smallint", nullable: true),
                    coupon_code = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    start_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Voucher___3213E83F0298D293", x => x.id);
                    table.ForeignKey(
                        name: "FK__Voucher_T__level__5E8A0973",
                        column: x => x.level_id,
                        principalTable: "membership_level",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "cart",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cart__3213E83FED0FE127", x => x.id);
                    table.ForeignKey(
                        name: "FK__Cart__user_id__37703C52",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "membership",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    card_code = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    level_id = table.Column<byte>(type: "smallint", nullable: false),
                    total_spent = table.Column<decimal>(type: "numeric(12,2)", nullable: true, defaultValue: 0m),
                    level_start_date = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "(CONVERT([date],getdate()))"),
                    level_end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    last_level_spent_reset = table.Column<DateOnly>(type: "date", nullable: true),
                    status = table.Column<byte>(type: "smallint", nullable: true, defaultValue: (byte)1),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Membersh__3213E83FF483D767", x => x.id);
                    table.ForeignKey(
                        name: "FK__Membershi__level__51300E55",
                        column: x => x.level_id,
                        principalTable: "membership_level",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Membershi__user___503BEA1C",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "news",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    public_id = table.Column<Guid>(type: "uuid", nullable: true, defaultValueSql: "(newsequentialid())"),
                    slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    thumbnail_url = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValue: "Draft"),
                    is_featured = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    seo_description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    published_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__News__3213E83F6A9F9780", x => x.id);
                    table.ForeignKey(
                        name: "FK__News__user_id__73BA3083",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user_address",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    full_address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User_Add__3213E83FE51862F8", x => x.id);
                    table.ForeignKey(
                        name: "FK__User_Addr__user___787EE5A0",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_code = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    store_id = table.Column<int>(type: "integer", nullable: false),
                    payment_method_id = table.Column<int>(type: "integer", nullable: true),
                    order_date = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    delivery_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    total_amount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    discount_amount = table.Column<decimal>(type: "numeric(10,2)", nullable: true, defaultValue: 0m),
                    shipping_fee = table.Column<decimal>(type: "numeric(8,2)", nullable: true, defaultValue: 0m),
                    grand_total = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    coins_earned = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    status = table.Column<byte>(type: "smallint", nullable: true, defaultValue: (byte)1),
                    delivery_address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    customer_phone = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false),
                    customer_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    voucher_code_used = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    store_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    user_notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Order__3213E83FDE5887F1", x => x.id);
                    table.ForeignKey(
                        name: "FK__Order__payment_m__29221CFB",
                        column: x => x.payment_method_id,
                        principalTable: "payment_method",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Order__store_id__282DF8C2",
                        column: x => x.store_id,
                        principalTable: "store",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Order__user_id__2739D489",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "shop_table",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    store_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    capacity = table.Column<byte>(type: "smallint", nullable: false),
                    can_be_merged = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    merged_with_table_id = table.Column<int>(type: "integer", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Table__3213E83FCBCE513F", x => x.id);
                    table.ForeignKey(
                        name: "FK__Table__merged_wi__0D44F85C",
                        column: x => x.merged_with_table_id,
                        principalTable: "shop_table",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Table__store_id__0C50D423",
                        column: x => x.store_id,
                        principalTable: "store",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "producticelevels",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    IceLevelId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_producticelevels", x => new { x.ProductId, x.IceLevelId });
                    table.ForeignKey(
                        name: "FK_producticelevels_ice_level_IceLevelId",
                        column: x => x.IceLevelId,
                        principalTable: "ice_level",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_producticelevels_product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "productsizes",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    SizeId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productsizes", x => new { x.ProductId, x.SizeId });
                    table.ForeignKey(
                        name: "FK_productsizes_product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_productsizes_size_SizeId",
                        column: x => x.SizeId,
                        principalTable: "size",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "productsugarlevels",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    SugarLevelId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productsugarlevels", x => new { x.ProductId, x.SugarLevelId });
                    table.ForeignKey(
                        name: "FK_productsugarlevels_product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_productsugarlevels_sugar_level_SugarLevelId",
                        column: x => x.SugarLevelId,
                        principalTable: "sugar_level",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "review",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    content = table.Column<string>(type: "text", nullable: true),
                    rating = table.Column<byte>(type: "smallint", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValue: "Pending"),
                    media_url = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: true),
                    admin_response = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Review__3213E83F5E4BD5F0", x => x.id);
                    table.ForeignKey(
                        name: "FK__Review__product___7F2BE32F",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Review__user_id__00200768",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user_voucher",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    voucher_template_id = table.Column<int>(type: "integer", nullable: false),
                    voucher_code = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false),
                    issued_date = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    expiry_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    status = table.Column<byte>(type: "smallint", nullable: true, defaultValue: (byte)1),
                    used_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    order_id_used = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User_Vou__3213E83F950C18C0", x => x.id);
                    table.ForeignKey(
                        name: "FK__User_Vouc__order__65370702",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__User_Vouc__vouch__662B2B3B",
                        column: x => x.voucher_template_id,
                        principalTable: "voucher_template",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "cart_item",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cart_id = table.Column<long>(type: "bigint", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    base_price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    final_price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    note = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    parent_item_id = table.Column<long>(type: "bigint", nullable: true),
                    size_id = table.Column<short>(type: "smallint", nullable: true),
                    sugar_level_id = table.Column<short>(type: "smallint", nullable: true),
                    ice_level_id = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cart_ite__3213E83F486E79A8", x => x.id);
                    table.ForeignKey(
                        name: "FK__Cart_item__cart___3B40CD36",
                        column: x => x.cart_id,
                        principalTable: "cart",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Cart_item__ice_l__40058253",
                        column: x => x.ice_level_id,
                        principalTable: "ice_level",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Cart_item__paren__3D2915A8",
                        column: x => x.parent_item_id,
                        principalTable: "cart_item",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Cart_item__produ__3C34F16F",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Cart_item__size___3E1D39E1",
                        column: x => x.size_id,
                        principalTable: "size",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Cart_item__sugar__3F115E1A",
                        column: x => x.sugar_level_id,
                        principalTable: "sugar_level",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "comment",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    parent_id = table.Column<int>(type: "integer", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    news_id = table.Column<int>(type: "integer", nullable: false),
                    content = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValue: "Pending"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Comment__3213E83FE8B13CD2", x => x.id);
                    table.ForeignKey(
                        name: "FK__Comment__news_id__06CD04F7",
                        column: x => x.news_id,
                        principalTable: "news",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Comment__parent___04E4BC85",
                        column: x => x.parent_id,
                        principalTable: "comment",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Comment__user_id__05D8E0BE",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "order_item",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_id = table.Column<long>(type: "bigint", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    base_price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    final_price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    note = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    parent_item_id = table.Column<long>(type: "bigint", nullable: true),
                    size_id = table.Column<short>(type: "smallint", nullable: true),
                    sugar_level_id = table.Column<short>(type: "smallint", nullable: true),
                    ice_level_id = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Order_it__3213E83F36FD5A45", x => x.id);
                    table.ForeignKey(
                        name: "FK__Order_ite__ice_l__31B762FC",
                        column: x => x.ice_level_id,
                        principalTable: "ice_level",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Order_ite__order__2CF2ADDF",
                        column: x => x.order_id,
                        principalTable: "order",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Order_ite__paren__2EDAF651",
                        column: x => x.parent_item_id,
                        principalTable: "order_item",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Order_ite__produ__2DE6D218",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Order_ite__size___2FCF1A8A",
                        column: x => x.size_id,
                        principalTable: "size",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Order_ite__sugar__30C33EC3",
                        column: x => x.sugar_level_id,
                        principalTable: "sugar_level",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "reservation",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    reservation_code = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    store_id = table.Column<int>(type: "integer", nullable: false),
                    reservation_datetime = table.Column<DateTime>(type: "datetime", nullable: false),
                    number_of_guests = table.Column<byte>(type: "smallint", nullable: false),
                    customer_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    customer_phone = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false),
                    note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    status = table.Column<byte>(type: "smallint", nullable: true, defaultValue: (byte)1),
                    assigned_table_id = table.Column<int>(type: "integer", nullable: true),
                    DepositAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    IsDepositPaid = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reservat__3213E83F5F2BFAAF", x => x.id);
                    table.ForeignKey(
                        name: "FK__Reservati__assig__17C286CF",
                        column: x => x.assigned_table_id,
                        principalTable: "shop_table",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Reservati__store__16CE6296",
                        column: x => x.store_id,
                        principalTable: "store",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Reservati__user___15DA3E5D",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Cart__B9BE370E2DCD722C",
                table: "cart",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cart_item_cart_id",
                table: "cart_item",
                column: "cart_id");

            migrationBuilder.CreateIndex(
                name: "IX_cart_item_ice_level_id",
                table: "cart_item",
                column: "ice_level_id");

            migrationBuilder.CreateIndex(
                name: "IX_cart_item_parent_item_id",
                table: "cart_item",
                column: "parent_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_cart_item_product_id",
                table: "cart_item",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_cart_item_size_id",
                table: "cart_item",
                column: "size_id");

            migrationBuilder.CreateIndex(
                name: "IX_cart_item_sugar_level_id",
                table: "cart_item",
                column: "sugar_level_id");

            migrationBuilder.CreateIndex(
                name: "IX_category_parent_id",
                table: "category",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Category__32DD1E4C00F59B8C",
                table: "category",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_comment_news_id",
                table: "comment",
                column: "news_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_parent_id",
                table: "comment",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_user_id",
                table: "comment",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_membership_level_id",
                table: "membership",
                column: "level_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Membersh__81703D727EAE3828",
                table: "membership",
                column: "card_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Membersh__B9BE370EFC41B82F",
                table: "membership",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Membersh__72E12F1BBCF20D34",
                table: "membership_level",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_news_user_id",
                table: "news",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UQ__News__32DD1E4C5A174899",
                table: "news",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__News__5699A53062598161",
                table: "news",
                column: "public_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_order_payment_method_id",
                table: "order",
                column: "payment_method_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_store_id",
                table: "order",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_user_id",
                table: "order",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Order__99D12D3FD8ECB07F",
                table: "order",
                column: "order_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_order_item_ice_level_id",
                table: "order_item",
                column: "ice_level_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_order_id",
                table: "order_item",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_parent_item_id",
                table: "order_item",
                column: "parent_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_product_id",
                table: "order_item",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_size_id",
                table: "order_item",
                column: "size_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_sugar_level_id",
                table: "order_item",
                column: "sugar_level_id");

            migrationBuilder.CreateIndex(
                name: "IX_policy_brand_id",
                table: "policy",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Policy__32DD1E4C980885E3",
                table: "policy",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_category_id",
                table: "product",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Product__32DD1E4CAB867B39",
                table: "product",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Product__5699A530AE3E7BDD",
                table: "product",
                column: "public_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_producticelevels_IceLevelId",
                table: "producticelevels",
                column: "IceLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_productsizes_SizeId",
                table: "productsizes",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_productsugarlevels_SugarLevelId",
                table: "productsugarlevels",
                column: "SugarLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_reservation_assigned_table_id",
                table: "reservation",
                column: "assigned_table_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservation_store_id",
                table: "reservation",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservation_user_id",
                table: "reservation",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Reservat__FA8FADE431DEB25F",
                table: "reservation",
                column: "reservation_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_review_product_id",
                table: "review",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_review_user_id",
                table: "review",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_shop_table_merged_with_table_id",
                table: "shop_table",
                column: "merged_with_table_id");

            migrationBuilder.CreateIndex(
                name: "IX_shop_table_store_id",
                table: "shop_table",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "IX_social_media_brand_id",
                table: "social_media",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "IX_store_brand_id",
                table: "store",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Store__32DD1E4C92725A74",
                table: "store",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Store__5699A53072383001",
                table: "store",
                column: "public_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__User__5699A530AE2F5693",
                table: "user",
                column: "public_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__User__AB6E616418E7B215",
                table: "user",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_address_user_id",
                table: "user_address",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_voucher_user_id",
                table: "user_voucher",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_voucher_voucher_template_id",
                table: "user_voucher",
                column: "voucher_template_id");

            migrationBuilder.CreateIndex(
                name: "UQ__User_Vou__21731069F90ADEF0",
                table: "user_voucher",
                column: "voucher_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_voucher_template_level_id",
                table: "voucher_template",
                column: "level_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Voucher___ADE5CBB794CB76DB",
                table: "voucher_template",
                column: "coupon_code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cart_item");

            migrationBuilder.DropTable(
                name: "comment");

            migrationBuilder.DropTable(
                name: "membership");

            migrationBuilder.DropTable(
                name: "order_item");

            migrationBuilder.DropTable(
                name: "policy");

            migrationBuilder.DropTable(
                name: "producticelevels");

            migrationBuilder.DropTable(
                name: "productsizes");

            migrationBuilder.DropTable(
                name: "productsugarlevels");

            migrationBuilder.DropTable(
                name: "reservation");

            migrationBuilder.DropTable(
                name: "review");

            migrationBuilder.DropTable(
                name: "social_media");

            migrationBuilder.DropTable(
                name: "user_address");

            migrationBuilder.DropTable(
                name: "user_voucher");

            migrationBuilder.DropTable(
                name: "cart");

            migrationBuilder.DropTable(
                name: "news");

            migrationBuilder.DropTable(
                name: "order");

            migrationBuilder.DropTable(
                name: "ice_level");

            migrationBuilder.DropTable(
                name: "size");

            migrationBuilder.DropTable(
                name: "sugar_level");

            migrationBuilder.DropTable(
                name: "shop_table");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "voucher_template");

            migrationBuilder.DropTable(
                name: "payment_method");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "store");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "membership_level");

            migrationBuilder.DropTable(
                name: "brand");
        }
    }
}
