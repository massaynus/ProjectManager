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
    public class LeaderMainViewModel : BaseViewModel
    {
        //TODO : Complete the LeaderMainViewModel 
        //TODO : Write functions for commands

        public Project SelectedProject { get; set; }
        public Issue SelectedIssue { get; set; }
        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<Issue> Issues { get; set; }
        public List<(Project, double)> Progresses { get; set; }


        public ICommand GetProjects { get; set; }
        public ICommand GetIssues { get; set; }
        public ICommand CalculateProgresses { get; set; }
    }
}
