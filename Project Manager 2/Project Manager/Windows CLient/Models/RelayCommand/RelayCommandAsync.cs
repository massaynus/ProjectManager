using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Windows_CLient
{
    class RelayCommandAsync : ICommand
    {
        Func<Task> func;

        public RelayCommandAsync(Func<Task> func)
        {
            this.func = func;
        }

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public bool CanExecute(object parameter) => true;

        [STAThread]
        public async void Execute(object parameter)
        {
            if (func != null)
            {
                try
                {
                    await func();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"{e.Message}\nInner Exception\t{e.InnerException.GetType()}", e.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return;
        }
    }
}
