using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfPlayground.Ui.Core.Behaviors;

public abstract class FocusBehaviourBase : Behavior<FrameworkElement>
{
    /// <summary>
    /// Conducts the focus on element.
    /// </summary>
    /// <param name="elementBinding">The element binding.</param>
    /// <param name="propertyPath">The property path.</param>
    /// <param name="isUsingDataWrappers">if set to <c>true</c> [is using data wrappers].</param>
    protected virtual void ConductFocusOnElement(Binding elementBinding, string propertyPath, bool isUsingDataWrappers)
    {
        if (elementBinding == null)
        {
            return;
        }
        if (isUsingDataWrappers)
        {
            if (!elementBinding.Path.Path.Contains(propertyPath))
            {
                return;
            }
        }
        else if (elementBinding.Path.Path != propertyPath)
        {
            return;
        }
        AssociatedObject.Dispatcher.BeginInvoke(new Action(delegate ()
        {
            if (AssociatedObject.Focus())
            {
                return;
            }
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(AssociatedObject), AssociatedObject);
        }), DispatcherPriority.Background, Array.Empty<object>());
    }
}
