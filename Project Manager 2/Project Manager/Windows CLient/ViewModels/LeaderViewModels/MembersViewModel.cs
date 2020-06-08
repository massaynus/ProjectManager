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
using System.Windows.Controls;

namespace Windows_CLient.ViewModels
{
    public class MembersViewModel : BaseViewModel
    {
        private User selectedUser = new User();
        public MembersViewModel()
        {
            Users = new ObservableCollection<User>();
            TasksDoneByUser = new ObservableCollection<Task>();
            IssuesByUser = new ObservableCollection<Issue>();


            GetUsers = new RelayCommandAsync(getUsers);
            GetUsers.Execute(null);
        }


        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<Task> TasksDoneByUser { get; internal set; }
        public ObservableCollection<Issue> IssuesByUser { get; internal set; }

        public bool ShowTasksDoneShadow { get => TasksDoneByUser.Count >= 5; }
        public bool ShowIssuesShadow { get => IssuesByUser.Count >= 5; }

        public User SelectedUser
        {
            get => selectedUser;
            set
            {
                if (selectedUser != value)
                {
                    selectedUser = value;
                    OnPropertyChanged(nameof(selectedUser));

                    TasksDoneByUser.Clear();
                    foreach (var Task in selectedUser.Tasks)
                        if (Task.DoneBy == selectedUser.UserID)
                            if (Task.isComplete == true) TasksDoneByUser.Insert(0, Task);
                            else TasksDoneByUser.Add(Task);


                    IssuesByUser.Clear();
                    foreach (var issue in selectedUser.Issues)
                        if (issue.Issuer == selectedUser.UserID)
                            if (issue.isSolved == true) IssuesByUser.Insert(0, issue);
                            else IssuesByUser.Add(issue);

                    OnPropertyChanged(nameof(ShowIssuesShadow));
                    OnPropertyChanged(nameof(ShowTasksDoneShadow));
                }
            }
        }

        public ICommand GetUsers { get; set; }

        async TT.Task getUsers()
        {
            Users = JsonConvert.DeserializeObject<ObservableCollection<User>>(await MakeApiCall(APIClient.Action.GET, Controller.Users));
            OnPropertyChanged(nameof(Users));

            if (Users.Count > 0)  SelectedUser = Users[0];

        }
    }
}
