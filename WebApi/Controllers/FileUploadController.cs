using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("[controller]")]

public class FileUploadController : ControllerBase
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileUploadController(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

     [HttpGet("GetList")]
     public List<string>GetQuotesByCategory()
     {
        var list = new List<String>();
        var path = Path.Combine(_webHostEnvironment.WebRootPath,"images");
        var files = Directory.GetFiles(path);
        list.AddRange(files);
        var directories = Directory.GetDirectories(path);
        list.AddRange(directories);
        return list.ToList();
     }




    [HttpPost("UploadFile")]

    public string GetQuotesById(IFormFile file)
    {
        var curentFolder = _webHostEnvironment.WebRootPath;
        var fullPath = Path.Combine(curentFolder,"images",file.FileName);
        using (var stream = new FileStream(fullPath,FileMode.CreateNew))
        {
            file.CopyTo(stream);
        }
        
        return fullPath;
    }
}
