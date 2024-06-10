using Caliburn.Micro;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Wpf.CaliburnMicro.Core;
using Wpf.CaliburnMicro.ViewModels.Login;

namespace Wpf.CaliburnMicro.ViewModels;

public class RootViewModel : Conductor<IScreen>.Collection.OneActive, IHandle<LoginStateChanged>
{
    private string? _message;
    public string? Message
    {
        get => _message; set
        {
            _message = value;
            NotifyOfPropertyChange(nameof(Message));
        }
    }

    public string WindowTitle => "WPF Reference Architecture";

    private readonly IEventAggregator _eventAggregator;
    private IFactory<LoginViewModel> _loginFactory;
    private IFactory<MainAppViewModel> _mainFactory;

    public RootViewModel(IEventAggregator eventAggregator,
        IFactory<LoginViewModel> loginFactory,
        IFactory<MainAppViewModel> mainFactory)
    {
        _eventAggregator = eventAggregator;
        _loginFactory = loginFactory;
        _mainFactory = mainFactory;

        _eventAggregator.SubscribeOnUIThread(this);
    }

    protected override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        await ShowLogin(cancellationToken);
        //await ActivateItemAsync(new TestImageViewModel(), cancellationToken);
    }

    private async Task ShowLogin(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested) return;

        var loginScreen = _loginFactory.Create();
        if (loginScreen is null) Debugger.Break();

        await ActivateItemAsync(loginScreen!, cancellationToken);
    }

    private async Task ShowMain(LoginInfo loginInfo, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested) return;

        var mainAppScreen = _mainFactory.Create(m => m.TrySetLoginInfo(loginInfo));
        if (mainAppScreen is null) Debugger.Break();

        await ActivateItemAsync(mainAppScreen!, cancellationToken);
    }

    public async Task HandleAsync(LoginStateChanged stateChanged, CancellationToken cancellationToken)
    {
        if (stateChanged.LoginInfo?.CurrentState is LoginState.NotLoggedIn)
        {
            await ShowLogin(cancellationToken);
        }
        else if (stateChanged.LoginInfo?.CurrentState is LoginState.LoggedIn)
        {
            await ShowMain(stateChanged.LoginInfo, cancellationToken);
        }

        if (stateChanged.LoginInfo?.CurrentState is not LoginState.LoggedIn) return;

    }
}
