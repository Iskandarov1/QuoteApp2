using Quote.Application.Core.Abstractions.Data;
using Quote.Application.Core.Abstractions.Messaging;
using Quote.Contracts.Requests.CategoriesRequest;
using Quote.Domain.Core.Errors;
using Quote.Domain.Core.Localizations;
using Quote.Domain.Core.Primitives;
using Quote.Domain.Core.Primitives.Result;
using Quote.Domain.Repositories;

namespace Quote.Application.Category.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork,
    ISharedViewLocalizer sharedViewLocalizer) : ICommandHandler<UpdateCategoryCommand,Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var maybeCategory = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (maybeCategory.HasValue)
            return Result.Failure<Guid>(DomainErrors.Category.NotFound);

        var category = Domain.ValueObjects.Category.Create(request.Name,
            CaseConverter.PascalToSnakeCase(nameof(UpdateCategoryRequest.Name)), sharedViewLocalizer);
        
        categoryRepository.Update(
            maybeCategory.Value.Update(
                category.Value));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(maybeCategory.Value.Id);
    }
}