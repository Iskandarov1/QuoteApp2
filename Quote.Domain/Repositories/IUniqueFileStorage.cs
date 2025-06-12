using Microsoft.AspNetCore.Http;

namespace Quote.Domain.Repositories;

public interface IUniqueFileStorage
{
    string Save(IFormFile file, CancellationToken ct = default);
}