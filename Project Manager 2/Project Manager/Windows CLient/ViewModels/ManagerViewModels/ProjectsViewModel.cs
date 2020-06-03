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
    public class ProjectsViewModel : BaseViewModel
    {
        private Project selectedProject = new Project();
        public ProjectsViewModel()
        {
            Projects = new ObservableCollection<Project>();

            GetProjects = new RelayCommandAsync(getProjetcs);
            AssigneProject = new RelayCommandAsync(assigneProject);
            AddProject = new RelayCommandAsync(addProject);
            NewProject = new RelayCommand(() => SelectedProject = new Project());

            GetProjects.Execute(null);
        }

        public ObservableCollection<Project> Projects { get; set; }
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
        public ICommand AssigneProject { get; set; }
        public ICommand AddProject { get; set; }
        public ICommand NewProject { get; set; }


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
            var content = APIClient.GetStringContent(SelectedProject);
            var res = await APIClient.client.PutAsync("Projects", content);
            if (res.IsSuccessStatusCode)
            {
                MessageBox.Show($"Project was assigned successfuly!!!", "Great!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Status Code: {res.StatusCode}\n{res.ReasonPhrase}", "Oops!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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
    }
}
