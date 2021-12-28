using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using static Marketplace.ClassifiedAd.ReadModels;
using static Marketplace.Domain.ClassifiedAd.ClassifiedAd;

namespace Marketplace.ClassifiedAd;

public static class Queries
{
    public static Task<List<ClassifiedAdListItem>> Query(
        this IAsyncDocumentSession session,
        QueryModels.GetPublishedClassifiedAds query) =>
            session.Query<Domain.ClassifiedAd.ClassifiedAd>()
                .Where(x => x.State == ClassifiedAdState.Active)
                .Select(x => new ClassifiedAdListItem
                {
                    ClassifiedAdId = x.Id.Value,
                    Price = x.Price!.Amount,
                    Title = x.Title!.Value,
                    CurrencyCode = x.Price.CurrencyCode,
                }).PagedListAsync(query.Page, query.PageSize);

    public static Task<List<T>> PagedListAsync<T>(this IRavenQueryable<T> query, int page, int pageSize)
    => query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
}