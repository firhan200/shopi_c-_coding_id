using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopiApi.Dto.Product;
using ShopiApi.Models;
using ShopiApi.Repositories;
using ShopiApi.Services;
using System.Security.Claims;

namespace ShopiApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository _productRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ProductRepository productRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(_productRepository.GetAll());
        }

        [Authorize]
        [HttpGet("/GetUserId")]
        public ActionResult GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.Sid);
            var role = User.FindFirstValue(ClaimTypes.Role);

            return Ok(new { UserId = userId, Role = role });
        }

        [HttpGet("GetAll/WithAdapter")]
        public ActionResult GetAllWithAdapter()
        {
            return Ok(_productRepository.GetAllWithAdapter());
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> Create([FromForm] CreateProductDto data)
        {
            IFormFile image = data.Image!;

            // TODO: save image to server
            var ext = Path.GetExtension(image.FileName).ToLowerInvariant(); //.jpg

            //get filename
            string fileName = Guid.NewGuid().ToString() + ext; //pasti unik
            string uploadDir = "uploads"; //foldering biar rapih
            string physicalPath = $"wwwroot/{uploadDir}";
            //saving image
            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, physicalPath, fileName); // D:projects/shopi/wwwroot/uploads/asdasd.png
            using var stream = System.IO.File.Create(filePath);
            await image.CopyToAsync(stream);

            //create url path
            string fileUrlPath = $"{uploadDir}/{fileName}";

            string errorMessage = _productRepository.Create(data.Name, data.Description, data.Price, fileUrlPath);

            // kalau ada isi nya
            if (!string.IsNullOrEmpty(errorMessage)) { 
                // berarti ada error
                return Problem(errorMessage);
            }

            // kalau tidak maka OKE
            return Ok();
        }
    }
}
