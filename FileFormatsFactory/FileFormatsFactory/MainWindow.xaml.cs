using FactoryLib;

using Microsoft.Win32;

using PersonDbLib;

using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace FileFormatsFactory;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
  public MainWindow() => InitializeComponent();

  private void Window_Loaded(object sender, RoutedEventArgs e)
  {
    CreateButtons();
  }

  private void CreateButtons()
  {
    foreach (string parserName in ParserFactory<Person>.Instance.ParserNames)
    {
      var button = new Button { Content = parserName, Margin = new() { Left = 2, Right = 2 } };
      button.Click += (s, e) =>
      {
        var parser = ParserFactory<Person>.Instance.Create(parserName);
        lblFilename.Content ="Filename: " + txtFoldername.Text + "/persons." + parserName;
        var items = parser.Parse(lblFilename.Content.ToString() ?? "");
        lstResult.ItemsSource = items;
        lblMessage.Content = $"{items.Count} persons loaded from {lblFilename.Content}.{parserName}";
      };
      panRead.Children.Add(button);
      button = new Button { Content = parserName, Margin = new() {Left = 2, Right = 2} };
      button.Click += (s, e) =>
      {
        var parser = ParserFactory<Person>.Instance.Create(parserName);
        txtRawString.Text = parser.Encode(lstResult.ItemsSource.OfType<Person>().ToList());
      };
      panSave.Children.Add(button);
    }
  }

  private void FileDialogButtonClick(object sender, RoutedEventArgs e)
  {
    var dialog = new OpenFileDialog
    {
      InitialDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName,
      Filter = "Alle Dateien (*.*)|*.*"
    };
    foreach (string parserName in ParserFactory<Person>.Instance.ParserNames)
    {
      dialog.Filter += $"|{parserName} files (*.{parserName})|*.{parserName}";
    }
    
    if (dialog.ShowDialog() == true)
      txtFoldername.Text = Path.GetDirectoryName(dialog.FileName);
  }
}
