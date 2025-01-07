using System.Reflection;
using System.Text;

namespace Core.Helpers;

public static class QueryBuilder
{
    public static string BuildQuery(object obj)
    {
        return string.Join('&', GetNestedPropertyValues(obj));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="urlPath">Example: "api/users/multiple?"</param>
    /// <param name="elementFormat">Example: "ids={0}&"</param>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static string BuildWithArray(string urlPath, string elementFormat, IEnumerable<string> elements)
    {
        StringBuilder query = new(urlPath);
        foreach (var id in elements)
            query.AppendFormat(elementFormat, id);
        query.Length--;

        return query.ToString();
    }






    /// <summary>
    /// Flatten the nested properties into a single sequence
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private static IEnumerable<string> GetNestedPropertyValues(object obj)
    {
        return obj.GetType().GetProperties()
            .SelectMany(nestedProperty => GetPropertyValues(obj, nestedProperty));
    }

    private static IEnumerable<string> GetPropertyValues(object parent, PropertyInfo property)
    {
        string propertyName = property.Name;
        object? propertyValue = property.GetValue(parent)!;

        if (propertyValue is null)
            return Array.Empty<string>();

        if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
        {
            if (property.PropertyType.IsArray)
                return GetArrayValues(propertyName, (Array)propertyValue);
            return GetNestedPropertyValues(propertyValue)
                .Select(nestedValue => $"{Uri.EscapeDataString(propertyName)}.{nestedValue}");
        }
        else
        {
            string encodedKey = Uri.EscapeDataString(propertyName);

            string encodedValue = property.PropertyType.IsEnum
                ? Uri.EscapeDataString(((int)propertyValue).ToString())
                : Uri.EscapeDataString(propertyValue.ToString()!);

            return new[] { $"{encodedKey}={encodedValue}" };
        }
    }

    private static IEnumerable<string> GetArrayValues(string propertyName, Array array)
    {
        List<string> values = new();

        for (byte i = 0; i < array.Length; i++)
        {
            string encodedKey = Uri.EscapeDataString(propertyName);
            string encodedValue = Uri.EscapeDataString(array.GetValue(i)?.ToString()!);
            values.Add($"{encodedKey}={encodedValue}");
        }

        return values;
    }
}