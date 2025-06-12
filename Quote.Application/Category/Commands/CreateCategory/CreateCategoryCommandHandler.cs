using Quote.Application.Core.Abstractions.Data;
using Quote.Application.Core.Abstractions.Messaging;
using Quote.Contracts.Requests.CategoriesRequest;
using Quote.Domain.Core.Errors;
using Quote.Domain.Core.Localizations;
using Quote.Domain.Core.Primitives;
using Quote.Domain.Core.Primitives.Maybe;
using Quote.Domain.Core.Primitives.Result;
using Quote.Domain.Repositories;

namespace Quote.Application.Category.Commands.CreateCategory;

public class CreateCategoryCommandHandler(
    ISharedViewLocalizer sharedViewLocalizer,
    IUnitOfWork unitOfWork,
    ICategoryRepository categoryRepository):ICommandHandler<CreateCategoryCommand,Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var existingCategory = await categoryRepository.GetByNameAsync(request.Name, cancellationToken);
        if (existingCategory.HasValue)
            return Result.Failure<Guid>(DomainErrors.Category.AlreadyExists);
        
        var categoryName = Domain.ValueObjects.Category.Create(request.Name,CaseConverter.PascalToSnakeCase(nameof(CreateCategoryRequest.Name)),sharedViewLocalizer);
        if (categoryName.IsFailure)
            return Result.Failure<Guid>(categoryName.Error);

        var category = new Domain.Entities.Category(categoryName.Value);
        
        categoryRepository.Insert(category);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(category.Id);

    }
}