using Core.Domain;
using Infrastructure.DataAccess.SQLServer.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.DataAccess.SQLServer;

public class BaseContext : DbContext
{
    public BaseContext(DbContextOptions options) : base(options) { }

    /// <summary>
    /// Concrete repositories provide mapping configuration & included properties
    /// Might use this method or call PagingQuery's constructor within Concrete repositories
    /// </summary>
    public PagingQuery<T, TDto> GetPagingQuery<T, TDto>(
        Expression<Func<T, TDto>> mappingConfig,
        Expression<Func<T, bool>>? whereExpression,
        short pageIndex, int pageSize,
        bool asSplitQuery = false,
        params Expression<Func<T, object?>>[] includeExpressions)
        where T : DomainObject
    {
        return new(this, mappingConfig, whereExpression, pageIndex, pageSize, asSplitQuery, includeExpressions);
    }
}