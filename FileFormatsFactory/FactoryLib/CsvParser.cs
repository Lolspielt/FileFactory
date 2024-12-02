using PersonDbLib;

using System.Reflection;
using System.Text;

namespace FactoryLib;
[Serializable]
internal class CsvParser<T> : IParser<T>
{
  private const char Separator = ',';
  public string Extension => "csv";

  public string Encode(List<T> data)
  {
    var result = new StringBuilder();
    result.Append(typeof(T).GetProperties().Select(x => string.Concat(x.Name[0].ToString().ToLower(), x.Name.AsSpan(1))).Aggregate((x1, x2) => x1 + Separator + x2) + "\n");
    foreach (var item in data)
    {
      result.Append(typeof(T).GetProperties().Select(x => x.GetValue(item)?.ToString()).Aggregate((x1, x2) => x1 + Separator + x2) + "\n");
    }
    return result.ToString();
  }
  public List<T> Parse(string fileName)
  {
    var result = new List<T>();

    var lines = File.ReadLines(fileName).ToList();

    string[] properties = lines[0].Split(Separator).Select(x => string.Concat(x[0].ToString().ToUpper(), x.AsSpan(1))).ToArray();

    foreach (string? item in lines.Skip(1))
    {
      string[] parts = item.Split(Separator);
      object? obj = Activator.CreateInstance(typeof(T));
      if (obj is T t)
      {
        for (int i = 0; i < properties.Length; i++)
        {
          var prop = t.GetType().GetProperty(properties[i]);
          if (prop != null && prop.CanWrite)
          {
            object? value = parts[i];
            if (prop.PropertyType.Name != "String")
            {
              var m = prop.PropertyType.GetMethod("Parse", [typeof(string)]);
              if (m is not MethodInfo method) continue;
              value = method.Invoke(null, [parts[i]]);
            }
            prop.SetValue(t, value, null);
          }
        }

        result.Add(t);
      }
    }
    return result;
  }
}
