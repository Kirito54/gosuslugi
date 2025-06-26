using Minio;
using Minio.DataModel.Args;
using Microsoft.Extensions.Configuration;

namespace GovServices.Server.Services;

public class MinioService
{
    private readonly IMinioClient _client;

    public MinioService(IConfiguration configuration)
    {
        var section = configuration.GetSection("Minio");
        var endpoint = section["Endpoint"] ?? "localhost:9000";
        var accessKey = section["AccessKey"] ?? string.Empty;
        var secretKey = section["SecretKey"] ?? string.Empty;
        var useSsl = section.GetValue<bool>("UseSSL");

        _client = new MinioClient()
            .WithEndpoint(endpoint)
            .WithCredentials(accessKey, secretKey)
            .WithSSL(useSsl)
            .Build();
    }

    public async Task UploadAsync(Stream stream, string bucket, string name, string contentType)
    {
        var args = new PutObjectArgs()
            .WithBucket(bucket)
            .WithObject(name)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType(contentType);
        await _client.PutObjectAsync(args);
    }

    public async Task<Stream> DownloadAsync(string bucket, string name)
    {
        var memory = new MemoryStream();
        var args = new GetObjectArgs()
            .WithBucket(bucket)
            .WithObject(name)
            .WithCallbackStream(s => s.CopyTo(memory));
        await _client.GetObjectAsync(args);
        memory.Position = 0;
        return memory;
    }

    public Task DeleteAsync(string bucket, string name)
    {
        var args = new RemoveObjectArgs()
            .WithBucket(bucket)
            .WithObject(name);
        return _client.RemoveObjectAsync(args);
    }

    public async Task<bool> ExistsAsync(string bucket, string name)
    {
        try
        {
            var args = new StatObjectArgs()
                .WithBucket(bucket)
                .WithObject(name);
            await _client.StatObjectAsync(args);
            return true;
        }
        catch (Minio.Exceptions.ObjectNotFoundException)
        {
            return false;
        }
    }

    public async Task<IEnumerable<string>> ListFilesAsync(string bucket)
    {
        var results = new List<string>();
        var args = new ListObjectsArgs()
            .WithBucket(bucket)
            .WithRecursive(true);
        await foreach (var item in _client.ListObjectsEnumAsync(args))
        {
            results.Add(item.Key);
        }
        return results;
    }

    public async Task CreateBucketIfNotExistsAsync(string bucket)
    {
        var existsArgs = new BucketExistsArgs().WithBucket(bucket);
        bool exists = await _client.BucketExistsAsync(existsArgs);
        if (!exists)
        {
            var makeArgs = new MakeBucketArgs().WithBucket(bucket);
            await _client.MakeBucketAsync(makeArgs);
        }
    }
}
