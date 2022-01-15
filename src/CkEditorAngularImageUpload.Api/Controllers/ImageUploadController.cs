using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CkEditorAngularImageUpload.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageUploadController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public ImageUploadController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult> Post()
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            HttpContext httpContext = _httpContextAccessor.HttpContext;

            IFormFile formFile = httpContext.Request.Form.Files[0];

            string fileExtension = Path.GetExtension(formFile.FileName);

            var filename = $"{ Guid.NewGuid()}{fileExtension}";

            var filePath = Path.Combine(currentDirectory,"Uploads", filename);

            using (var stream = System.IO.File.Create(filePath))
            {
                await formFile.CopyToAsync(stream);
            }

            return new OkObjectResult(new
            {
                Url = $"{_configuration["ApplicationUrl"]}/uploads/{filename}",
                Uploaded = 1,
                File = filename
            });
        }
    }
}
