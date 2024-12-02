using PersonDbLib;

namespace FactoryLib;

public interface IParser<T>
{
  List<T> Parse(string fileName);
  string Encode(List<T> data);
  string Extension { get; }
}
