using Caliburn.Micro;
using System.Collections.Generic;
using System.Windows;
using System;
using Wpf.ReferenceArchitecture.ViewModels;
using Wpf.ReferenceArchitecture.Core;

namespace Wpf.ReferenceArchitecture.Bootstrap;
internal class SimpleContainerBootstrapper : BootstrapperBase
{
    private SimpleContainer container;

    public SimpleContainerBootstrapper()
    {
        Initialize();
    }

    
    protected override void Configure()
    {
        container = new SimpleContainer();
        container.RegisterInstance(typeof(SimpleContainer), "SimpleContainer", container);


        container.Singleton<IWindowManager, WindowManager>();

        container.PerRequest<RootViewModel>();
        container.PerRequest<LoginViewModel>();
        container.RegisterPerRequest(typeof(IFactory<LoginViewModel>), null, typeof(CaliburnFactory<LoginViewModel>));
        var t = container.GetInstance<IFactory<LoginViewModel>>();
        if (t != null) { 
            Console.WriteLine("nope"); 
        }
    }

    protected override async void OnStartup(object sender, StartupEventArgs e)
    {
        await DisplayRootViewForAsync<RootViewModel>();
    }

    protected override object GetInstance(Type service, string key)
    {
        return container.GetInstance(service, key);
    }

    protected override IEnumerable<object> GetAllInstances(Type service)
    {
        return container.GetAllInstances(service);
    }

    protected override void BuildUp(object instance)
    {
        container.BuildUp(instance);
    }
}
