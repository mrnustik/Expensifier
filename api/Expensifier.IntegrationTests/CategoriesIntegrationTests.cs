using System.Net;
using System.Net.Http.Json;
using Expensifier.API.Categories;
using Expensifier.API.Categories.CreateCategory;
using FluentAssertions;

namespace Expensifier.IntegrationTests;

public class CategoriesIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public CategoriesIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Category_Can_Be_Created()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsync("/api/categories",
                                              JsonContent.Create(new CreateCategoryCommand("Test Category")));

        // Assert
        response
            .Should()
            .HaveStatusCode(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Category_Can_Be_Get_After_Creation()
    {
        // Arrange
        var client = _factory.CreateClient();
        var categoryName = $"Test Category {Guid.NewGuid()}";
        var createResponse = await client.PostAsync("/api/categories",
                                                    JsonContent.Create(
                                                        new CreateCategoryCommand(categoryName)));
        createResponse.EnsureSuccessStatusCode();
        var categoryId = await createResponse.Content.ReadFromJsonAsync<CategoryId>();

        // Act
        var response = await client.GetAsync("/api/categories");

        // Assert
        response.Should()
                .Be200Ok()
                .And
                .Satisfy<IEnumerable<Category>>(c => c
                                                     .Should()
                                                     .ContainEquivalentOf(new
                                                     {
                                                         Id = categoryId,
                                                         Name = categoryName
                                                     }));
    }
}