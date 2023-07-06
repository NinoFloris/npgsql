using System;
using System.Collections;
using System.Collections.Generic;
using Npgsql.PostgresTypes;
using Npgsql.Properties;
using NpgsqlTypes;

namespace Npgsql.Internal.Resolvers;

sealed class UnsupportedTypeInfoResolver<TBuilder> : IPgTypeInfoResolver
{
    public PgTypeInfo? GetTypeInfo(Type? type, DataTypeName? dataTypeName, PgSerializerOptions options)
    {
        if (typeof(IEnumerable<>).IsAssignableFrom(type) && !typeof(IList).IsAssignableFrom(type) && type != typeof(string))
            throw new NotSupportedException("Writing is not supported for IEnumerable parameters, use an array or List instead.");

        if (type != typeof(object) && dataTypeName == DataTypeNames.Record)
        {
            throw new NotSupportedException(
                string.Format(NpgsqlStrings.RecordsNotEnabled, nameof(NpgsqlSlimDataSourceBuilder.EnableRecords), typeof(TBuilder).Name));
        }

        if (type is { IsGenericType: true } && type.GetGenericTypeDefinition() == typeof(NpgsqlRange<>)
            || dataTypeName.HasValue && options.TypeCatalog.GetPostgresTypeByName(dataTypeName.Value) is PostgresRangeType)
        {
            throw new NotSupportedException(
                string.Format(NpgsqlStrings.RangesNotEnabled, nameof(NpgsqlSlimDataSourceBuilder.EnableRanges), typeof(TBuilder).Name));
        }

        return null;
    }
}
