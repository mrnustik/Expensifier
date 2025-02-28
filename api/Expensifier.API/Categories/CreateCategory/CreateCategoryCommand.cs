using Expensifier.API.Common.Commands;
using Expensifier.API.Common.Users;
using Marten;

namespace Expensifier.API.Categories.CreateCategory;

public record CreateCategoryCommand(
    string Name,
    CategoryType Type)
    : Command<CategoryId>
{
    public class Handler : CommandHandler<CreateCategoryCommand, CategoryId>
    {
        private readonly IDocumentSession _session;
        private readonly IUserProvider _userProvider;

        public Handler(IDocumentSession session, 
                       IUserProvider userProvider)
        {
            _session = session;
            _userProvider = userProvider;
        }

        protected override async Task<CategoryId> Execute(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = new Category
            {
                Id = CategoryId.New(),
                Name = command.Name,
                Type = command.Type,
                UserId = _userProvider.CurrentUserId
            };
            _session.Store(category);
            await _session.SaveChangesAsync(cancellationToken);
            return category.Id.Value;
        }
    }
}