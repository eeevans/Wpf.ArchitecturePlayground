using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.Binding;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    Dictionary<string, Func<Page>> _pageFactory = new Dictionary<string, Func<Page>>
    {
        { "bindPropertyButton", () => new BindPropertyToDcProperty() },
    };

    public MainWindow()
    {
        InitializeComponent();
    }

    private void NavigateButtonOnClick(object sender, RoutedEventArgs e)
    {
        Page? pageToNavigate = null;
        Button? button = sender as Button;
        if (button is null) return;

        pageToNavigate = _pageFactory.TryGetValue(button.Name, out Func<Page>? factory) ? factory() : null;

        if (pageToNavigate is not null)
        {
            button.Tag = pageToNavigate;
            button.IsEnabled = false;
            navigationFrame.Navigate(pageToNavigate);
        }
    }
}