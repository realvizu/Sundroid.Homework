using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sundroid.Homework.Persistence;

/// <summary>
/// Helper methods for DbContext model builder operations.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Maps all properties of an owned type into DB columns with the same name as the property.
    /// </summary>
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