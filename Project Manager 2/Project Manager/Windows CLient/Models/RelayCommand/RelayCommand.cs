using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Windows_CLient
{
    class RelayCommand : ICommand
    {
        Action action;
        Action<object> parametrizedAction;
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public RelayCommand(Action<object> action)
        {
            parametrizedAction = action;
        }

        public RelayCommand(Action action)
        {
            this.action = action;
        }

        public bool CanExecute(object parameter) => true;

        public virtual void Execute(object parameter)
        {
            if (action != null)
            {
                action();
                return;
            }
            if (parametrizedAction != null)
            {
                parametrizedAction(parameter);
                return;
            }
        }
    }
}
