
using System.Reflection;
using System.Text.Json;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;

namespace FactoryLib;
[Serializable]
internal class BinaryParser<T> : IParser<T>
{
  public string Extension => "bin";

  public string Encode(List<T> data)
  {
    string jsonData = JsonSerializer.Serialize(data);

    byte[] byteArray = Encoding.UTF8.GetBytes(jsonData);

    string base64String = Convert.ToBase64String(byteArray);

    return base64String;
  }

  public List<T> Parse(string fileName)
  {
    string base64String = File.ReadAllText(fileName);

    byte[] byteArray = Convert.FromBase64String(base64String);

    string jsonData = Encoding.UTF8.GetString(byteArray);

    List<T> data = JsonSerializer.Deserialize<List<T>>(jsonData);

    return data;
  }
}
