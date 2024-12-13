using Dotnet.Homeworks.Storage.API.Dto.Internal;
using Minio;

namespace Dotnet.Homeworks.Storage.API.Services;

public class StorageFactory : IStorageFactory
{
    private readonly IMinioClient _minioClient;

    public StorageFactory(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    public async Task<IStorage<Image>> CreateImageStorageWithinBucketAsync(string bucketName)
    {
        await MakeBucketIfNotExistsAsync(bucketName);

        return new ImageStorage(bucketName, _minioClient);
    }
    
    private async Task MakeBucketIfNotExistsAsync(string bucket, CancellationToken cancellationToken = default)
    {
        var isBucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs()
            .WithBucket(bucket), cancellationToken);

        if (isBucketExists)
        {
            return;
        }

        await _minioClient.MakeBucketAsync(new MakeBucketArgs()
            .WithBucket(bucket), cancellationToken);
    }
}