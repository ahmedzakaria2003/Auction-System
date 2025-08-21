using AuctionSystem.Application.Services.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;

    public FileService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveFileAsync(IFormFile file, string folderName)
    {
        
            var wwwRootPath = _env.WebRootPath;
            var folderPath = Path.Combine(wwwRootPath, folderName);
            Directory.CreateDirectory(folderPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var fullPath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/{folderName}/{fileName}";
     
    }

    public async Task DeleteFileAsync(string relativePath)
    {
        try {
            var fullPath = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                await Task.Run(() => File.Delete(fullPath));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while deleting file: {ex.Message}");

        }
    }
}
