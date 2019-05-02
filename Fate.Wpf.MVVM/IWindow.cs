using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fate.Wpf.MVVM
{
    public interface IWindow
    {
        bool? WindowResult { get; set; }

        Action Close { get; set; }

        Action<bool> Enable { get; set; }

        Task OnLoadedAsync();

        Task<bool> CanCloseAsync();
    }
}
