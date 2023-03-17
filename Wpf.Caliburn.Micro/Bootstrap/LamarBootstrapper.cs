using Caliburn.Micro;
using Lamar;
using Lamar.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows;
using Wpf.ReferenceArchitecture.Core;
using Wpf.ReferenceArchitecture.ViewModels;

namespace Wpf.ReferenceArchitecture.Bootstrap;
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
            c.For<IWindowManager>().Use<WindowManager>();
            c.For(typeof(IFactory<>)).Use(typeof(Factory<>));
            c.AddTransient<RootViewModel>();
            c.AddTransient<LoginViewModel>();
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
        return container.GetAllInstances(service) as IEnumerable<Object>;
    }

    protected override void BuildUp(object instance)
    {
        Scope scope = container as Scope;
        scope.BuildUp(instance);
    }

}
