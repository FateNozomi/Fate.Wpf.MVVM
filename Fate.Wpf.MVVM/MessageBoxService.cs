using System;
using System.Windows;

namespace Fate.Wpf.MVVM
{
    public class MessageBoxService : IMessageBoxService
    {
        public bool? Show(string text, string caption = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage image = MessageBoxImage.None)
        {
            MessageBoxResult result = MessageBox.Show(text, caption, button, image);

            if (result == MessageBoxResult.OK || result == MessageBoxResult.Yes)
            {
                return true;
            }
            else if (result == MessageBoxResult.No)
            {
                return false;
            }
            else
            {
                return null;
            }
        }

        public bool? Dispatch(string text, string caption = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage image = MessageBoxImage.None)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                return Show(text, caption, button, image);
            }
            else
            {
                bool? result = false;
                Application.Current.Dispatcher.Invoke(() =>
                    result = Show(text, caption, button, image));
                return result;
            }
        }
    }
}
