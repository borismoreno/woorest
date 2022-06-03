using MongoDB.Driver;
using woorest.Entities;

namespace woorest.Repositories;

public class WoocommerceRepository : IWoocommerceRepository
{
    private const string collectionName = "weebhooks";
    private readonly IMongoCollection<Weebhook> dbCollecion;
    private readonly FilterDefinitionBuilder<Weebhook> filterDefinitionBuilder = Builders<Weebhook>.Filter;

    public WoocommerceRepository(IMongoDatabase database)
    {
        dbCollecion = database.GetCollection<Weebhook>(collectionName);
    }
    public async Task CreateAsync(Weebhook weebhook)
    {
        if (weebhook == null)
            throw new ArgumentException(nameof(Weebhook));
        await dbCollecion.InsertOneAsync(weebhook);
    }
}