using Expensifier.API.Common.Queries;
using Expensifier.API.Common.Users;
using Marten;

namespace Expensifier.API.Categories.GetCategories;

public record GetCategories : Query<IReadOnlyCollection<Category>>
{
    public class Handler : QueryHandler<GetCategories, IReadOnlyCollection<Category>>
    {
        private readonly IQuerySession _querySession;
        private readonly IUserProvider _userProvider;

        public Handler(IQuerySession querySession, IUserProvider userProvider)
        {
            _querySession = querySession;
            _userProvider = userProvider;
        }

        protected override async Task<IReadOnlyCollection<Category>> Query(
            GetCategories query,
            CancellationToken cancellationToken)
        {
            var categories = await _querySession.Query<Category>()
                                                .Where(c => c.UserId == _userProvider.CurrentUserId)
                                                .ToListAsync(cancellationToken);
            return categories.ToList();
        }
    }
}