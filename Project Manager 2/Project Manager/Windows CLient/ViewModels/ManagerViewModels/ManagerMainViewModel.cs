using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Windows_CLient.Views.ManagerViews.Pages;

namespace Windows_CLient.ViewModels
{
    public class ManagerMainViewModel : BaseViewModel
    {
        public ManagerMainViewModel()
        {
            GotoStatisitcs = new RelayCommand(gotoStatisitcs);
            GotoProjects = new RelayCommand(gotoProjects);
            GotoPaiements = new RelayCommand(gotoPaiements);
            GotoTeams = new RelayCommand(gotoTeams);
            GotoWorkers = new RelayCommand(gotoWorkers);
        }
        public Page CurrentPage { get; set; }

        public ICommand GotoStatisitcs { get; set; }
        public ICommand GotoProjects { get; set; }
        public ICommand GotoPaiements { get; set; }
        public ICommand GotoTeams { get; set; }
        public ICommand GotoWorkers { get; set; }

        public void gotoStatisitcs()
        {
            CurrentPage = new StatisticsPage();
            OnPropertyChanged(nameof(CurrentPage));
        }
        public void gotoProjects()
        {
            CurrentPage = new ProjectsPage();
            OnPropertyChanged(nameof(CurrentPage));
        }
        public void gotoPaiements()
        {
            CurrentPage = new PaiementsPage();
            OnPropertyChanged(nameof(CurrentPage));
        }
        public void gotoTeams()
        {
            CurrentPage = new TeamsPage();
            OnPropertyChanged(nameof(CurrentPage));
        }
        public void gotoWorkers()
        {
            CurrentPage = new WorkersPage();
            OnPropertyChanged(nameof(CurrentPage));
        }


    }
}
