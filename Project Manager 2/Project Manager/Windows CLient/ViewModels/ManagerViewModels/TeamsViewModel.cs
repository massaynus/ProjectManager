using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows_CLient.Models;

namespace Windows_CLient.ViewModels
{
    public class TeamsViewModel : BaseViewModel
    {
        //TODO: complete the TeamsViewModel
        //TODO: write commands
        public ObservableCollection<Team> Teams { get; set; }
        public Team SelectedTeam { get; set; }

        public ICommand GetTeams { get; set; }
        public ICommand AddTeam { get; set; }
        public ICommand UpdateTeam { get; set; }
        public ICommand GetTeams { get; set; }
    }
}
