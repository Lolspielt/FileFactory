using FactoryLib;

using Microsoft.Win32;

using PersonDbLib;

using System.IO;
using System.Windows;
using System.Windows.Controls;

using static System.Net.WebRequestMethods;

namespace FileFormatsFactory;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
  public MainWindow() => InitializeComponent();

  private readonly string[] _filenames = ["persons"];

  private void Window_Loaded(object sender, RoutedEventArgs e)
  {
    CreateRadios();
    CreateButtons();
  }

  private void CreateRadios()
  {
    foreach (string filename in _filenames)
    {
      var radio = new RadioButton { Content = filename};
      panTypes.Children.Add(radio);
    }
    panTypes.Children.OfType<RadioButton>().First().IsChecked = true;
  }

  private void CreateButtons()
  {
    foreach (string parserName in ParserFactory<Person>.Instance.ParserNames)
    {
      var button = new Button { Content = parserName, Margin = new() { Left = 2, Right = 2 }, IsEnabled = false };
      button.Click += (_, _) =>
      {
        var parser = ParserFactory<Person>.Instance.Create(parserName);
        string type = panTypes.Children.OfType<RadioButton>().First(x => x.IsChecked ?? false).Content.ToString() ?? "";
        string fileName = $"{txtFoldername.Text}/{type}.{parserName}";
        lblFilename.Content = "Filename: " + fileName;
        var items = parser.Parse(fileName);
        lstResult.ItemsSource = items;
        lblMessage.Content = $"{items.Count} {type} loaded from {fileName}";
        panSave.Children.OfType<Button>().ToList().ForEach(button => button.IsEnabled = true);
      };
      panRead.Children.Add(button);
      button = new Button { Content = parserName, Margin = new() { Left = 2, Right = 2 }, IsEnabled = false };
      button.Click += (_, _) =>
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

  private void TxtFoldername_TextChanged(object sender, TextChangedEventArgs e)
  {
    try
    {
      string[] files = Directory.GetFiles(txtFoldername.Text);
      string type = panTypes.Children.OfType<RadioButton>().First(x => x.IsChecked ?? false).Content.ToString() ?? "";
      foreach (var button in panRead.Children.OfType<Button>())
      {
        button.IsEnabled = files.Any(x => x.EndsWith($"{type}.{button.Content}"));
      }
    }
    catch 
    {
      foreach (var button in panRead.Children.OfType<Button>())
      {
        button.IsEnabled = false;
      }
    }
  }
}
