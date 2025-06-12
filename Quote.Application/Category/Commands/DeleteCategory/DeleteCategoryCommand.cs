using Quote.Application.Core.Abstractions.Messaging;
using Quote.Domain.Core.Primitives.Result;

namespace Quote.Application.Category.Commands.DeleteCategory;

public sealed record DeleteCategoryCommand(Guid CategoryId):ICommand<Result>;