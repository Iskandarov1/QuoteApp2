using Quote.Application.Core.Abstractions.Data;
using Quote.Application.Core.Abstractions.Messaging;
using Quote.Domain.Core.Errors;
using Quote.Domain.Core.Localizations;
using Quote.Domain.Core.Primitives.Result;
using Quote.Domain.Repositories;

namespace Quote.Application.Category.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork):ICommandHandler<DeleteCategoryCommand,Result>
{
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var maybeCategory = await categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (maybeCategory.HasNoValue)
            return Result.Failure(DomainErrors.Category.NotFound);
        
        categoryRepository.Remove(maybeCategory.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}