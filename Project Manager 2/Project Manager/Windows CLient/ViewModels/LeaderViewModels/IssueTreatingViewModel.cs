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
    public class IssueTreatingViewModel : BaseViewModel
    {
        //TODO : Complete the IssueTreatingViewModel
        //TODO : Write functions for commands


        public Project SelectedProject { get; set; }
        public Issue SelectedIssue { get; set; }
        public ObservableCollection<Issue> Issues { get; set; }
        public ICommand MarkAsSolved { get; set; }
    }
}
