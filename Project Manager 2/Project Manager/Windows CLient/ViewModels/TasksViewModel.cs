﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WCM = Windows_CLient.Models;

namespace Windows_CLient.ViewModels
{
    public class TasksViewModel : BaseViewModel
    {
        private ObservableCollection<WCM.Task> tasks;
        public TasksViewModel()
        {
            Tasks = new ObservableCollection<WCM.Task>();
            GetTasks = new RelayCommand(getTasks);
            BookTask = new RelayCommand(bookTask);
            CompleteTask = new RelayCommand(completeTask);
        }

        public ObservableCollection<WCM.Task> Tasks 
        {
            get
            {
                if (tasks.Count == 0) GetTasks.Execute(null);
                return tasks;
            }
            set => tasks = value;
        }
        public WCM.Task SelectedTask { get; set; }
        
        public ICommand GetTasks { get; set; }
        public ICommand BookTask { get; set; }
        public ICommand CompleteTask { get; set; }


        public async void getTasks()
        {
            var response = await APIClient.client.GetAsync(APIClient.API_HOST + "Tasks", System.Net.Http.HttpCompletionOption.ResponseContentRead);
            if (response.IsSuccessStatusCode)
            {
                string RsJson = await response.Content.ReadAsStringAsync();
                Tasks = new ObservableCollection<WCM.Task>(JsonConvert.DeserializeObject<List<WCM.Task>>(RsJson));
            }
            OnPropertyChanged(nameof(Tasks));
        }

        public async void bookTask()
        {
            
        }

        public async void completeTask()
        {

        }
    }
}