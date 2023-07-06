using System;
using System.Numerics;
using Npgsql.Internal.Converters;
using Npgsql.PostgresTypes;
using NpgsqlTypes;

namespace Npgsql.Internal.Resolvers;

sealed class RangeTypeInfoResolver : IPgTypeInfoResolver
{
    TypeInfoMappingCollection Mappings { get; }

    public RangeTypeInfoResolver()
    {
        Mappings = new TypeInfoMappingCollection();
        AddInfos(Mappings);
        // TODO: Opt-in only
        AddArrayInfos(Mappings);
    }

    public PgTypeInfo? GetTypeInfo(Type? type, DataTypeName? dataTypeName, PgSerializerOptions options)
        => Mappings.Find(type, dataTypeName, options);

    static void AddInfos(TypeInfoMappingCollection mappings)
    {
        mappings.AddStructType<NpgsqlRange<int>>(DataTypeNames.Int4Range,
            static (options, mapping, _) => mapping.CreateInfo(options, new RangeConverter<int>(new Int4Converter<int>())),
            isDefault: true);
        mappings.AddStructType<NpgsqlRange<long>>(DataTypeNames.Int8Range,
            static (options, mapping, _) => mapping.CreateInfo(options, new RangeConverter<long>(new Int8Converter<long>())),
            isDefault: true);
        mappings.AddStructType<NpgsqlRange<decimal>>(DataTypeNames.NumRange,
            static (options, mapping, _) =>
                mapping.CreateInfo(options, new RangeConverter<decimal>(new DecimalNumericConverter<decimal>())), isDefault: true);
        mappings.AddStructType<NpgsqlRange<BigInteger>>(DataTypeNames.NumRange,
            static (options, mapping, _) => mapping.CreateInfo(options, new RangeConverter<BigInteger>(new BigIntegerNumericConverter())),
            isDefault: true);

        // daterange
        mappings.AddStructType<NpgsqlRange<DateTime>>(DataTypeNames.DateRange,
            static (options, mapping, _) => mapping.CreateInfo(options,
                new RangeConverter<DateTime>(new DateTimeDateConverter(options.EnableDateTimeInfinityConversions))), isDefault: true);
        mappings.AddStructType<NpgsqlRange<int>>(DataTypeNames.DateRange,
            static (options, mapping, _) => mapping.CreateInfo(options, new RangeConverter<int>(new Int4Converter<int>())),
            isDefault: true);
#if NET6_0_OR_GREATER
        mappings.AddStructType<NpgsqlRange<DateOnly>>(DataTypeNames.DateRange,
            static (options, mapping, _) =>
                mapping.CreateInfo(options, new DateOnlyDateConverter(options.EnableDateTimeInfinityConversions)), isDefault: true);
#endif

        // TODO: timestamp/timestamptz
    }

    static void AddArrayInfos(TypeInfoMappingCollection mappings)
    {
    }
}
