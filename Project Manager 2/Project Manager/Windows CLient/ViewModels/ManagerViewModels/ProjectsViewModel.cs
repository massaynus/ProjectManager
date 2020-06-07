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
using static Windows_CLient.APIClient;

namespace Windows_CLient.ViewModels
{
    public class ProjectsViewModel : BaseViewModel
    {
        private Project selectedProject = new Project();
        public ProjectsViewModel()
        {
            Teams = new List<Team>();
            Projects = new ObservableCollection<Project>();

            GetProjects = new RelayCommandAsync(getProjetcs);
            GetTeams = new RelayCommandAsync(getTeams);

            UpdateProject = new RelayCommandAsync(assigneProject);
            AddProject = new RelayCommandAsync(addProject);
            NewProject = new RelayCommand(newProject);

            GetProjects.Execute(null);
            if (Teams.Count == 0) GetTeams.Execute(null);
        }

        public ObservableCollection<Project> Projects { get; set; }
        public List<Team> Teams { get; set; }

        public Project SelectedProject 
        { 
            get => selectedProject;
            set
            {
                if (value != selectedProject)
                {
                    selectedProject = value;
                    OnPropertyChanged(nameof(SelectedProject));
                }
            }
        }

        public ICommand GetProjects { get; set; }
        public ICommand GetTeams { get; set; }
        public ICommand UpdateProject { get; set; }
        public ICommand AddProject { get; set; }
        public ICommand NewProject { get; set; }

        async TT.Task getTeams()
        {
            Teams = JsonConvert.DeserializeObject<List<Team>>(await MakeApiCall(APIClient.Action.GET, Controller.Teams));
            OnPropertyChanged(nameof(Teams));
        }
        async TT.Task getProjetcs()
        {
            var res = await APIClient.client.GetAsync("Projects");
            if (res.IsSuccessStatusCode)
            {
                Projects = JsonConvert.DeserializeObject<ObservableCollection<Project>>(await res.Content.ReadAsStringAsync());
                OnPropertyChanged(nameof(Projects));
                if (Projects.Count > 0)
                    SelectedProject = Projects[0];
            }
            else
            {
                MessageBox.Show($"Status Code: {res.StatusCode}\n{res.ReasonPhrase}", "Oops!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        async TT.Task assigneProject()
        {
            string content = await MakeApiCall(APIClient.Action.PUT, Controller.Projects, SelectedProject, SelectedProject.ProjectID.ToString());
            if (string.IsNullOrEmpty(content)) MessageBox.Show("Project was assigned successfuly!!!", "Great!", MessageBoxButton.OK, MessageBoxImage.Information);
            else MessageBox.Show($"Project wasn't assigned successfuly", "Oops!", MessageBoxButton.OK, MessageBoxImage.Warning);

        }
        async TT.Task addProject()
        {
            var content = APIClient.GetStringContent(SelectedProject);
            var res = await APIClient.client.PostAsync("Projects", content);
            if (res.IsSuccessStatusCode)
            {
                MessageBox.Show($"Project was assigned successfuly!!!", "Great!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Status Code: {res.StatusCode}\n{res.ReasonPhrase}", "Oops!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        void newProject()
        {
            SelectedProject = new Project() { Owner = APIClient.User.UserID };
            Projects.Add(SelectedProject);
        }
    }
}
