using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace CkEditorAngularImageUpload.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageUploadController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImageUploadController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public ActionResult Post()
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;

            IFormFile file = httpContext.Request.Form.Files[0];

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                var fileBytes = memoryStream.ToArray();
                string base64String = Convert.ToBase64String(fileBytes);

                return new OkObjectResult(new
                {
                    Url = $"data:image/jpeg;base64,{base64String}",
                    Uploaded = 1,
                    File = file.Name
                });

            }
        }
    }
}
