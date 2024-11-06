namespace Patient.BLL.Constans;

public static class DateParsingConstants
{
    public const string DateRegexPattern = @"(eq|ne|gt|lt|ge|le|sa|eb|ap)(\d{4}-\d{2}-\d{2})(T\d{2}:\d{2})?";

    public static readonly string[] DateFormats = [DateFormatWithoutTime, DateFormatWithTime];

    public const string DateFormatWithoutTime = "yyyy-MM-dd";

    public const string DateFormatWithTime = "yyyy-MM-dd HH:mm";

    public const string TimeFormatSpecifier = "T";

    public const string TimeDelimiter = " ";

    public const string RegexOperatorGroupName = "operator";

    public const string RegexDateGroupName = "date";

    public const string RegexTimeGroupName = "time";
}
