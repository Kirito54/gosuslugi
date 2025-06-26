using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Services;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/files")]
public class FileController : ControllerBase
{
    private readonly MinioService _minio;
    private const string Bucket = "files";

    public FileController(MinioService minio)
    {
        _minio = minio;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        await _minio.CreateBucketIfNotExistsAsync(Bucket);
        await using var stream = file.OpenReadStream();
        await _minio.UploadAsync(stream, Bucket, file.FileName, file.ContentType);
        return Ok();
    }

    [HttpGet("download/{fileName}")]
    public async Task<IActionResult> Download(string fileName)
    {
        var stream = await _minio.DownloadAsync(Bucket, fileName);
        return File(stream, "application/octet-stream", fileName);
    }

    [HttpDelete("{fileName}")]
    public async Task<IActionResult> Delete(string fileName)
    {
        await _minio.DeleteAsync(Bucket, fileName);
        return NoContent();
    }

    [HttpGet("list")]
    public async Task<IActionResult> List()
    {
        var files = await _minio.ListFilesAsync(Bucket);
        return Ok(files);
    }

    [HttpPut("update/{fileName}")]
    public async Task<IActionResult> Update(string fileName, IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        await _minio.UploadAsync(stream, Bucket, fileName, file.ContentType);
        return Ok();
    }
}
