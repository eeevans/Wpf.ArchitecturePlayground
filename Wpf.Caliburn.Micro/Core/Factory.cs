using Caliburn.Micro;
using Lamar;
using Microsoft.Extensions.DependencyInjection;

namespace Wpf.ReferenceArchitecture.Core;

public interface IFactory<T> where T : class
{
    T Create();
}

public class Factory<T> : IFactory<T> where T : class
{
    private readonly IContainer _container;

    public Factory(IContainer container)
    {
        this._container = container;
    }

    public T Create()
    {
        return _container.GetService<T>() as T;
    }

}

public class CaliburnFactory<T> : IFactory<T> where T : class
{
    private readonly SimpleContainer _container;

    public CaliburnFactory(SimpleContainer container)
    {
        this._container = container;
    }

    public T Create()
    {
        return _container.GetInstance<T>() as T;
    }

}
