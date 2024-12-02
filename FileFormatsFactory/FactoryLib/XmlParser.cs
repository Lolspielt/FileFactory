using PersonDbLib;

using System.Xml;
using System.Xml.Serialization;

namespace FactoryLib;
[Serializable]
internal class XmlParser<T> : IParser<T>
{
  public string Extension => "xml";

  public string Encode(List<T> data)
  {
    var xmlserializer = new XmlSerializer(typeof(List<T>));
    var stringWriter = new StringWriter();
    var writer = XmlWriter.Create(stringWriter);

    xmlserializer.Serialize(writer, data);
    return stringWriter.ToString();
  }

  public List<T> Parse(string fileName)
  {
    var xmlStream = new StreamReader(fileName);
    var serializer = new XmlSerializer(typeof(List<T>));
    return (List<T>)serializer.Deserialize(xmlStream)!;
  }
}
