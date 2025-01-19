using Core.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.DataAccess.SQLServer.Helpers;

public static class DbSetExtensions
{
    public static async Task<bool> AnyExt<T>(this DbSet<T> DbSet, Expression<Func<T, bool>> predicate)
        where T : DomainObject
        => await DbSet.AsNoTracking().AnyAsync(predicate);

    public static async Task<T?> FindExt<T>(this DbSet<T> DbSet, params object[] keys)
        where T : DomainObject
        => await DbSet.FindAsync(keys);

    public static async Task InsertExt<T>(this DbSet<T> DbSet, T entity)
        where T : DomainObject
        => await DbSet.AddAsync(entity);

    public static void SoftDeleteExt<T>(this DbSet<T> DbSet, T entity)
        where T : Entity
    {
        entity.IsDeleted = true;
    }

    /*public static async Task SoftDeleteExt<T>(this DbSet<T> DbSet, Guid id, CancellationToken cancellationToken = default)
        where T : Entity
    {
        await DbSet
            .Where(_ => _.Id == id)
            .ExecuteUpdateAsync(_ => _.SetProperty(_ => _.IsDeleted, true), cancellationToken);
    }*/

    public static void DeleteExt<T>(this DbSet<T> DbSet, T entity)
        where T : DomainObject
        => DbSet.Remove(entity);

    public static async Task DeleteExt<T>(this DbSet<T> DbSet, Guid id, CancellationToken cancellationToken = default)
        where T : Entity
        => await DbSet.Where(_ => _.Id == id).ExecuteDeleteAsync(cancellationToken);

    public static async Task DeleteExt<T>(this DbSet<T> DbSet, IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        where T : Entity
        => await DbSet.Where(_ => ids.Contains(_.Id)).ExecuteDeleteAsync(cancellationToken);
}