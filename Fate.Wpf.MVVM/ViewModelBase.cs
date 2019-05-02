using System;
using System.Threading.Tasks;

namespace Fate.Wpf.MVVM
{
    public class ViewModelBase : PropertyChangedBase, IWindow
    {
        public bool? WindowResult { get; set; }

        public Action Close { get; set; }

        public Action<bool> Enable { get; set; }

        public virtual Task OnLoadedAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task<bool> CanCloseAsync()
        {
            return Task.FromResult(true);
        }
    }
}
