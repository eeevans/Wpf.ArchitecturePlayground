using System;

namespace WpfPlayground.Ui.Core.Mvvm;

public interface IFocusRequestViewModel
{
    event Action<string> FocusRequested;
    void RaiseFocusEvent(string focusProperty);
}

