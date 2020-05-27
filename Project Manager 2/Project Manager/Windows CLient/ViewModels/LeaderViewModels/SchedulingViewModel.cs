using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TT = System.Threading.Tasks;
using System.Windows.Input;
using Windows_CLient.Models;
using System.Windows;
using Newtonsoft.Json;

namespace Windows_CLient.ViewModels
{
    public class SchedulingViewModel : BaseViewModel
    {
        private Project _project = new Project();
        private Task _task = new Task();

        public event EventHandler TaskSent;
        public SchedulingViewModel()
        {
            TaskSent += delegate { OnPropertyChanged(nameof(Task)); };

            SubmitTask = new RelayCommandAsync(submitTask);
            TT.Task.Run(GetStacks).Wait();

            NewTask = new RelayCommand(() =>
            {
                Task = new Task();
                Task.Project = Project.ProjectID;
                OnPropertyChanged(nameof(Task));

                IsNewTask = true;
                OnPropertyChanged(nameof(IsNewTask));
            });
            ModifytTask = new RelayCommandAsync(modifyTask);
            DeleteTask = new RelayCommandAsync(deleteTask);

            NewTask.Execute(null);
        }

        #region Task Properties
        public bool IsNewTask { get; set; } = false;
        public Project Project
        {
            get => _project;
            set
            {
                if (_project != value)
                {
                    _project = value;
                    Task.Project = value.ProjectID;
                    OnPropertyChanged(nameof(Project));
                }
            }
        }
        public Task Task
        {
            get => _task;
            set
            {
                if (value != _task)
                {
                    IsNewTask = false;
                    _task = value;
                    OnPropertyChanged(nameof(Task));
                    OnPropertyChanged(nameof(IsNewTask));
                }
            }
        }
        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<Stack> Stacks { get; set; }
        #endregion

        public ICommand SubmitTask { get; set; }
        public ICommand NewTask { get; set; }
        public ICommand ModifytTask { get; set; }
        public ICommand DeleteTask { get; set; }

        public async TT.Task submitTask()
        {
            var stringContent = APIClient.GetStringContent(Task);
            var res = await APIClient.client.PostAsync("Tasks", stringContent);
            if (res.IsSuccessStatusCode)
            {
                TaskSent(this, new EventArgs());
                MessageBox.Show("Task Added Successfully", "Great!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Status Code: {res.StatusCode}\n{await res.Content.ReadAsStringAsync()}", "Oops", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public async TT.Task modifyTask()
        {
            Task.isBooked = false;
            Task.isComplete = false;

            var stringContent = APIClient.GetStringContent(Task);
            var res = await APIClient.client.PutAsync("Tasks/" + Task.TaskID, stringContent);
            if (res.IsSuccessStatusCode)
            {
                TaskSent(this, new EventArgs());
                MessageBox.Show("Task Updated Successfully", "Great!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Status Code: {res.StatusCode}\n{await res.Content.ReadAsStringAsync()}", "Oops", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public async TT.Task deleteTask()
        {
            var res = await APIClient.client.DeleteAsync("Tasks/" + Task.TaskID);
            TaskSent(this, new EventArgs());

            if (res.IsSuccessStatusCode)
            {
                TaskSent(this, new EventArgs());
                MessageBox.Show("Task Removed Successfully", "Great!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Status Code: {res.StatusCode}\n{await res.Content.ReadAsStringAsync()}", "Oops", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public async TT.Task GetStacks()
        {
            var res = await APIClient.client.GetAsync("Stacks");
            if (res.IsSuccessStatusCode)
            {
                Stacks = JsonConvert.DeserializeObject<ObservableCollection<Stack>>(await res.Content.ReadAsStringAsync());
                OnPropertyChanged(nameof(Stacks));
            }
            else
            {
                MessageBox.Show($"Status Code: {res.StatusCode}\n{await res.Content.ReadAsStringAsync()}", "Oops", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
