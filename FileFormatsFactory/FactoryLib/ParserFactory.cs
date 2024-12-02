using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;

namespace FactoryLib;
public class ParserFactory<T>
{
  private static ParserFactory<T>? _instance = null;
  public static ParserFactory<T> Instance => _instance ??= new ParserFactory<T>();
  public string[] ParserNames => _prototypes.Keys.Order().ToArray();

  private readonly Dictionary<string, IParser<T>> _prototypes = [];
  private ParserFactory()
  {
    Assembly.GetExecutingAssembly().GetTypes()
      .Where(x => !x.IsAbstract && !x.IsInterface)
      .Where(x => x.BaseType != null)
      .Where(x => DoesImplement(x.MakeGenericType(typeof(T)),typeof(IParser<T>)))
      .ToList()
      .ForEach(Register);
  }

  public IParser<T> Create(string name)
  {
    return _prototypes.ContainsKey(name)
      ? Clone(_prototypes[name])
      : throw new ArgumentException($"Unknown type <${name}>");
  }

  private void Register(Type type)
  {
    IParser<T> parser = (Activator.CreateInstance(type.MakeGenericType(typeof(T))) as IParser<T>)!;
    Console.WriteLine($"Register prototype {parser.Extension}");
    _prototypes[parser.Extension] = parser;
  }

  private IParser<T> Clone(IParser<T> parser)
  {
    var stream = new MemoryStream();
#pragma warning disable SYSLIB0011 // Type or member is obsolete
    var formatter = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolete
    formatter.Serialize(stream, parser);
    stream.Seek(0, SeekOrigin.Begin);
    var copy = (IParser<T>)formatter.Deserialize(stream);
    stream.Close();
    return copy;
  }

  private static bool DoesImplement(Type classType, Type genericInterfaceType)
  {
    return classType.GetInterfaces()
    .Any(x => x.IsGenericType
    && x == genericInterfaceType);
  }
}
