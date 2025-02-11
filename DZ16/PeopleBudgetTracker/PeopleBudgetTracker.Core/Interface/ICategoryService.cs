using PeopleBudgetTracker.Core.DTOs;

namespace PeopleBudgetTracker.Core.Interfaces;

public interface ICategoryService
{
    Task<CategoryDTO> AddCategoryAsync(CategoryDTO categoryDto, int userId, CancellationToken cancellationToken = default);
}
