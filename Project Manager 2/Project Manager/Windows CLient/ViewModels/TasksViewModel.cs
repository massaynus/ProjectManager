using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Windows_CLient.Views;
using WCM = Windows_CLient.Models;

namespace Windows_CLient.ViewModels
{
    public class TasksViewModel : BaseViewModel
    {
        private Timer refreshingTimer;
        private WCM.Task selectedTask;
        private ObservableCollection<WCM.Task> tasks;
        private bool? NoTasksExist;
        public TasksViewModel()
        {
            FlagTask = new RelayCommand(flagTask);
            GetTasks = new RelayCommand(getTasks);
            BookTask = new RelayCommand(bookTask);
            CompleteTask = new RelayCommand(completeTask);

            GetTasks.Execute(null);

            Tasks = new ObservableCollection<WCM.Task>();
            SelectedTask = Tasks.Count > 0 ? Tasks[0] : new WCM.Task();

            refreshingTimer = new Timer(double.Parse(ConfigurationManager.AppSettings["RefreshingRate"]));
            refreshingTimer.Elapsed += delegate 
            { 
                GetTasks.Execute(null); 
                
            };
            refreshingTimer.Start();
        }

        public string LastRefresh { get; set; }
        public ObservableCollection<WCM.Task> Tasks
        {
            get => tasks;
            set
            {
                tasks = value;
                if (tasks.Count > 0) SelectedTask = tasks[0];
            }
        }
        public WCM.Task SelectedTask
        {
            get
            {
                return selectedTask;
            }
            set
            {
                if (selectedTask != value && value != null)
                {
                    selectedTask = value;
                    
                    OnPropertyChanged(nameof(SelectedTask));
                    
                    OnPropertyChanged(nameof(CanBook));
                    OnPropertyChanged(nameof(CanComplete));
                }
            }
        }

        public ICommand GetTasks { get; set; }
        public ICommand BookTask { get; set; }
        public ICommand CompleteTask { get; set; }
        public ICommand FlagTask { get; set; }

        public bool CanComplete { get => (SelectedTask.isBooked ?? false) && SelectedTask.DoneBy == APIClient.User.UserID; }
        public bool CanBook { get => SelectedTask.isBooked != true; }


        public async void getTasks()
        {
            var response = await APIClient.client.GetAsync(APIClient.API_HOST + "Tasks", System.Net.Http.HttpCompletionOption.ResponseContentRead);
            if (response.IsSuccessStatusCode)
            {
                string RsJson = await response.Content.ReadAsStringAsync();
                Tasks = new ObservableCollection<WCM.Task>(JsonConvert.DeserializeObject<List<WCM.Task>>(RsJson));
                NoTasksExist = Tasks.Count == 0;

                LastRefresh = DateTime.Now.ToShortTimeString();
                OnPropertyChanged(nameof(LastRefresh));
            }
            OnPropertyChanged(nameof(Tasks));
        }

        /// <summary>
        /// Marks the selected task as booked for the current user
        /// </summary>
        public async void bookTask()
        {
            var response = await APIClient.client.PutAsync(APIClient.API_HOST + $"Tasks/BookTask/{SelectedTask.TaskID}", null);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("The task was booked successfully", "update", MessageBoxButton.OK, MessageBoxImage.Information);

                GetTasks.Execute(null);
                OnPropertyChanged(nameof(tasks));
            }
            else
            {
                MessageBox.Show(await response.Content.ReadAsStringAsync(), "update", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
      
        /// <summary>
        /// Marks the selected task as completed
        /// </summary>
        public async void completeTask()
        {
            var response = await APIClient.client.PutAsync(APIClient.API_HOST + $"Tasks/CompleteTask/{SelectedTask.TaskID}", null);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("The task was marked as complete successfully", "update", MessageBoxButton.OK, MessageBoxImage.Information);

                GetTasks.Execute(null);
                OnPropertyChanged(nameof(tasks));
            }
            else
            {
                MessageBox.Show(await response.Content.ReadAsStringAsync(), "update", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Open a window that lets you flag a task
        /// </summary>
        public void flagTask()
        {
            refreshingTimer.Stop();

            Views.FlagTask flag = new FlagTask();
            var context = new FlagTasksViewModel();

            context.Issue.isSolved = false;
            context.Issue.Issuer = APIClient.User.UserID;
            context.Issue.Task = SelectedTask.TaskID;
            context.Self = flag;

            flag.DataContext = context;
            flag.ShowDialog();

            refreshingTimer.Start();
        }
        
    }
}
