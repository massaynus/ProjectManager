using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TT = System.Threading.Tasks;
using System.Windows.Input;
using Windows_CLient.Models;

namespace Windows_CLient.ViewModels
{
    public class MembersViewModel : BaseViewModel
    {
        //TODO : Complete the MembersViewModel
        //TODO : Write functions for commands


        public ObservableCollection<User> Users { get; set; }
        public List<Task> TasksDoneByUser { get; set; }
        public List<Issue> IssuesByUser { get; set; }

        public User SelectedUser { get; set; }
        public Task SelectedTask { get; set; }
        public Issue SelectedIssues { get; set; }

        /// <summary>
        /// Done/Flagged
        /// </summary>
        public List<(User, int, int)> UserStats { get; set; }

        public ICommand GetUsers { get; set; }
        public ICommand GetUserStats { get; set; }
        public ICommand UpdateUser { get; set; }
    }
}
