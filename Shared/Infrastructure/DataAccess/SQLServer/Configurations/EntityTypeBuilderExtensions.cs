using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace Infrastructure.DataAccess.SQLServer.Configurations;

public static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder<T> SetColumnsTypes<T>(this EntityTypeBuilder<T> entityTypeBuilder, Dictionary<Expression<Func<T, object?>>, string> columns)
        where T : DomainObject
    {
        foreach (var column in columns)
            entityTypeBuilder.Property(column.Key).HasColumnType(column.Value);
        return entityTypeBuilder;
    }

    public static EntityTypeBuilder<T> SetEnumParsing<T, TEnum>(this EntityTypeBuilder<T> entityTypeBuilder, Expression<Func<T, TEnum>> property)
        where T : DomainObject
        where TEnum : Enum
    {
        entityTypeBuilder.Property(property).HasConversion(_ => _.ToString(), _ => (TEnum)Enum.Parse(typeof(TEnum), _));
        return entityTypeBuilder;
    }

    public static EntityTypeBuilder<T> SetUnique<T>(this EntityTypeBuilder<T> entityTypeBuilder, params Expression<Func<T, object?>>[] properties)
        where T : DomainObject
    {
        foreach (var property in properties)
            entityTypeBuilder.HasIndex(property).IsUnique();
        return entityTypeBuilder;
    }

    public static EntityTypeBuilder<T> SetDefaultSQL<T>(this EntityTypeBuilder<T> entityTypeBuilder, Expression<Func<T, object?>> property, string sql)
        where T : DomainObject
    {
        entityTypeBuilder.Property(property).HasDefaultValueSql(sql);
        return entityTypeBuilder;
    }
}