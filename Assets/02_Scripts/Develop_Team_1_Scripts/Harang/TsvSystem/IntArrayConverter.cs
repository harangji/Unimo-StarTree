using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Linq;

public class IntArrayConverter : ITypeConverter
{
    public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        if (value is int[] array)
            return string.Join(",", array);
        return string.Empty;
    }

    public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        return text.Split(',').Select(int.Parse).ToArray();
    }
}

public class FloatArrayConverter : ITypeConverter
{
    public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        if (value is int[] array)
            return string.Join(",", array);
        return string.Empty;
    }

    public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
            return Array.Empty<float>();

        return text.Split(',')
            .Select(s => float.Parse(s.Trim()))
            .ToArray();
    }
}