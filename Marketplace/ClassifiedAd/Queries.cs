using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Queries;
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

    public static Task<List<ClassifiedAdListItem>> Query(
        this IAsyncDocumentSession session,
        QueryModels.GetOwnersClassifiedAd query) =>
        session.Query<Domain.ClassifiedAd.ClassifiedAd>()
            .Where(x => x.OwnerId.Value == query.OwnerId)
            .Select(x => new ClassifiedAdListItem
            {
                ClassifiedAdId = x.Id.Value,
                Price = x.Price!.Amount,
                Title = x.Title!.Value,
                CurrencyCode = x.Price.CurrencyCode,
            }).PagedListAsync(query.Page, query.PageSize);

    public static Task<ClassifiedAdDetails> Query(
        this IAsyncDocumentSession session,
        QueryModels.GetPublicClassifiedAd query) =>
        session.Query<Domain.ClassifiedAd.ClassifiedAd>()
        .Where(x => x.Id.Value == query.ClassifiedAdId)
        .Select(x => new ClassifiedAdDetails
        {
            ClassifiedAdId = x.Id.Value,
            Title = x.Title.Value,
            Description = x.Text.Value,
            Price = x.Price.Amount,
            CurrencyCode = x.Price.CurrencyCode,
            SellersDisplayName = RavenQuery
                .Load<Domain.UserProfile.UserProfile>("UserProfile/" + x.OwnerId.Value).DisplayName.Value,
        }).SingleAsync();

    public static Task<List<T>> PagedListAsync<T>(this IRavenQueryable<T> query, int page, int pageSize)
    => query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
}