using PersonDbLib;

using System.Text.Json;

namespace FactoryLib;
[Serializable]
internal class JsonParser<T> : IParser<T>
{
  public string Extension => "json";

  public string Encode(List<T> data) => JsonSerializer.Serialize(data);
  public List<T> Parse(string fileName) => JsonSerializer.Deserialize<List<T>>(File.ReadAllText(fileName)) ?? [];
}
