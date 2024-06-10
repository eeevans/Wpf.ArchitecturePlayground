using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Wpf.ReferenceArchitecture.Core;

internal static class DispatcherService
{
    public static void Invoke(Action action)
    {
        Dispatcher dispatchObject = Application.Current.Dispatcher;

        if (dispatchObject == null || dispatchObject.CheckAccess())
        {
            action();
        }
        else
        {
            dispatchObject.Invoke(action);
        }
    }

    public static void BeginInvoke(Action action, DispatcherPriority priority = DispatcherPriority.Normal)
    {
        Dispatcher dispatchObject = Application.Current.Dispatcher;

        dispatchObject.BeginInvoke(action, priority);
    }

    public static Task InvokeAsync(Action action)
    {
        Dispatcher dispatchObject = Application.Current.Dispatcher;
        var tcs = new TaskCompletionSource<object>();

        if (dispatchObject == null || dispatchObject.CheckAccess())
        {
            action();
            tcs.SetResult(null);
            return tcs.Task;
        }

        dispatchObject.BeginInvoke(new Action(() =>
        {
            try
            {
                action();
                tcs.SetResult(null);
            }
            catch (Exception exception)
            {
                tcs.SetException(exception);
            }
        }));

        return tcs.Task;
    }

}
