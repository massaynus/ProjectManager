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
using System.Windows.Controls;
using Windows_CLient.Views.LeaderViews;
using System.Timers;
using System.Configuration;

namespace Windows_CLient.ViewModels
{
    public class LeaderMainViewModel : BaseViewModel
    {
        //TODO : Complete the LeaderMainViewModel 
        //TODO : Write functions for commands
        private Timer timer;
        public LeaderMainViewModel()
        {
            GotoIssues = new RelayCommand(gotoissues);
            GotoMembers = new RelayCommand(gotomembers);
            GotoStats = new RelayCommand(gotostats);
            GotoScheduling = new RelayCommand(gotoscheduling);

            GetProjects = new RelayCommandAsync(GetProjectsAsync);
            GetIssues = new RelayCommandAsync(GetIssuesAsync);

            Projects = new ObservableCollection<Project>(APIClient.User.Team1.Projects);
            if (Projects.Count == 0) GetProjects.Execute(null);

            Issues = new ObservableCollection<Issue>();

            foreach (var project in Projects)
                foreach (var task in project.Tasks)
                    foreach (var issue in task.Issues)
                        Issues.Add(issue);

            OnPropertyChanged(nameof(Issues));

            timer = new Timer(double.Parse(ConfigurationManager.AppSettings["RefreshingRate"]));
            timer.Elapsed += delegate
            {
                GetProjects.Execute(null);
                GetIssues.Execute(null);
            };
        }

        #region ViewModel

        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<Issue> Issues { get; set; }

        public int UnsolvedIssuesCount { get => Issues.Where(I => !(I.isSolved ?? false)).Count(); }

        public ICommand GetProjects { get; set; }
        public ICommand GetIssues { get; set; }

        public async TT.Task GetProjectsAsync()
        {
            timer.Stop();
            var res = await APIClient.client.GetAsync(APIClient.API_HOST + "Projects");
            if (res.IsSuccessStatusCode)
            {
                Projects = JsonConvert.DeserializeObject<ObservableCollection<Project>>(await res.Content.ReadAsStringAsync());
                OnPropertyChanged(nameof(Projects));
            }
            else
            {
                MessageBox.Show($"Status Code: {res.StatusCode}\nError:\n{await res.Content.ReadAsStringAsync()}", "Oops!",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            timer.Start();
        }
        public async TT.Task GetIssuesAsync()
        {
            timer.Stop();
            var res = await APIClient.client.GetAsync(APIClient.API_HOST + "Issues");
            if (res.IsSuccessStatusCode)
            {
                Issues = JsonConvert.DeserializeObject<ObservableCollection<Issue>>(await res.Content.ReadAsStringAsync());
                OnPropertyChanged(nameof(Issues));
            }
            else
            {
                MessageBox.Show($"Status Code: {res.StatusCode}\nError:\n{await res.Content.ReadAsStringAsync()}", "Oops!",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            timer.Start();
        }

        #endregion

        #region Navigation

        public Page CurrentPage { get; set; } = new LeaderStatsPage();
        public ICommand GotoStats { get; set; }
        public ICommand GotoScheduling { get; set; }
        public ICommand GotoIssues { get; set; }
        public ICommand GotoMembers { get; set; }

        #region Nav Helpers
        void gotostats()
        {
            CurrentPage = new LeaderStatsPage();
            OnPropertyChanged(nameof(CurrentPage));
        }
        void gotoscheduling()
        {
            
            var page = new TaskSchedulingPage();
            var context = ((SchedulingViewModel)page.DataContext);
            context.Projects = Projects;
            context.OnPropertyChanged(nameof(context.Projects));
            context.TaskSent += delegate
             {
                 GetProjects.Execute(null);
             };

            CurrentPage = page;
            OnPropertyChanged(nameof(CurrentPage));
        }

        void gotoissues()
        {
            CurrentPage = new IssuesTreatingPage();
            OnPropertyChanged(nameof(CurrentPage));
        }

        void gotomembers()
        {
            CurrentPage = new TeamMembersPage();
            OnPropertyChanged(nameof(CurrentPage));
        } 
        #endregion

        #endregion
    }
}
