namespace PersonDbLib;
[Serializable]
public class Adress
{
  public int Id { get; set; }
  public required string Country { get; set; }
  public required string City { get; set; }
  public int? PostalCode { get; set; }
  public required string StreetName { get; set; }
  public int StreetNumber { get; set; }

  public override string ToString() => $"{Country}: {PostalCode}-{City}, {StreetName} {StreetNumber}";
}
