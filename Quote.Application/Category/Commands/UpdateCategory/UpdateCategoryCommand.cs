using Quote.Application.Core.Abstractions.Messaging;
using Quote.Domain.Core.Primitives.Result;

namespace Quote.Application.Category.Commands.UpdateCategory;

public sealed record UpdateCategoryCommand(
    Guid CategoryId, 
    string Name) : ICommand<Result<Guid>>;