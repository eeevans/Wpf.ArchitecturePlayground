using Caliburn.Micro;
using System;
//using Microsoft.Extensions.DependencyInjection;

namespace Wpf.CaliburnMicro.Core;

public class CaliburnFactory<T> : IFactory<T> where T : class
{
    private readonly SimpleContainer _container;

    public CaliburnFactory(SimpleContainer container)
    {
        _container = container;
    }

    public T Create()
    {
        return _container.GetInstance<T>() as T;
    }

    public T? Create(Action<T> initAction)
    {
        var instance = Create();
        initAction(instance);
        return instance;
    }
}
