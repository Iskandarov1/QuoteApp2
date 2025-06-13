using System.Security.Cryptography;
using Quote.Domain.Repositories;

namespace Quote.Api.Contracts;


    public sealed class UniqueFileStorage(
        IWebHostEnvironment env,
        ILogger<UniqueFileStorage> logger) : IUniqueFileStorage
    {
        private readonly string _root = Path.Combine(env.ContentRootPath, "Forms");  
        private const string LogName = "upload.log";

        public string Save(IFormFile file, CancellationToken ct = default)
        {
            Directory.CreateDirectory(_root);

            // ---------- 1. compute hash ----------
            using var sha  = SHA256.Create();
            using var mem  = new MemoryStream();
            file.CopyTo(mem);
            var hash = Convert.ToHexString(sha.ComputeHash(mem)).ToLowerInvariant();

            // ---------- 2. final path ----------
            var ext      = Path.GetExtension(file.FileName);
            var fileName = $"{hash}{ext}";
            var path     = Path.Combine(_root, fileName);

            var action = "re-use";
            if (!File.Exists(path))
            {
                action = "save";
                mem.Position = 0;                     
                 using var fs = File.Create(path, 81920, FileOptions.WriteThrough);
                mem.CopyTo(fs);
            }


            var logLine =
                $"{DateTime.UtcNow:O}\t{file.FileName}\t{fileName}\t{action}{Environment.NewLine}";
            File.AppendAllText(Path.Combine(_root, LogName), logLine);

            logger.LogInformation("Upload {Action}: {Original} -> {Stored}",
                action, file.FileName, path);

            return path;                               
        }
    }