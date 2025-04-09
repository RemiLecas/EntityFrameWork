public class CategoryReadDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class CategoryCreateDTO
{
    public string Name { get; set; } = string.Empty;
}

public class CategoryUpdateDTO
{
    public string Name { get; set; } = string.Empty;
}
