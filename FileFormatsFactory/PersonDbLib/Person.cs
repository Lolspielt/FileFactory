namespace PersonDbLib;
[Serializable]
public class Person
{
  //id,firstname,lastname,email,gender,birthDate,adressId
  public int Id { get; set; }
  public required string Firstname { get; set; }
  public required string Lastname { get; set; }
  public required string Email { get; set; }
  public required string Gender { get; set; }
  public DateOnly BirthDate { get; set; }
  public int AdressId { get; set; }

  //public Adress Adress { get; set; } = null!;

  //private TimeSpan DateDiff => DateTime.Now - BirthDate.ToDateTime(new());
  //private double AgeYears => DateDiff.TotalDays / 365.25;
  override public string ToString() => $"{Firstname} {Lastname} [{BirthDate:dd.MM.yyyy}] / {AdressId}";

}
