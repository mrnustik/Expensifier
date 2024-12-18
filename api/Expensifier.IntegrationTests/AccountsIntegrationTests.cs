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

public class AccountsIntegrationTests : IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory = new();

    public Task InitializeAsync()
    {
        return _factory.StartAsync();
    }

    public Task DisposeAsync()
    {
        return _factory.DisposeAsync().AsTask();
    }

    [Fact]
    public async Task CreateAccount_WithValidValues_ReturnsOK()
    {
        // Arrange 
        var httpClient = _factory.CreateClient();

        // Act
        var response = await httpClient.PostAsync(
            "api/accounts/create",
            JsonContent.Create(new CreateAccountCommand("Account")));


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
            JsonContent.Create(new CreateAccountCommand("Account")));
        createdResponse.EnsureSuccessStatusCode();
        var location = createdResponse
                       .Headers
                       .Location;

        // Act
        var getByIdResponse = await httpClient.GetAsync(location);

        // Assert
        getByIdResponse.EnsureSuccessStatusCode();
        getByIdResponse.StatusCode
                       .Should()
                       .Be(HttpStatusCode.OK);
        var body = await getByIdResponse.Content
                                        .ReadFromJsonAsync<AccountDetail>();
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
            JsonContent.Create(new CreateAccountCommand("Account")));
        createdResponse.EnsureSuccessStatusCode();
        var accountId = await createdResponse.Content
                                             .ReadFromJsonAsync<AccountId>();

        // Act
        var getByIdResponse = await httpClient.GetAsync($"api/accounts/{accountId}");

        // Assert
        getByIdResponse.EnsureSuccessStatusCode();
        getByIdResponse.StatusCode
                       .Should()
                       .Be(HttpStatusCode.OK);
        var body = await getByIdResponse.Content
                                        .ReadFromJsonAsync<AccountDetail>();
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
            JsonContent.Create(new CreateAccountCommand("Account")));
        createdResponse.EnsureSuccessStatusCode();
        var accountId = await createdResponse.Content
                                             .ReadFromJsonAsync<AccountId>();

        // Act
        var getByIdResponse = await httpClient.GetAsync("api/accounts");

        // Assert
        getByIdResponse.EnsureSuccessStatusCode();
        getByIdResponse.StatusCode
                       .Should()
                       .Be(HttpStatusCode.OK);
        var body = await getByIdResponse.Content
                                        .ReadFromJsonAsync<IReadOnlyCollection<AccountListItem>>();
        body.Should()
            .ContainSingle(a =>
                               a.Name == "Account" &&
                               a.Id == accountId.Value);
    }
}