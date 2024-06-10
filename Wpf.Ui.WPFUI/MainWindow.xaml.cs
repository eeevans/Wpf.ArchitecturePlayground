using System;
using System.Windows;
using System.Windows.Threading;
using WpfPlayground.Ui.Core.Mvvm;

namespace Wpf.Ui.WPFUI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : IFocusRequestViewModel
{
    public MainWindow()
    {
        DataContext = this;

        Appearance.SystemThemeWatcher.Watch(this);

        InitializeComponent();
        //notifyIcon = new NotifyIcon();
        //Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/Resources/Icons/applicationIcon-256.png")).Stream;
        //BitmapImage icon = new BitmapImage()
        //{
        //    StreamSource = iconStream
        //};
        //notifyIcon.Icon = icon;
        //notifyIcon.Register();

        //notifyIcon.Visibility = Visibility.Visible;
        //this.AddChild(notifyIcon);
        Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, () => RaiseFocusEvent(nameof(DisplayName)));

    }

    public string? DisplayName { get; set; }

    public event Action<string>? FocusRequested = (s) => { };

    private void Show_Click(object sender, RoutedEventArgs e)
    {
        MyPopup.IsOpen = true;
        Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, () => RaiseFocusEvent(nameof(DisplayName)));
    }

    private void Hide_Click(object sender, RoutedEventArgs e)
    {
        MyPopup.IsOpen = false;
    }

    private void MyPopup_Loaded(object sender, RoutedEventArgs e)
    {
        NameBox.Focus();
    }

    public void RaiseFocusEvent(string focusProperty)
    {
        FocusRequested?.Invoke(focusProperty);
    }
}
