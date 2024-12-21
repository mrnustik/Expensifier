using Expensifier.API.Accounts.CreateAccount;
using Expensifier.API.Accounts.DeleteAccount;
using Expensifier.API.Accounts.Domain;
using Expensifier.API.Accounts.GetAccountById;
using Expensifier.API.Accounts.GetAccounts;
using Expensifier.API.Common.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Expensifier.API.Accounts;

public static class Endpoints
{
    public static void AddAccountEndpoints(this WebApplication app)
    {
        app.MapPost("api/accounts/create",
                    async ([FromBody] CreateAccountCommand command,
                           IMediator mediator,
                           CancellationToken cancellationToken) =>
                    {
                        var accountId = await mediator.Send(command, cancellationToken);
                        return TypedResults.Created($"api/accounts/{accountId}", accountId);
                    });

        app.MapGet("api/accounts/{id}",
                   async ([FromRoute] AccountId id,
                          IMediator mediator,
                          CancellationToken cancellationToken) =>
                   {
                       var account = await mediator.Send(new GetAccountByIdQuery(id), cancellationToken);
                       return TypedResults.Ok(account);
                   });

        app.MapGet("api/accounts",
                   async (IMediator mediator,
                          IUserProvider userProvider,
                          CancellationToken cancellationToken) =>
                   {
                       var accounts =
                           await mediator.Send(new GetAllAccountsQuery(userProvider.CurrentUserId), cancellationToken);
                       return TypedResults.Ok(accounts);
                   });

        app.MapDelete("api/accounts/{id}",
                      async ([FromRoute] AccountId id,
                             IMediator mediator,
                             CancellationToken cancellationToken) =>
                      {
                            await mediator.Send(new DeleteAccountCommand(id), cancellationToken);
                            return TypedResults.NoContent();
                      });
    }
}