using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TT = System.Threading.Tasks;
using System.Windows.Input;
using Windows_CLient.Models;
using Newtonsoft.Json;
using System.Windows;

namespace Windows_CLient.ViewModels
{
    public class LeaderMainViewModel : BaseViewModel
    {
        //TODO : Complete the LeaderMainViewModel 
        //TODO : Write functions for commands

        public LeaderMainViewModel()
        {
            GetProjects = new RelayCommandAsync(GetProjectsAsync);
            GetIssues = new RelayCommandAsync(GetIssuesAsync);
            CalculateProgresses = new RelayCommandAsync(CalculateProgressesAsync);

            Projects = new ObservableCollection<Project>(APIClient.User.Team1.Projects);
            
            Issues = new ObservableCollection<Issue>();
            
            foreach (var project in Projects)
                foreach (var task in project.Tasks)
                    foreach (var issue in task.Issues)
                        Issues.Add(issue);
            
            OnPropertyChanged(nameof(Issues));
        }

        public Project SelectedProject { get; set; }
        public Issue SelectedIssue { get; set; }
        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<Issue> Issues { get ; set; }
        public List<(Project, double)> Progresses { get; set; }


        public ICommand GetProjects { get; set; }
        public ICommand GetIssues { get; set; }
        public ICommand CalculateProgresses { get; set; }

        public async TT.Task GetProjectsAsync()
        {
            var res = await APIClient.client.GetAsync(APIClient.API_HOST + "Projects");
            if (res.IsSuccessStatusCode)
            {
                Projects = JsonConvert.DeserializeObject<ObservableCollection<Project>>(await res.Content.ReadAsStringAsync());
                OnPropertyChanged(nameof(Projects));

                if (Projects.Count > 0)
                {
                    SelectedProject = Projects[0];
                    OnPropertyChanged(nameof(SelectedProject));
                }
            }
            else
            {
                MessageBox.Show($"Status Code: {res.StatusCode}\nError:\n{await res.Content.ReadAsStringAsync()}", "Oops!",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public async TT.Task GetIssuesAsync()
        {
            var res = await APIClient.client.GetAsync(APIClient.API_HOST + "Issues");
            if (res.IsSuccessStatusCode)
            {
                Issues = JsonConvert.DeserializeObject<ObservableCollection<Issue>>(await res.Content.ReadAsStringAsync());
                OnPropertyChanged(nameof(Issues));

                if (Issues.Count > 0)
                {
                    SelectedIssue = Issues[0];
                    OnPropertyChanged(nameof(SelectedIssue));
                }
            }
            else
            {
                MessageBox.Show($"Status Code: {res.StatusCode}\nError:\n{await res.Content.ReadAsStringAsync()}", "Oops!",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public async TT.Task CalculateProgressesAsync()
        {
            //todo: calculate progresses
            await TT.Task.Delay(500);
        }
    }
}
