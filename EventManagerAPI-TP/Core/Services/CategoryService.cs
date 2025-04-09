using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Récupérer toutes les catégories
    public async Task<IEnumerable<CategoryReadDTO>> GetCategoriesAsync()
    {
        return await _context.Categories
            .Select(c => new CategoryReadDTO
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();
    }

    // Créer une nouvelle catégorie
    public async Task<CategoryReadDTO> CreateCategoryAsync(CategoryCreateDTO categoryCreateDTO)
    {
        var category = new Category
        {
            Name = categoryCreateDTO.Name
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return new CategoryReadDTO
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    // Mettre à jour une catégorie existante
    public async Task<CategoryReadDTO> UpdateCategoryAsync(int id, CategoryUpdateDTO categoryUpdateDTO)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            return null;
        }

        category.Name = categoryUpdateDTO.Name;

        _context.Categories.Update(category);
        await _context.SaveChangesAsync();

        return new CategoryReadDTO
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    // Supprimer une catégorie
    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            return false;
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return true;
    }

    // Récupérer une catégorie par son ID
    public async Task<CategoryReadDTO> GetCategoryByIdAsync(int id)
    {
        var category = await _context.Categories
            .Select(c => new CategoryReadDTO
            {
                Id = c.Id,
                Name = c.Name
            })
            .FirstOrDefaultAsync(c => c.Id == id);

        return category;
    }
}
