using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TT = System.Threading.Tasks;
using System.Windows.Input;
using Windows_CLient.Models;
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
            Sexes.Add(new { T = "Male", V = "M" });
            Sexes.Add(new { T = "Female", V = "F" });

            NewUser = new RelayCommand(newUser);
            
            Users = new ObservableCollection<User>();
            Teams = new ObservableCollection<Team>();

            AddUser = new RelayCommandAsync(addUser);
            UpdateUser = new RelayCommandAsync(updateUser);
            DisableUserAccount = new RelayCommandAsync(disableUserAccount);
            EnableUserAccount = new RelayCommandAsync(enableUserAccount);

            GetUsers = new RelayCommandAsync(getUsers);
            GetTeams = new RelayCommandAsync(getTeams);
            GetRoles = new RelayCommandAsync(getRoles);

            GetUsers.Execute(null);
            GetTeams.Execute(null);
            GetRoles.Execute(null);

            OnPropertyChanged(nameof(Manager));
        }
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<Team> Teams { get; set; }
        
        public User Manager { get => APIClient.User; }
        public List<User> Leaders { get => Users.Where(U => U.Role1.RoleName == Role.TeamLeader).ToList(); }
        public List<dynamic> Sexes { get; internal set; } = new List<dynamic>();
        public List<Role> Roles { get; internal set; } = new List<Role>();

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
        public ICommand GetRoles { get; set; }
        public ICommand NewUser { get; set; }
        public ICommand AddUser { get; set; }
        public ICommand UpdateUser { get; set; }
        public ICommand DisableUserAccount { get; set; }
        public ICommand EnableUserAccount { get; set; }

        async TT.Task getUsers()
        {
            Users = JsonConvert.DeserializeObject<ObservableCollection<User>>(await MakeApiCall(APIClient.Action.GET, Controller.Users));
            
            foreach (var user in Users) user.Password = "Don't change";


            OnPropertyChanged(nameof(Users));
            OnPropertyChanged(nameof(Leaders));
        }
        async TT.Task getTeams()
        {
            Teams = JsonConvert.DeserializeObject<ObservableCollection<Team>>(await MakeApiCall(APIClient.Action.GET, Controller.Teams));
            OnPropertyChanged(nameof(Teams));
        }
        async TT.Task getRoles()
        {
            Roles = JsonConvert.DeserializeObject<List<Role>>(await MakeApiCall(APIClient.Action.GET, Controller.Roles));
            OnPropertyChanged(nameof(Roles));
        }

        void newUser()
        {
            SelectedUser = new User() { Manager = APIClient.User.UserID };
            Users.Add(SelectedUser);
        }
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
            if (string.IsNullOrEmpty(JSON))
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
