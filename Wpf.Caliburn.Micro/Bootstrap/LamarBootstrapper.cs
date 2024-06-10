using Caliburn.Micro;
using Lamar;
using Lamar.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows;
using Wpf.CaliburnMicro.Core;
using Wpf.CaliburnMicro.ViewModels;

namespace Wpf.CaliburnMicro.Bootstrap;

internal class LamarBootstrapper : BootstrapperBase
{
    private IContainer container;

    public LamarBootstrapper()
    {
        Initialize();
    }


    protected override void Configure()
    {
        container = new Container(c =>
        {
            c.ForSingletonOf<IWindowManager>().Use<WindowManager>();
            c.ForSingletonOf<IEventAggregator>().Use<EventAggregator>();
            c.For(typeof(IFactory<>)).Use(typeof(Factory<>));
            c.AddTransient<RootViewModel>();
            c.AddTransient<LoginViewModel>();
            c.AddTransient<MainAppViewModel>();
        });
        Console.WriteLine(container.WhatDoIHave());

    }

    protected override async void OnStartup(object sender, StartupEventArgs e)
    {
        await DisplayRootViewForAsync<RootViewModel>();
    }

    protected override object GetInstance(Type service, string key)
    {
        return null == key ? container.GetInstance(service) : container.GetInstance(service, key);
    }

    protected override IEnumerable<object>? GetAllInstances(Type service)
    {
        return container.GetAllInstances(service) as IEnumerable<object>;
    }

    protected override void BuildUp(object instance)
    {
        if (container is Scope scope)
            scope.BuildUp(instance);
    }

}
