using Lamar;
using System;

namespace Wpf.CaliburnMicro.Core;

public interface IFactory<T> where T : class
{
    T? Create();
    T? Create(Action<T> initAction);
}

public class Factory<T> : IFactory<T> where T : class
{
    private readonly IContainer _container;

    public Factory(IContainer container)
    {
        _container = container;
    }

    public T? Create(Action<T> initAction)
    {
        T instance = _container.GetInstance<T>();
        initAction(instance);
        return instance;
    }

    public T? Create()
    {
        _container.GetInstance<T>();
        return _container.GetInstance<T>() as T; // .GetService<T>() as T;
    }

}
