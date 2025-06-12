using Quote.Application.Core.Abstractions.Messaging;
using Quote.Domain.Core.Primitives.Result;

namespace Quote.Application.Category.Commands.CreateCategory;

public sealed record CreateCategoryCommand(string Name):ICommand<Result<Guid>>;