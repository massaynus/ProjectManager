using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows_CLient.Models;

namespace Windows_CLient.ViewModels
{
    public class WorkersViewModel : BaseViewModel
    {
        //TODO: complete the WorkersViewModel
        //TODO: Write commands
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<Team> Teams { get; set; }
        public User SelectedUser { get; set; }
        public ICommand GetUsers { get; set; }
        public ICommand GetTeams { get; set; }
        public ICommand ResetPassword { get; set; }
        public ICommand UpdateUser { get; set; }
        public ICommand DisableUserAccount { get; set; }
        public ICommand EnableUserAccount { get; set; }

    }
}
