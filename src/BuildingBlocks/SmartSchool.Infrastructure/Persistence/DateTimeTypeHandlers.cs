using System.Data;
using Dapper;

namespace SmartSchool.Infrastructure.Persistence;

public static class DateTimeTypeHandlers
{
    public static void Register()
    {
        SqlMapper.RemoveTypeMap(typeof(DateTimeOffset));
        SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        SqlMapper.AddTypeHandler(new DateOnlyHandler());
        SqlMapper.AddTypeHandler(new TimeOnlyHandler());
    }
    private sealed class DateTimeOffsetHandler : SqlMapper.TypeHandler<DateTimeOffset>
    {
        public override DateTimeOffset Parse(object value) => value switch
        {
            DateTimeOffset instant => instant,
            DateTime instant => new DateTimeOffset(DateTime.SpecifyKind(instant, DateTimeKind.Utc)),
            _ => DateTimeOffset.Parse(Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture)!, System.Globalization.CultureInfo.InvariantCulture)
        };
        public override void SetValue(IDbDataParameter parameter, DateTimeOffset value)
        {
            parameter.DbType = DbType.DateTimeOffset;
            parameter.Value = value.ToUniversalTime();
        }
    }
    private sealed class DateOnlyHandler : SqlMapper.TypeHandler<DateOnly>
    {
        public override DateOnly Parse(object value) => value switch
        {
            DateOnly date => date,
            DateTime date => DateOnly.FromDateTime(date),
            _ => DateOnly.Parse(Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture)!, System.Globalization.CultureInfo.InvariantCulture)
        };
        public override void SetValue(IDbDataParameter parameter, DateOnly value)
        {
            parameter.DbType = DbType.Date;
            parameter.Value = value.ToDateTime(TimeOnly.MinValue);
        }
    }
    private sealed class TimeOnlyHandler : SqlMapper.TypeHandler<TimeOnly>
    {
        public override TimeOnly Parse(object value) => value switch
        {
            TimeOnly time => time,
            TimeSpan time => TimeOnly.FromTimeSpan(time),
            DateTime time => TimeOnly.FromDateTime(time),
            _ => TimeOnly.Parse(Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture)!, System.Globalization.CultureInfo.InvariantCulture)
        };
        public override void SetValue(IDbDataParameter parameter, TimeOnly value)
        {
            parameter.DbType = DbType.Time;
            parameter.Value = value.ToTimeSpan();
        }
    }
}
