// Controllers/NewsController.cs
using drinking_be.Dtos.NewsDtos;
using drinking_be.Interfaces.NewsInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace drinking_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        /// <summary>
        /// API phục vụ Carousel/Slider (Frontend gọi: GET /api/carousel)
        /// </summary>
        [HttpGet("/api/carousel")] // ⭐️ Route tuyệt đối
        public IActionResult GetCarousel()
        {
            // Vì chưa có bảng Carousel riêng, ta trả về dữ liệu giả lập (Mock)
            // hoặc bạn có thể Query từ bảng News/Products nếu muốn.
            var slides = new List<object>
            {
                new {
                    id = 1,
                    imageUrl = "https://img.freepik.com/free-vector/flat-design-bubble-tea-banner-template_23-2149463197.jpg",
                    title = "Chào hè rực rỡ",
                    link = "/menu"
                },
                new {
                    id = 2,
                    imageUrl = "https://img.freepik.com/free-vector/flat-bubble-tea-banner-template_23-2149463198.jpg",
                    title = "Món mới: Trà sữa nướng",
                    link = "/products/tra-sua-nuong"
                }
            };
            return Ok(slides);
        }

        // --- PUBLIC ENDPOINTS (Khách hàng) ---

        /// <summary>
        /// Lấy danh sách tất cả các bài viết đã xuất bản.
        /// </summary>
        /// <returns>Danh sách NewsReadDto.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<NewsReadDto>))]
        public async Task<IActionResult> GetPublishedNews()
        {
            var newsList = await _newsService.GetPublishedNewsAsync();
            return Ok(newsList);
        }

        /// <summary>
        /// Lấy chi tiết một bài viết đã xuất bản dựa trên Slug.
        /// </summary>
        /// <param name="slug">Slug của bài viết.</param>
        /// <returns>NewsReadDto chi tiết.</returns>
        [HttpGet("{slug}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NewsReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetNewsBySlug(string slug)
        {
            var news = await _newsService.GetNewsBySlugAsync(slug);
            if (news == null)
            {
                return NotFound("Không tìm thấy bài viết hoặc bài viết chưa được xuất bản.");
            }
            return Ok(news);
        }

        // --- ADMIN ENDPOINT ---

        /// <summary>
        /// [ADMIN] Tạo bài viết tin tức mới.
        /// </summary>
        /// <param name="newsDto">Dữ liệu tạo bài viết.</param>
        /// <returns>NewsReadDto của bài viết đã tạo.</returns>
        // Cần thêm [Authorize(Roles = "Admin")] nếu triển khai xác thực
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NewsReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNews([FromBody] NewsCreateDto newsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdNews = await _newsService.CreateNewsAsync(newsDto);

                return CreatedAtAction(nameof(GetNewsBySlug),
                                       new { slug = createdNews.Slug },
                                       createdNews);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nghiệp vụ (ví dụ: UserID không hợp lệ)
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }
    }
}