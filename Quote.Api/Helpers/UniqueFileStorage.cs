using System.Security.Cryptography;
using System.Text.Json;
using Quote.Domain.Repositories;

namespace Quote.Api.Helpers;

public sealed class UniqueFileStorage(
    IWebHostEnvironment env,
    ILogger<UniqueFileStorage> logger) : IUniqueFileStorage
{
    private readonly string _root = Path.Combine(env.ContentRootPath, "Logs");  
    private const string LogName = "upload.log";

    public string Save(IFormFile file, CancellationToken ct = default)
    {
        Directory.CreateDirectory(_root);

        using var sha = SHA256.Create();
        using var mem = new MemoryStream();
        file.CopyTo(mem);
        var hash = Convert.ToHexString(sha.ComputeHash(mem)).ToLowerInvariant();

        var ext = Path.GetExtension(file.FileName);
        var fileName = $"{hash}{ext}";
        var path = Path.Combine(_root, fileName);

        var action = "re-use";
        if (!File.Exists(path))
        {
            action = "save";
            mem.Position = 0;                     
            using var fs = File.Create(path, 81920, FileOptions.WriteThrough);
            mem.CopyTo(fs);
        }


        var fileInfo = new
        {
            Timestamp = DateTime.UtcNow,
            OriginalName = file.FileName,
            StoredName = fileName,
            Action = action,
            Size = file.Length,
            ContentType = file.ContentType,
            Hash = hash
        };


        var logLine = $"{DateTime.UtcNow:O}\t{JsonSerializer.Serialize(fileInfo)}{Environment.NewLine}";
        File.AppendAllText(Path.Combine(_root, LogName), logLine);


        logger.LogInformation("Upload {Action}: {OriginalName} ({Size} bytes) -> {StoredPath} with hash {Hash}",
            action, file.FileName, file.Length, path, hash);

        return path;                               
    }
}