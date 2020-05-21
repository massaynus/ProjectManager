using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows_CLient.Models;

namespace Windows_CLient.ViewModels
{
    public class SchedulingViewModel : BaseViewModel
    {
        //TODO : Complete the SchedulingViewModel
        //TODO : Write functions for actions and ctor

        #region Task Properties
        public Project Project { get; set; }
        public int ProjectID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public int Difficulty { get; set; }
        public DateTime DeadLine { get; set; }
        public Stack Stack { get; set; } 
        public int StackID { get; set; } 
        #endregion

        public ICommand SubmitTask { get; set; }
        public ICommand Cancel { get; set; }
    }
}
