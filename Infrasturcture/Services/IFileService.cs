using Microsoft.AspNetCore.Http;

namespace Infrasturcture.Services;

public interface  IFileService
{
    string CreateFile(string folder,IFormFile file);
    bool DeleteFile(string folder,string Filename);
}
