using System.Globalization;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Patient.BLL.Constans;
using Patient.DAL.Entities;

namespace Patient.BLL.Utilities;

public static class DateFilterExpressionBuilder
{
    public static Expression<Func<PatientProfile, bool>> BuildExpression(string filterParameter)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filterParameter);

        var (operatorType, date) = ParseParameter(filterParameter);
        var expression = CreateExpression(operatorType, date);

        return expression;
    }

    public static Expression<Func<PatientProfile, bool>> BuildExpressionByPeriod(string filterParameter,
        DateTime startPeriod = default, DateTime endPeriod = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filterParameter);

        var (operatorType, date) = ParseParameter(filterParameter);
        var expression = CreateExpressionByPeriod(operatorType, date, startPeriod, endPeriod);

        return expression;
    }

    public static (string operatorType, DateTime date) ParseParameter(string filterParameter)
    {
        var regex = new Regex(DateParsingConstants.DateRegexPattern);
        var match = regex.Match(filterParameter);

        if (!match.Success)
        {
            throw new ArgumentException("Invalid parameter format.");
        }

        var operatorPart = match.Groups[1].Value;
        var datePart = match.Groups[2].Value;
        var timePart = match.Groups[3].Value;

        var dateTimeString = datePart + (string.IsNullOrEmpty(timePart)
            ? String.Empty
            : DateParsingConstants.TimeDelimiter + timePart.Replace(DateParsingConstants.TimeFormatSpecifier,
                String.Empty, StringComparison.Ordinal));

        if (!DateTime.TryParseExact(dateTimeString, DateParsingConstants.DateFormats,
                                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
        {
            throw new ArgumentException("Invalid date format.");
        }

        return (operatorType: operatorPart, date: date);
    }

    private static Expression<Func<PatientProfile, bool>> CreateExpression(string operatorType, DateTime date)
    {
        var nextDay = date.AddDays(1);

        return operatorType switch
        {
            "eq" => patient => patient.RecordDate >= date && patient.RecordDate < nextDay,
            "ne" => patient => patient.RecordDate < date || patient.RecordDate >= nextDay,
            "ge" => patient => patient.RecordDate >= date,
            "le" => patient => patient.RecordDate <= date,
            "sa" => patient => patient.RecordDate >= nextDay,
            "eb" => patient => patient.RecordDate < date,
            "ap" => patient => patient.RecordDate.DayOfYear == date.DayOfYear,
            _ => throw new NotSupportedException($"Unsupported operator '{operatorType}'.")
        };
    }

    private static Expression<Func<PatientProfile, bool>> CreateExpressionByPeriod(string operatorType, DateTime date,
        DateTime startPeriod = default, DateTime endPeriod = default)
    {
        return operatorType switch
        {
            "lt" => patient => (patient.RecordDate >= startPeriod && patient.RecordDate < date)
                               || (patient.RecordDate.DayOfYear == date.DayOfYear && patient.RecordDate.TimeOfDay == TimeSpan.Zero),
            "gt" => patient => (patient.RecordDate > date && patient.RecordDate <= endPeriod)
                               || (patient.RecordDate.DayOfYear == date.DayOfYear && patient.RecordDate.TimeOfDay == TimeSpan.Zero),
            _ => throw new NotSupportedException($"Unsupported operator '{operatorType}'.")
        };
    }
}
