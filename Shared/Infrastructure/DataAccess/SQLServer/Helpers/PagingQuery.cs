using Core.Domain;
using Core.Responses;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.DataAccess.SQLServer.Helpers;

public class PagingQuery<T, TDto> where T : DomainObject
{
    private readonly DbContext _context;
    private readonly IQueryable<T> _unorderedQuery;
    private IQueryable<T> _orderedQuery;

    private readonly short _pageIndex;
    private readonly byte _pageSize;
    private readonly Expression<Func<T, TDto>> _mappingConfig;

    /// <summary>
    /// dtoType is for specifying the implied type.
    /// </summary>
    public PagingQuery(
        DbContext context, Expression<Func<T, TDto>> mappingConfig,
        Expression<Func<T, bool>>? whereExpression,
        short pageIndex, byte pageSize,
        bool asSplitQuery = false, params Expression<Func<T, object?>>[] includeExpressions)
    {
        _context = context;
        _unorderedQuery = _context.Set<T>();

        if (includeExpressions is not null)
        {
            if (asSplitQuery)
                _unorderedQuery = _unorderedQuery.AsSplitQuery();
            byte i;
            for (i = 0; i < includeExpressions.Length; i++)
                _unorderedQuery = _unorderedQuery.Include(includeExpressions[i]);
        }

        if (whereExpression is not null)
            _unorderedQuery = _unorderedQuery.Where(whereExpression);

        _orderedQuery = _unorderedQuery;

        _pageIndex = pageIndex;
        _pageSize = pageSize;
        _mappingConfig = mappingConfig;
    }

    public async Task<PagedResult<TDto>> ExecuteWithOrderBy<TOrderBy>(
        Expression<Func<T, TOrderBy>> orderByExpression, bool ascending = true, bool isAnsiWarningTransaction = false, bool asNoTracking = false)
    {
        List<TDto> items = null;
        int total = 0;

        if (isAnsiWarningTransaction)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                await _context.Database.ExecuteSqlRawAsync("SET ANSI_WARNINGS OFF");
                total = await GetTotal();
                items = await GetItems(orderByExpression, ascending, asNoTracking);
                //transaction.Commit();
            });
            return new PagedResult<TDto>(total, _pageIndex, _pageSize, items);
        }

        total = await GetTotal();
        items = await GetItems(orderByExpression, ascending, asNoTracking);
        return new PagedResult<TDto>(total, _pageIndex, _pageSize, items);
    }






    private async Task<int> GetTotal()
    {
        return await _unorderedQuery.CountAsync();
    }

    private async Task<List<TDto>> GetItems<TOrderBy>(Expression<Func<T, TOrderBy>> orderByExpression, bool ascending, bool asNoTracking)
    {
        _orderedQuery = ascending ? _unorderedQuery.OrderBy(orderByExpression) : _unorderedQuery.OrderByDescending(orderByExpression);
        if (asNoTracking)
            _orderedQuery = _orderedQuery.AsNoTracking();
        return await _orderedQuery
            .Skip(_pageIndex * _pageSize).Take(_pageSize)
            .Select(_mappingConfig).ToListAsync();
    }
}