using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TT = System.Threading.Tasks;
using System.Windows.Input;
using Windows_CLient.Models;
using System.Windows;
using Newtonsoft.Json;

namespace Windows_CLient.ViewModels
{
    public class IssueTreatingViewModel : BaseViewModel
    {
        //TODO: make it get the Issueer and task info
        public IssueTreatingViewModel()
        {
            MarkAsSolved = new RelayCommandAsync(markAsSolved);
            GetIssues = new RelayCommandAsync(getIssues);
            GetIssues.Execute(null);
        }

        public Issue SelectedIssue { get; set; }
        public ObservableCollection<Issue> Issues { get; set; }
        public ICommand MarkAsSolved { get; set; }
        public ICommand GetIssues { get; set; }
        public async TT.Task markAsSolved()
        {
            SelectedIssue.isSolved = true;
            var StrContent = APIClient.GetStringContent(SelectedIssue);

            var res = await APIClient.client.PutAsync($"Issues/{SelectedIssue.IssueID}", StrContent);
            if (res.IsSuccessStatusCode)
            {
                MessageBox.Show("Marked successfully", "Great!", MessageBoxButton.OK, MessageBoxImage.Information);
                GetIssues.Execute(null);
            }
            else
            {
                MessageBox.Show($"Status Code:  {res.StatusCode}\n{res.ReasonPhrase}", "Oops", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public async TT.Task getIssues()
        {
            var res = await APIClient.client.GetAsync("Issues");
            if (res.IsSuccessStatusCode)
            {
                Issues = JsonConvert.DeserializeObject<ObservableCollection<Issue>>(await res.Content.ReadAsStringAsync());
                OnPropertyChanged(nameof(Issues));
                if (Issues.Count > 0)
                {
                    SelectedIssue = Issues[0];
                }
            }
            else
            {
                MessageBox.Show($"Status Code:  {res.StatusCode}\n{res.ReasonPhrase}", "Oops", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
