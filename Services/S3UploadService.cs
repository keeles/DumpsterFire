using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;

public class S3UploadService
{
    private readonly IAmazonS3 _s3Client;
    private readonly IConfiguration _configuration;

    public S3UploadService(IAmazonS3 s3Client, IConfiguration configuration)
    {
        _s3Client = s3Client;
        _configuration = configuration;
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        if (file == null || !ValidateFile(file))
            return "";

        try
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";

            var bucketName = _configuration["AWS:S3:BucketName"];
            var key = $"{uniqueFileName}";

            using var transferUtility = new TransferUtility(_s3Client);

            using var uploadStream = file.OpenReadStream();
            await transferUtility.UploadAsync(uploadStream, bucketName, key);
            Console.WriteLine("uploaded");
            return $"https://{bucketName}.s3.amazonaws.com/{key}";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"S3 Upload Error: {ex.Message}");
            return "";
        }
    }

    private bool ValidateFile(IFormFile file)
    {
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };

        if (!allowedTypes.Contains(file.ContentType))
            return false;

        return file.Length <= 5 * 1024 * 1024;
    }
}
