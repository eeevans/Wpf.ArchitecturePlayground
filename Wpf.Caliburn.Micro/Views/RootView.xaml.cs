using System;
using System.Windows;
using System.Windows.Interop;
using Wpf.ReferenceArchitecture.Core;

namespace Wpf.ReferenceArchitecture.Views;

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
        this.Loaded += (e, o) =>
        {
            this.RestoredWindowState = this.WindowState;
        };
    }

    private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Minimized;
    }

    private void OnMaximizeRestoreButtonClick(object sender, RoutedEventArgs e)
    {
        if (this.WindowState == WindowState.Maximized)
        {
            this.WindowState = WindowState.Normal;
        }
        else
        {
            this.WindowState = WindowState.Maximized;
        }
    }

    private void OnCloseButtonClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void RefreshMaximizeRestoreButton()
    {
        if (this.WindowState == WindowState.Maximized)
        {
            this.maximizeButton.Visibility = Visibility.Collapsed;
            this.restoreButton.Visibility = Visibility.Visible;
        }
        else
        {
            this.maximizeButton.Visibility = Visibility.Visible;
            this.restoreButton.Visibility = Visibility.Collapsed;
        }
    }

    public void RestoreWindow()
    {
        DispatcherService.BeginInvoke(() =>
        {
            this.Show();
            this.WindowState = this.RestoredWindowState;
        });
    }

    private void Window_StateChanged(object sender, EventArgs e)
    {
        if (this.WindowState == WindowState.Maximized || this.WindowState == WindowState.Normal)
        {
            this.RestoredWindowState = this.WindowState;
        }

        this.RefreshMaximizeRestoreButton();
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        ((HwndSource)PresentationSource.FromVisual(this)).AddHook(WindowPlacement.HookProc);
    }

}
