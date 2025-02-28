using Expensifier.API.Common.Users;

namespace Expensifier.API.Categories;

public record Category
{
    public CategoryId? Id { get; init; }
    public string Name { get; init; }
    public CategoryType Type { get; init; }
    public UserId UserId { get; init; }
};