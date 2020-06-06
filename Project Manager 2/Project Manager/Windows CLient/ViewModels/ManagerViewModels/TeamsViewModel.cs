using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TT = System.Threading.Tasks;
using System.Windows.Input;
using Windows_CLient.Models;
using static Windows_CLient.APIClient;
using System.Windows;

namespace Windows_CLient.ViewModels
{
    public class TeamsViewModel : BaseViewModel
    {
        private Team selectedTeam = new Team();
        public TeamsViewModel()
        {
            Teams = new ObservableCollection<Team>();

            GetTeams = new RelayCommandAsync(getTeams);
            AddTeam = new RelayCommandAsync(addTeams);
            UpdateTeam = new RelayCommandAsync(updateTeams);
            DisassembleTeam = new RelayCommandAsync(disassembleTeam);
            NewTeam = new RelayCommand(newTeam);

            GetTeams.Execute(null);
        }
        public ObservableCollection<Team> Teams { get; set; }
        public Team SelectedTeam
        {
            get => selectedTeam;
            set
            {
                if (value != selectedTeam)
                {
                    selectedTeam = value;
                    OnPropertyChanged(nameof(SelectedTeam));
                }
            }
        }

        public ICommand GetTeams { get; set; }
        public ICommand AddTeam { get; set; }
        public ICommand UpdateTeam { get; set; }
        public ICommand DisassembleTeam { get; set; }
        public ICommand NewTeam { get; set; }


        async TT.Task getTeams()
        {
            Teams = JsonConvert.DeserializeObject<ObservableCollection<Team>>(await MakeApiCall(APIClient.Action.GET, Controller.Teams));
            OnPropertyChanged(nameof(Teams));
        }
        async TT.Task addTeams()
        {
            string JSON = await MakeApiCall(APIClient.Action.POST, Controller.Teams, SelectedTeam);
            if (JSON.Contains("TeamID"))
            {
                SelectedTeam = JsonConvert.DeserializeObject<Team>(JSON);
                OnPropertyChanged(nameof(SelectedTeam));
                MessageBox.Show("Team Added successfuly!!", "Great!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Team wasn't added", "Oops!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        async TT.Task updateTeams()
        {
            string JSON = await MakeApiCall(APIClient.Action.PUT, Controller.Teams, SelectedTeam, SelectedTeam.TeamID.ToString());
            if (JSON.Contains("TeamID"))
            {
                SelectedTeam = JsonConvert.DeserializeObject<Team>(JSON);
                OnPropertyChanged(nameof(SelectedTeam));
                MessageBox.Show("Team Updated successfuly!!", "Great!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Team wasn't updated", "Oops!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        async TT.Task disassembleTeam()
        {
            string Names = string.Empty;
            
            var Users = JsonConvert.DeserializeObject<List<User>>(await MakeApiCall(APIClient.Action.GET, Controller.Users));
            Users = Users.Where(U => U.Team == SelectedTeam.TeamID).ToList();
            Users.ForEach(U => Names += $"{U.LastName}\t{U.FirstName}\n");
            if (MessageBox.Show("Would really like to make these people teamless?\n" + Names,
                                "Are you sure about that?",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                int affectedCount = 0;
                foreach (var user in Users)
                {
                    user.Team = null;
                    affectedCount += (await MakeApiCall(APIClient.Action.PUT, Controller.Users, user, user.UserID.ToString())).Contains("UserID") ? 1 : 0;
                }
                if (affectedCount == Users.Count) MessageBox.Show("Every one was kicked out!!", "Great! >_<", MessageBoxButton.OK, MessageBoxImage.Information);
                else MessageBox.Show("Something went wrong with kicking some people!!", "Oops!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else MessageBox.Show("Opeation canceled.", "Ok");
        }
        void newTeam() => SelectedTeam = new Team();


    }
}
