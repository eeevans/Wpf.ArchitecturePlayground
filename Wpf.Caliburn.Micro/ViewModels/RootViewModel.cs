using Caliburn.Micro;
using System.Threading;
using System.Threading.Tasks;
using Wpf.ReferenceArchitecture.Core;

namespace Wpf.ReferenceArchitecture.ViewModels;

public class RootViewModel : Conductor<IScreen>.Collection.OneActive, IHandle<string>
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

    private IFactory<LoginViewModel> _loginFactory; 

    public RootViewModel(IFactory<LoginViewModel> loginFactory)
    {
        _loginFactory = loginFactory;
    }

    protected override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        await ActivateItemAsync(_loginFactory.Create(), cancellationToken);
    }



    public Task HandleAsync(string message, CancellationToken cancellationToken)
    {
        //this.Message = message.ToString();
        return Task.CompletedTask;
    }
}
