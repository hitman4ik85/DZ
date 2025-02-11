using Microsoft.EntityFrameworkCore;
using PeopleBudgetTracker.Core.DTOs;
using PeopleBudgetTracker.Core.Interfaces;
using PeopleBudgetTracker.Entities.Models;
using PeopleBudgetTracker.Storage;

namespace PeopleBudgetTracker.Core.Services;

public class CategoryService : ICategoryService
{
    private readonly PeopleBudgetTrackerContext _context;

    public CategoryService(PeopleBudgetTrackerContext context)
    {
        _context = context;
    }

    public async Task<CategoryDTO> AddCategoryAsync(CategoryDTO categoryDto, int userId, CancellationToken cancellationToken = default)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);
        if (account == null)
            throw new ArgumentException("User account not found", nameof(userId));

        var category = new Category
        {
            Name = categoryDto.Name,
            AccountId = account.Id
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync(cancellationToken);

        categoryDto.Id = category.Id;
        categoryDto.AccountId = account.Id;
        return categoryDto;
    }
}
