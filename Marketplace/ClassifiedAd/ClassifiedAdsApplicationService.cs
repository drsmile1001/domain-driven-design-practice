using Marketplace.Domain.ClassifiedAd;
using Marketplace.Domain.Shared;
using Marketplace.Framework;
using static Marketplace.ClassifiedAd.Contracts;

namespace Marketplace.ClassifiedAd;
public class ClassifiedAdsApplicationService : IApplicationService
{
    private readonly IClassifiedAdRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrencyLookup _currencyLookup;

    public ClassifiedAdsApplicationService(
        ICurrencyLookup currencyLookup,
        IClassifiedAdRepository repository,
        IUnitOfWork unitOfWork)
    {
        _currencyLookup = currencyLookup;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public Task Handle(object command)
    => command switch
    {
        V1.Create cmd => HandleCreate(cmd),
        V1.SetTitle cmd => HandleUpdate(cmd.Id, c => c.SetTitle(cmd.Title)),
        V1.UpdateText cmd => HandleUpdate(cmd.Id, c => c.UpdateText(new ClassifiedAdText(cmd.Text))),
        V1.UpdatePrice cmd => HandleUpdate(cmd.Id, c =>
                             c.UpdatePrice(Price.FromDecimal(cmd.Price, cmd.Currency, _currencyLookup))),
        V1.RequestToPublish cmd => HandleUpdate(cmd.Id, c => c.RequestToPublish()),
        _ => throw new InvalidOperationException($"Command type {command.GetType().FullName} is unknown"),
    };

    private async Task HandleCreate(V1.Create cmd)
    {
        if (await _repository.Exists(cmd.Id))
        {
            throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");
        }

        var classifiedAd = new Domain.ClassifiedAd.ClassifiedAd(cmd.Id, cmd.OwnerId);
        await _repository.Add(classifiedAd);
        await _unitOfWork.Commit();
    }

    private async Task HandleUpdate(Guid classifiedAdId, Action<Domain.ClassifiedAd.ClassifiedAd> operation)
    {
        var classifiedAd = await _repository.Load(classifiedAdId);

        if (classifiedAd == null)
        {
            throw new InvalidOperationException($"Entity with id {classifiedAdId} cannot be found");
        }

        operation(classifiedAd);
        await _unitOfWork.Commit();
    }
}