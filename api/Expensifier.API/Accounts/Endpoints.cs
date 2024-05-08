using Expensifier.API.Accounts.CreateAccount;
using Expensifier.API.Accounts.Domain;
using Expensifier.API.Accounts.GetAccountsById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Expensifier.API.Accounts;

public static class Endpoints
{
    public static void AddAccountEndpoints(this WebApplication app)
    {
        app.MapPost("accounts/create",
                    async ([FromBody] CreateAccountCommand command,
                           IMediator mediator,
                           CancellationToken cancellationToken) =>
                    {
                        var accountId = await mediator.Send(command, cancellationToken);
                        return TypedResults.Created($"accounts/{accountId}");
                    });

        app.MapGet("accounts/{id}",
                   async ([FromRoute] AccountId id,
                          IMediator mediator,
                          CancellationToken cancellationToken) =>
                   {
                       var account = await mediator.Send(new GetAccountByIdQuery(id), cancellationToken);
                       return TypedResults.Ok(account);
                   });
    }
}