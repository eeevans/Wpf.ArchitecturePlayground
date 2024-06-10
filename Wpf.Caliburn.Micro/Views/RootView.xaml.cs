using System;
using System.Windows;
using System.Windows.Interop;
using Wpf.ReferenceArchitecture.Core;

namespace Wpf.CaliburnMicro.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class RootView : Window
{
    public WindowState RestoredWindowState { get; set; }

    public RootView()
    {
        InitializeComponent();
        RefreshMaximizeRestoreButton();
        Loaded += (e, o) =>
        {
            RestoredWindowState = WindowState;
        };
    }

    private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void OnMaximizeRestoreButtonClick(object sender, RoutedEventArgs e)
    {
        if (WindowState == WindowState.Maximized)
        {
            WindowState = WindowState.Normal;
        }
        else
        {
            WindowState = WindowState.Maximized;
        }
    }

    private void OnCloseButtonClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void RefreshMaximizeRestoreButton()
    {
        if (WindowState == WindowState.Maximized)
        {
            maximizeButton.Visibility = Visibility.Collapsed;
            restoreButton.Visibility = Visibility.Visible;
        }
        else
        {
            maximizeButton.Visibility = Visibility.Visible;
            restoreButton.Visibility = Visibility.Collapsed;
        }
    }

    public void RestoreWindow()
    {
        DispatcherService.BeginInvoke(() =>
        {
            Show();
            WindowState = RestoredWindowState;
        });
    }

    private void Window_StateChanged(object sender, EventArgs e)
    {
        if (WindowState == WindowState.Maximized || WindowState == WindowState.Normal)
        {
            RestoredWindowState = WindowState;
        }

        RefreshMaximizeRestoreButton();
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        ((HwndSource)PresentationSource.FromVisual(this)).AddHook(WindowPlacement.HookProc);
    }

}
