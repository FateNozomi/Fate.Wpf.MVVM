using System;
using System.Windows;
using System.Windows.Threading;

namespace Fate.Wpf.MVVM
{
    public sealed class ApplicationDispatcherLocator
    {
        private static readonly Lazy<ApplicationDispatcherLocator> _applicationDispatcherLocator =
            new Lazy<ApplicationDispatcherLocator>(() => new ApplicationDispatcherLocator());

        private Dispatcher _dispatcher = null;

        private ApplicationDispatcherLocator() { }

        public static ApplicationDispatcherLocator Instance => _applicationDispatcherLocator.Value;

        public Dispatcher Dispatcher => _dispatcher != null ? _dispatcher : Application.Current.Dispatcher;

        public void SetDispatcher(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
    }
}
