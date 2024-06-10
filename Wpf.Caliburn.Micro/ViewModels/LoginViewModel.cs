using Caliburn.Micro;
using System.Diagnostics;
using System.Threading;
using Wpf.CaliburnMicro.ViewModels.Login;

namespace Wpf.CaliburnMicro.ViewModels;

public class LoginViewModel : Screen
{
    private string? _userName;
    private readonly IEventAggregator _eventAggregator;

    public LoginViewModel(IEventAggregator eventAggregator)
    {
        DisplayName = "Login";
        UserName = "Test";
        _eventAggregator = eventAggregator;
    }

    public string? UserName
    {
        get => _userName;

        set
        {
            if (value == _userName) return;

            _userName = value;
            NotifyOfPropertyChange(() => UserName);
            NotifyOfPropertyChange(() => CanLogin);
        }
    }

    public void Login()
    {
        _eventAggregator.PublishAsync(new LoginStateChanged(new LoginInfo
        {
            UserName = UserName,
            CurrentState = LoginState.LoggedIn
        }),
        f => { return f(); },
        CancellationToken.None);
        Debug.WriteLine("Login");
    }

    public bool CanLogin => !string.IsNullOrWhiteSpace(UserName);
}
