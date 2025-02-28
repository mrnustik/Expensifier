using System.Net;
using System.Net.Http.Json;
using Expensifier.API.Accounts.CreateAccount;
using Expensifier.API.Accounts.Domain;
using Expensifier.API.Accounts.GetAccountById;
using Expensifier.API.Accounts.GetAccounts;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace Expensifier.IntegrationTests;

public class AccountsIntegrationTests 
{
    private readonly CustomWebApplicationFactory _factory;

    public AccountsIntegrationTests(TestInfrastructure infrastructure)
    {
        _factory = new CustomWebApplicationFactory(infrastructure);
    }

    [Fact]
    public async Task CreateAccount_WithValidValues_ReturnsOK()
    {
        // Arrange 
        var httpClient = _factory.CreateClient();

        // Act
        var response = await httpClient.PostAsync(
            "api/accounts/create",
            JsonContent.Create(new CreateAccountCommand("Account")),
            TestContext.Current.CancellationToken);


        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode
                .Should()
                .Be(HttpStatusCode.Created);
        response.Headers
                .Location
                .Should()
                .NotBeNull();
    }

    [Fact]
    public async Task GetAccountById_WithCreateAccountLocation_ReturnsOK()
    {
        // Arrange 
        var httpClient = _factory.CreateClient();
        var createdResponse = await httpClient.PostAsync(
            "api/accounts/create",
            JsonContent.Create(new CreateAccountCommand("Account")),
            TestContext.Current.CancellationToken);
        createdResponse.EnsureSuccessStatusCode();
        var location = createdResponse
                       .Headers
                       .Location;

        // Act
        var getByIdResponse = await httpClient.GetAsync(location, TestContext.Current.CancellationToken);

        // Assert
        getByIdResponse.EnsureSuccessStatusCode();
        getByIdResponse.StatusCode
                       .Should()
                       .Be(HttpStatusCode.OK);
        var body = await getByIdResponse.Content
                                        .ReadFromJsonAsync<AccountDetail>(TestContext.Current.CancellationToken);
        body.Name
            .Should()
            .Be("Account");
    }

    [Fact]
    public async Task GetAccountById_WithCreateAccountId_ReturnsOK()
    {
        // Arrange 
        var httpClient = _factory.CreateClient();
        var createdResponse = await httpClient.PostAsync(
            "api/accounts/create",
            JsonContent.Create(new CreateAccountCommand("Account")),
            TestContext.Current.CancellationToken);
        createdResponse.EnsureSuccessStatusCode();
        var accountId = await createdResponse.Content
                                             .ReadFromJsonAsync<AccountId>(TestContext.Current.CancellationToken);

        // Act
        var getByIdResponse = await httpClient.GetAsync($"api/accounts/{accountId}", TestContext.Current.CancellationToken);

        // Assert
        getByIdResponse.EnsureSuccessStatusCode();
        getByIdResponse.StatusCode
                       .Should()
                       .Be(HttpStatusCode.OK);
        var body = await getByIdResponse.Content
                                        .ReadFromJsonAsync<AccountDetail>(TestContext.Current.CancellationToken);
        body.Name
            .Should()
            .Be("Account");
        body.Balance
            .Should()
            .Be(0);
    }

    [Fact]
    public async Task GetAllAccounts_WithExistingAccount_ReturnsOK()
    {
        // Arrange 
        var httpClient = _factory.CreateClient();
        var createdResponse = await httpClient.PostAsync(
            "api/accounts/create",
            JsonContent.Create(new CreateAccountCommand("Account")),
            TestContext.Current.CancellationToken);
        createdResponse.EnsureSuccessStatusCode();
        var accountId = await createdResponse.Content
                                             .ReadFromJsonAsync<AccountId>(TestContext.Current.CancellationToken);

        // Act
        var getByIdResponse = await httpClient.GetAsync("api/accounts", TestContext.Current.CancellationToken);

        // Assert
        getByIdResponse.EnsureSuccessStatusCode();
        getByIdResponse.StatusCode
                       .Should()
                       .Be(HttpStatusCode.OK);
        var body = await getByIdResponse.Content
                                        .ReadFromJsonAsync<IReadOnlyCollection<AccountListItem>>(TestContext.Current.CancellationToken);
        body.Should()
            .ContainSingle(a =>
                               a.Name == "Account" &&
                                 a.Id == accountId);
    }

    [Fact]
    public async Task DeleteAccount_WithExistingAccount_ReturnsOK()
    {
        // Arrange
        var httpClient = _factory.CreateClient();
        var createdResponse = await httpClient.PostAsync(
            "api/accounts/create",
            JsonContent.Create(new CreateAccountCommand("Account")),
            TestContext.Current.CancellationToken);
        createdResponse.EnsureSuccessStatusCode();
        var accountId = await createdResponse.Content.ReadFromJsonAsync<AccountId>(TestContext.Current.CancellationToken);
        
        // Act
        var deleteResponse = await httpClient.DeleteAsync($"api/accounts/{accountId}", TestContext.Current.CancellationToken);
        
        // Assert
        deleteResponse.EnsureSuccessStatusCode();
        deleteResponse.StatusCode
                      .Should()
                      .Be(HttpStatusCode.NoContent);
    }
    
    
    [Fact]
    public async Task DeleteAccount_ShouldRemoveItFromAccountList()
    {
        // Arrange
        var httpClient = _factory.CreateClient();
        var createdResponse = await httpClient.PostAsync(
            "api/accounts/create",
            JsonContent.Create(new CreateAccountCommand("Account")),
            TestContext.Current.CancellationToken);
        createdResponse.EnsureSuccessStatusCode();
        var accountId = await createdResponse.Content.ReadFromJsonAsync<AccountId>(TestContext.Current.CancellationToken);
        var deleteResponse = await httpClient.DeleteAsync($"api/accounts/{accountId}", TestContext.Current.CancellationToken);
        deleteResponse.EnsureSuccessStatusCode();

        // Act
        var accounts = await httpClient.GetAsync("api/accounts", TestContext.Current.CancellationToken);
        
        // Assert
        accounts.EnsureSuccessStatusCode();
        accounts.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);
        var body = await accounts.Content.ReadFromJsonAsync<IReadOnlyCollection<AccountListItem>>(TestContext.Current.CancellationToken);
        body.Should()
            .NotContain(a => a.Id == accountId);
    }
}