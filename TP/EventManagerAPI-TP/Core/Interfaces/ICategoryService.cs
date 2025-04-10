public interface ICategoryService
{
    Task<IEnumerable<CategoryReadDTO>> GetCategoriesAsync();

    Task<CategoryReadDTO> GetCategoryByIdAsync(int id);

    Task<CategoryReadDTO> CreateCategoryAsync(CategoryCreateDTO categoryCreateDTO);

    Task<CategoryReadDTO> UpdateCategoryAsync(int id, CategoryUpdateDTO categoryUpdateDTO);

    Task<bool> DeleteCategoryAsync(int id);
}
