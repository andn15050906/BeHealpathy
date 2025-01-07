/*
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Linq.Expressions;
using WisNet.Common.Core.Domain;
using WisNet.Common.Core.Domain.Interfaces;
using WisNet.Common.Core.Helpers;
using WisNet.Common.Core.Interfaces;

namespace Infrastructure.DataAccess.MongoDB;

public class BaseContext
{
    static BaseContext()
    {
        BsonClassMap.RegisterClassMap<Entity>(_ =>
        {
            _.MapIdProperty(_ => _.Id);
        });
    }

    private readonly IMongoDatabase _database;
    public Dictionary<Type, string> Collections { get; set; }

    public BaseContext(IOptions<MongoOptions> options)
    {
        _database = new MongoClient(options.Value.ConnectionString).GetDatabase(options.Value.Database);
    }






    public async Task<T> Find<T>(Guid id) where T : Entity
    {
        return (await GetCollection<T>().FindAsync(FilterById<T>(id))).FirstOrDefault();
    }

    public async Task<List<T>> Find<T>(FilterDefinition<T> predicate) where T : DomainObject
    {
        return await (await GetCollection<T>().FindAsync(predicate)).ToListAsync();
    }

    //public async Task<List<T>> Find<T>(Expression<Func<T, bool>> predicate) where T : DomainObject
    //{
    //    return await (await GetCollection<T>().FindAsync(predicate)).ToListAsync();
    //}

    public async Task<T> FindOne<T>(FilterDefinition<T> predicate) where T : Entity
    {
        return await GetCollection<T>().Find(predicate).Limit(1).SingleOrDefaultAsync();
    }






    public async Task<List<TProjection>> Find<T, TProjection>(
        FilterDefinition<T> predicate, ProjectionDefinition<T, TProjection> projection)
        where T : DomainObject
    {
        return await GetCollection<T>()
            .Find(predicate)
            .Project(projection)
            .ToListAsync();
    }

    //public async Task<List<TProjection>> Find<T, TProjection>(
    //    FilterDefinition<T> predicate, FindOptions<T, TProjection> options)
    //    where T : DomainObject
    //{
    //    return await (await GetCollection<T>().FindAsync(predicate, options)).ToListAsync();
    //}

    public async Task<PagedResult<TProjection>> FindPaged<T, TProjection>(
        FilterDefinition<T> predicate, ProjectionDefinition<T, TProjection> projection,
        Expression<Func<T, object>> orderByExpression, bool isAscending, short pageIndex, byte pageSize)
        where T : Entity
    {
        var collection = GetCollection<T>();

        var countTask = collection.CountDocumentsAsync(predicate);



        var findFluent = GetCollection<T>()
            .Find(predicate)
            .Project(projection);

        findFluent = isAscending
            ? findFluent.SortBy(orderByExpression)
            : findFluent.SortByDescending(orderByExpression);

        var itemsTask = findFluent
            .Skip(pageIndex * pageSize).Limit(pageSize)
            .ToListAsync();

        return new PagedResult<TProjection>(await countTask, pageIndex, pageSize, await itemsTask);
    }






    public async Task Insert<T>(T entity) where T : DomainObject
    {
        TrySetCreationTime(entity);

        await GetCollection<T>().InsertOneAsync(entity);
    }






    public async Task<bool> Update<T>(Guid id, UpdateDefinition<T> update)
        where T : Entity
    {
        TrySetModificationTime(update);

        var result = await GetCollection<T>().UpdateOneAsync(FilterById<T>(id), update);
        return result.IsAcknowledged;
    }

    public async Task<bool> Update<T>(FilterDefinition<T> predicate, UpdateDefinition<T> update)
        where T : DomainObject
    {
        TrySetModificationTime(update);

        var result = await GetCollection<T>().UpdateOneAsync(predicate, update);
        return result.IsAcknowledged;
    }

    //public async Task<bool> Update<T>(Expression<Func<T, bool>> predicate, UpdateDefinition<T> update)
    //    where T : DomainObject
    //{
    //    var result = await GetCollection<T>().UpdateOneAsync(predicate, update);
    //    return result.IsAcknowledged;
    //}

    public async Task<bool> Replace<T>(Guid id, T entity)
        where T : Entity
    {
        TrySetModificationTime(entity);

        var result = await GetCollection<T>().ReplaceOneAsync(FilterById<T>(id), entity);
        return result.IsAcknowledged;
    }






    public async Task<bool> DeleteOne<T>(Guid id) where T : Entity
    {
        var result = await GetCollection<T>().DeleteOneAsync(FilterById<T>(id));
        return result.IsAcknowledged;
    }

    public async Task<bool> DeleteOne<T>(FilterDefinition<T> predicate) where T : DomainObject
    {
        var result = await GetCollection<T>().DeleteOneAsync(predicate);
        return result.IsAcknowledged;
    }

    public async Task<bool> DeleteMany<T>(FilterDefinition<T> predicate) where T : DomainObject
    {
        var result = await GetCollection<T>().DeleteManyAsync(predicate);
        return result.IsAcknowledged;
    }

    //public async Task<bool> DeleteOne<T>(Expression<Func<T, bool>> predicate) where T : DomainObject
    //{
    //    var result = await GetCollection<T>().DeleteOneAsync(predicate);
    //    return result.IsAcknowledged;
    //}






    private IMongoCollection<T> GetCollection<T>() => _database.GetCollection<T>(Collections[typeof(T)]);

    private FilterDefinition<T> FilterById<T>(Guid id) where T : Entity
    {
        return Builders<T>.Filter.Eq(nameof(Entity.Id), id);
        //return Builders<T>.Filter.Eq(_ => _.Id, id);
    }

    private void TrySetCreationTime<T>(T domainObject) where T : DomainObject
    {
        if (domainObject is ICreationTimeAudited cta)
            cta.SetCreationTime();
    }

    private void TrySetModificationTime<T>(T domainObject) where T : DomainObject
    {
        if (domainObject is IModificationTimeAudited mta)
            mta.SetModificationTime();
    }

    private void TrySetModificationTime<T>(UpdateDefinition<T> updateDefinition) where T : DomainObject
    {
        if (typeof(T).IsAssignableFrom(typeof(IModificationTimeAudited)))
            updateDefinition.Set(nameof(IModificationTimeAudited.LastModificationTime), TimeHelper.Now);
    }
}
*/