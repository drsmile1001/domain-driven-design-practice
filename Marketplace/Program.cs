using EventStore.Client;
using Marketplace.ClassifiedAd;
using Marketplace.Domain.ClassifiedAd;
using Marketplace.Domain.Shared;
using Marketplace.Domain.UserProfile;
using Marketplace.Framework;
using Marketplace.Infrastructure;
using Marketplace.UserProfile;
using Raven.Client.Documents;

var builder = WebApplication.CreateBuilder(args);

var store = new DocumentStore
{
    Urls = new[] { "http://www.drsmile.work:8080" },
    Database = "Marketplace",
    Conventions =
    {
        FindIdentityProperty = x => x.Name == "DbId",
    },
};

store.Initialize();

var purgomalumClient = new PurgomalumClient(new HttpClient());

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<EventStoreClient>(c =>
{
    var settings = EventStoreClientSettings.Create("esdb://admin:changeit@114.32.171.117:2113?tls=false");
    var client = new EventStoreClient(settings);
    return client;
});
builder.Services.AddSingleton<IAggregateStore, EsAggregateStore>();
builder.Services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>();
builder.Services.AddScoped(c => store.OpenAsyncSession());
builder.Services.AddScoped<IUnitOfWork, RavenDbUnitOfWork>();
builder.Services.AddScoped<IClassifiedAdRepository, ClassifiedAdRepository>();
builder.Services.AddScoped<IUserProfileRepositoy, UserProfileRepository>();
builder.Services.AddScoped<ClassifiedAdsApplicationService>();
builder.Services.AddScoped(c => new UserProfileApplicationService(
    text => purgomalumClient.CheckForProfanity(text).GetAwaiter().GetResult(),
    c.GetRequiredService<IAggregateStore>()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
