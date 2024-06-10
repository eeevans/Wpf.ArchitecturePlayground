using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WpfPlayground.Core.Extensions;
using WpfPlayground.Ui.Core.Mvvm;

namespace WpfPlayground.Ui.Core.Behaviors;

public class BaseEditFocusBehaviour : FocusBehaviourBase
{
    protected DependencyProperty GetSourceProperty() { return TextBox.TextProperty; }

    protected override void OnAttached()
    {
        if (AssociatedObject.As<TextBox>() == null) return;

        base.OnAttached();

        AssociatedObject.Loaded += AssociatedObjectLoaded;
    }

    void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
    {
        if (AssociatedObject.DataContext.As<IFocusRequestViewModel>() == null) return;

        ((IFocusRequestViewModel)AssociatedObject.DataContext).FocusRequested += TextBoxFocusBehaviorFocusRequested;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        AssociatedObject.Loaded -= AssociatedObjectLoaded;

        if (AssociatedObject.DataContext.As<IFocusRequestViewModel>() == null) return;

        ((IFocusRequestViewModel)AssociatedObject.DataContext).FocusRequested -= TextBoxFocusBehaviorFocusRequested;
    }


    private void TextBoxFocusBehaviorFocusRequested(string propertyPath)
    {
        var binding = BindingOperations.GetBinding(AssociatedObject, GetSourceProperty());
        base.ConductFocusOnElement(binding, propertyPath, IsUsingDataWrappers);
    }

    public static readonly DependencyProperty IsUsingDataWrappersProperty = DependencyProperty.Register("IsUsingDataWrappers", typeof(bool), typeof(BaseEditFocusBehaviour), new FrameworkPropertyMetadata(false));

    public bool IsUsingDataWrappers
    {
        get { return (bool)GetValue(IsUsingDataWrappersProperty); }
        set { SetValue(IsUsingDataWrappersProperty, value); }
    }


}