using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.Binding;

/// <summary>
/// Interaction logic for BindPropertyToDcProperty.xaml
/// </summary>
public partial class BindPropertyToDcProperty
{
    private const string ADDED_BUTTON_TEXT = "Remove from list";
    private AppSession appSession;
    public BindPropertyToDcProperty()
    {
        InitializeComponent();
        appSession = new AppSession();
        this.DataContext = appSession;
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var btn = new Button();
        btn.Content = ADDED_BUTTON_TEXT;
        btn.Click += new RoutedEventHandler(AddedButtonOnClick);

        ButtonList.Items.Add(btn);
    }

    private void AddedButtonOnClick(object sender, RoutedEventArgs e)
    {
        ButtonList.Items.Remove(sender);
    }

    private void ToggleSizeButton_Click(object sender, RoutedEventArgs e)
    {
        appSession.FontSizeLabel = appSession.FontSizeLabel < 20 ? 25 : 16;
    }
}

public class AppSession : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged = (o, e) => { };

    double _fontSizeLabel = 16;

    public double FontSizeLabel { get { return _fontSizeLabel; } set { _fontSizeLabel = value; OnPropertyChanged("FontSizeLabel"); } }

    private void OnPropertyChanged(string name)
    {
        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
