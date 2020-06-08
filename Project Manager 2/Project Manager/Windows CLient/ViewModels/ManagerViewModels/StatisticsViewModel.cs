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
using System.Windows;

namespace Windows_CLient.ViewModels
{
    public class StatisticsViewModel : BaseViewModel
    {
        public class ProjectCompletion
        {
            public Project Project { get; set; }
            public string ProjectName { get => Project.Name; }
            public int Issues { get; set; }
            public int TasksCompleted { get; set; }
            public int TasksRemaining { get; set; }
        }
        public class UserStats : IComparable
        {
            public User User { get; set; }
            public string FullName { get => User.FullName; }
            public int Score { get; set; }

            public int CompareTo(object obj)
            {
                var U = (UserStats)obj;
                if (U.Score == this.Score) return 0;
                else if (U.Score > this.Score) return 1;
                else return -1;
            }
        }
        public class VS
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }

        public StatisticsViewModel()
        {
            GetPaiments = new RelayCommandAsync(getPaiments);
            GetProjects = new RelayCommandAsync(getProjects);
            GetUsers = new RelayCommandAsync(getUsers);

            GetPaiments.Execute(null);
            GetProjects.Execute(null);
            GetUsers.Execute(null);

            IVC.Add(new VS { Name = "Issues", Value = 0 });
            IVC.Add(new VS { Name = "Completions", Value = 0 });
        }


        public List<ProjectCompletion> ProjectCompletions { get; set; } = new List<ProjectCompletion>(); // BARS
        public List<VS> IVC { get; set; } = new List<VS>();// Pie




        public List<Paiment> Paiments { get; set; } = new List<Paiment>(); 

        public List<Paiment> Incomes { get; set; } = new List<Paiment>(); // Area
        public List<Paiment> Expenses { get; set; } = new List<Paiment>(); // Same Area



        public List<UserStats> Top5 { get; set; } = new List<UserStats>(); // Bars


        public ICommand GetPaiments { get; set; }
        public ICommand GetProjects { get; set; }
        public ICommand GetUsers { get; set; }

        async TT.Task getPaiments()
        {
            Paiments = JsonConvert.DeserializeObject<List<Paiment>>(await MakeApiCall(APIClient.Action.GET, Controller.Paiment));

            foreach (var P in Paiments)
            {
                if (P.SenderFullName == APIClient.User.FullName) Expenses.Add(P);
                else if (P.RecieverFullName == APIClient.User.FullName) Incomes.Add(P);
            }

            OnPropertyChanged(nameof(Paiments));
        }

        async TT.Task getProjects()
        {
            var projects = JsonConvert.DeserializeObject<List<Project>>(await MakeApiCall(APIClient.Action.GET, Controller.Projects));

            foreach (var project in projects)
            {
                var ProjectStat = new ProjectCompletion()
                {
                    Project = project,
                    TasksCompleted = project.Tasks?.Count(T => T.isComplete == true) ?? 0,
                    TasksRemaining = project.Tasks?.Count(T => !(T.isComplete ?? false)) ?? 0
                };

                project.Tasks?.ToList().ForEach(T => ProjectStat.Issues += T.Issues?.Count ?? 0);

                IVC[0].Value += ProjectStat.Issues;
                IVC[1].Value += ProjectStat.TasksCompleted;

                ProjectCompletions.Add(ProjectStat);
            }

            OnPropertyChanged(nameof(ProjectCompletions));
        }

        async TT.Task getUsers()
        {
            var users = JsonConvert.DeserializeObject<List<User>>(await MakeApiCall(APIClient.Action.GET, Controller.Users));

            foreach (var user in users)
            {
                Top5.Add(new UserStats { User = user, Score = user.TasksCompletedCount * 20 + user.TasksBookedCount * 10 + user.IssuesCount * 5 });
            }

            Top5.Sort();
            OnPropertyChanged(nameof(Top5));
        }

    }
}
