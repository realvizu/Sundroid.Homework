using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sundroid.Homework.Persistence;

public static class ModelBuilderExtensions
{
    public static void MapOwnedWithoutPrefix<TEntity, TOwned>(
        this OwnedNavigationBuilder<TEntity, TOwned> builder)
        where TEntity : class
        where TOwned : class
    {
        var props = typeof(TOwned).GetProperties();
        foreach (var prop in props)
        {
            builder.Property(prop.PropertyType, prop.Name).HasColumnName(prop.Name);
        }
    }
}