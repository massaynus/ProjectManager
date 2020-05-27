using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Windows_CLient.Models;
using TT = System.Threading.Tasks;

namespace Windows_CLient.ViewModels
{
    public class LeaderStatsViewModel : BaseViewModel
    {
        public List<(Project, double)> Progresses { get; set; }

        public ICommand CalculateProgresses { get; set; }
        public async TT.Task CalculateProgressesAsync()
        {
            //TODO: calculate progresses
            await TT.Task.Delay(500);
        }
    }
}
