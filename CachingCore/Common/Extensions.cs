using System.Reflection;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CachingCore.Common;

[AttributeUsage(AttributeTargets.All)]
public class PostgresEnum: Attribute { }

public static class PostgresEnumExtension
{
    public static void MapNpgsqlEnums(this ModelBuilder builder, string schema)
    {
        var postgresEnums = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x.GetCustomAttribute<PostgresEnum>() != null)
            .ToList();
    
        foreach (var postgresEnum in postgresEnums)
            builder.HasPostgresEnum($"{schema}.{postgresEnum.Name.Underscore()}", postgresEnum.GetEnumNames().Select(x => x.Underscore()).ToArray());
    }

    public static void RegisterEnumTypeConversion(this NpgsqlDataSourceBuilder builder, string schema)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x.GetCustomAttribute<PostgresEnum>() != null)
            .ToList();

        foreach (var type in types)
        {
            builder.MapEnum(type, $"{schema}.{type.Name.Underscore()}");
        }
    }

    public static void RedisConnection()
    {
        
    }
}