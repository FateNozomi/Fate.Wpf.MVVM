using System.Windows;

namespace Fate.Wpf.MVVM
{
    public interface IWindowService
    {
        void Show<T>(object dataContext)
            where T : Window, new();

        bool? ShowDialog<T>(object dataContext)
            where T : Window, new();

        T CreateWindow<T>(object dataContext)
            where T : Window, new();
    }
}
