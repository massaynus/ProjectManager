using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using TT = System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Windows_CLient.Models;

namespace Windows_CLient.ViewModels
{
    public class FlagTasksViewModel : BaseViewModel
    {
        public FlagTasksViewModel()
        {
            Issue = new Issue();
            Close = new RelayCommand(() => Self.Close());
            FlagTask = new RelayCommandAsync(flagTask);
            FlagMode = true;
        }
        public bool FlagMode { get; set; }
        public Window Self { get; set; }
        public Issue Issue { get; set; }
        public string Title { get => Issue.Title; set => Issue.Title = value; }
        public string Description { get => Issue.Description; set => Issue.Description = value; }

        public ICommand FlagTask { get; set; }
        public ICommand Close { get; set; }

        private async TT.Task flagTask()
        {
            using (StringContent content = APIClient.GetStringContent(Issue))
            {
                var response = await APIClient.client.PostAsync(APIClient.API_HOST + "Issues", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("The task was flagged successfully", "update", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(await response.Content.ReadAsStringAsync(), "update", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            Self.Close();
        }

    }
}
