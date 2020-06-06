using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TT = System.Threading.Tasks;
using System.Windows.Input;
using Windows_CLient.Models;$
using static Windows_CLient.APIClient;
using Newtonsoft.Json;
using System.Windows;

namespace Windows_CLient.ViewModels
{
    public class WorkersViewModel : BaseViewModel
    {
        private User selectedUser = new User();
        public WorkersViewModel()
        {
            NewUser = new RelayCommand(newUser);
            
            Users = new ObservableCollection<User>();
            Teams = new ObservableCollection<Team>();

            AddUser = new RelayCommandAsync(addUser);
            UpdateUser = new RelayCommandAsync(updateUser);
            DisableUserAccount = new RelayCommandAsync(disableUserAccount);
            EnableUserAccount = new RelayCommandAsync(enableUserAccount);

            GetUsers = new RelayCommandAsync(getUsers);
            GetTeams = new RelayCommandAsync(getTeams);
        }
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<Team> Teams { get; set; }
        public User SelectedUser
        {
            get => selectedUser;
            set
            {
                if (selectedUser != value)
                {
                    selectedUser = value;
                    OnPropertyChanged(nameof(SelectedUser));
                }
            }
        }

        public ICommand GetUsers { get; set; }
        public ICommand GetTeams { get; set; }
        public ICommand NewUser { get; set; }
        public ICommand AddUser { get; set; }
        public ICommand UpdateUser { get; set; }
        public ICommand DisableUserAccount { get; set; }
        public ICommand EnableUserAccount { get; set; }

        async TT.Task getUsers()
        {
            Users = JsonConvert.DeserializeObject<ObservableCollection<User>>(await MakeApiCall(APIClient.Action.GET, Controller.Users));
            OnPropertyChanged(nameof(Users));
        }
        async TT.Task getTeams()
        {
            Teams = JsonConvert.DeserializeObject<ObservableCollection<Team>>(await MakeApiCall(APIClient.Action.GET, Controller.Teams));
            OnPropertyChanged(nameof(Teams));
        }

        void newUser() => SelectedUser = new User() { Manager = APIClient.User.UserID };
        async TT.Task addUser()
        {
            string JSON = await MakeApiCall(APIClient.Action.POST, Controller.Users, SelectedUser);
            if (JSON.Contains("UserID"))
                MessageBox.Show("User info was added !!", "Great", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("User info wasn't added", "Oops!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        
        async TT.Task updateUser()
        {
            string JSON = await MakeApiCall(APIClient.Action.PUT, Controller.Users, SelectedUser, SelectedUser.UserID.ToString());
            if (JSON.Contains("UserID"))
                MessageBox.Show("User info was updated !!", "Great", MessageBoxButton.OK, MessageBoxImage.Information);
            else 
                MessageBox.Show("User info wasn't updated", "Oops!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        async TT.Task enableUserAccount()
        {
            SelectedUser.isAccountActive = true;
            await updateUser();
        }
        async TT.Task disableUserAccount()
        {
            SelectedUser.isAccountActive = false;
            await updateUser();
        }


    }
}
