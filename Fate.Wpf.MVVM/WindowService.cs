using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Fate.Wpf.MVVM
{
    public class WindowService : IWindowService
    {
        public void Show<T>(IWindow dataContext)
            where T : Window, new()
        {
            T window = new T();
            WindowConductor conductor = new WindowConductor(dataContext, window);
            conductor.Initialize();
            window.Show();
        }

        public bool? ShowDialog<T>(IWindow dataContext)
            where T : Window, new()
        {
            T window = new T();
            WindowConductor conductor = new WindowConductor(dataContext, window);
            conductor.Initialize();
            return window.ShowDialog();
        }

        public Task<bool?> ShowWindow<T>(IWindow dataContext, CancellationToken token)
            where T : Window, new()
        {
            return ShowWindow<T>(dataContext, null, token);
        }

        public async Task<bool?> ShowWindow<T>(IWindow dataContext, IWindow parentContext, CancellationToken token)
            where T : Window, new()
        {
            T window = new T();
            WindowConductor conductor = new WindowConductor(dataContext, window);
            conductor.Initialize();

            SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
            window.Closed += (s, e) => semaphore.Release();

            try
            {
                parentContext?.Enable(false);
                window.Show();
                await semaphore.WaitAsync(token);
                semaphore.Dispose();
                return dataContext.WindowResult;
            }
            finally
            {
                parentContext?.Enable(true);
            }
        }

        private class WindowConductor
        {
            private IWindow _viewModel;
            private Window _view;
            private bool actuallyClosing;

            public WindowConductor(IWindow viewModel, Window view)
            {
                _viewModel = viewModel;
                _view = view;
            }

            public void Initialize()
            {
                _view.DataContext = _viewModel;
                _view.Loaded += View_Loaded;
                _view.Closing += View_Closing;

                _viewModel.Close = () => _view.Close();
                _viewModel.Enable = enable => _view.IsEnabled = enable;
            }

            private async void View_Loaded(object sender, RoutedEventArgs e)
            {
                await _viewModel.OnLoadedAsync();
            }

            private async void View_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            {
                if (actuallyClosing)
                {
                    actuallyClosing = false;
                    return;
                }

                e.Cancel = true;
                await Task.Yield();
                bool canClose = await _viewModel.CanCloseAsync();
                if (canClose)
                {
                    actuallyClosing = true;
                    _view.Close();
                }
            }
        }
    }
}
