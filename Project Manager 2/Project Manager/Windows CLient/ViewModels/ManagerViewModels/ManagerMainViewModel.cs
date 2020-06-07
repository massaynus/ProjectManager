using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Windows_CLient.Models;
using Windows_CLient.Views.ManagerViews.Pages;

namespace Windows_CLient.ViewModels
{
    public class ManagerMainViewModel : BaseViewModel
    {
        private StatisticsPage statisticsPage;
        private PaiementsPage paiementsPage;
        private TeamsPage teamsPage;
        private ProjectsPage projectsPage;
        private WorkersPage workersPage;

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
            if (statisticsPage is null) statisticsPage = new StatisticsPage();
            CurrentPage = statisticsPage;
            OnPropertyChanged(nameof(CurrentPage));
        }
        public void gotoProjects()
        {
            if (projectsPage is null) projectsPage = new ProjectsPage();
            CurrentPage = projectsPage;
            if (teamsPage != null) ((ProjectsViewModel)projectsPage.DataContext).Teams = ((TeamsViewModel)teamsPage.DataContext).Teams.ToList();

            OnPropertyChanged(nameof(CurrentPage));
        }
        public void gotoPaiements()
        {
            if (paiementsPage is null) paiementsPage = new PaiementsPage();
            CurrentPage = paiementsPage;
            OnPropertyChanged(nameof(CurrentPage));
        }
        public void gotoTeams()
        {
            if (teamsPage is null) teamsPage = new TeamsPage();
            CurrentPage = teamsPage;
            OnPropertyChanged(nameof(CurrentPage));
        }
        public void gotoWorkers()
        {
            if (workersPage is null) workersPage = new WorkersPage();
            CurrentPage = workersPage;
            OnPropertyChanged(nameof(CurrentPage));
        }


    }
}
