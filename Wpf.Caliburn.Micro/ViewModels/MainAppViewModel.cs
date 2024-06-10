using Caliburn.Micro;
using Wpf.CaliburnMicro.ViewModels.Login;

namespace Wpf.CaliburnMicro.ViewModels;

public class MainAppViewModel : Screen
{
    private readonly IEventAggregator _eventAggregator;

    public LoginInfo? LoginInfo { get; private set; }

    public MainAppViewModel(IEventAggregator eventAggregator)
    {
        DisplayName = "Main App";
        _eventAggregator = eventAggregator;
    }

    public void TrySetLoginInfo(LoginInfo loginInfo)
    {
        LoginInfo ??= loginInfo;
    }

    public string GetGreeting => LoginInfo is null ? "Please login" : $"Welcome, {LoginInfo?.UserName}";

    public async void LogoutCommand()
    {
        await _eventAggregator.PublishOnUIThreadAsync(new LoginStateChanged(new LoginInfo
        {
            UserName = null,
            CurrentState = LoginState.NotLoggedIn
        }));
    }
}
