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
        private Issue _selectedIssue;
        public IssueTreatingViewModel()
        {
            MarkAsSolved = new RelayCommandAsync(markAsSolved);
            GetIssueInfo = new RelayCommandAsync(getIssueInfo);
            GetIssues = new RelayCommandAsync(getIssues);

            GetIssues.Execute(null);
            GetIssueInfo.Execute(null);
        }

        public Issue SelectedIssue
        { 
            get => _selectedIssue;
            set
            {
                if (value != _selectedIssue)
                {
                    _selectedIssue = value;
                    OnPropertyChanged(nameof(SelectedIssue));
                    GetIssueInfo.Execute(null);
                }
            } 
        }
        public Task IssueTask { get => (SelectedIssue is null) ? null : SelectedIssue.IssueTask; }
        public User Issuer { get; set; }

        public ObservableCollection<Issue> Issues { get; set; } = new ObservableCollection<Issue>();
        public ICommand MarkAsSolved { get; set; }
        public ICommand GetIssues { get; set; }
        public ICommand GetIssueInfo { get; set; }
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
                if (Issues.Count > 0)
                {
                    SelectedIssue = Issues[0];
                }
                OnPropertyChanged(nameof(Issues));
            }
            else
            {
                MessageBox.Show($"Status Code:  {res.StatusCode}\n{res.ReasonPhrase}", "Oops", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public async TT.Task getIssueInfo()
        {
            if (SelectedIssue is null) return;

            if (SelectedIssue.IssueTask is null)
            {
                var res = await APIClient.client.GetAsync($"Tasks/{SelectedIssue.Task}");
                if (res.IsSuccessStatusCode)
                {
                    SelectedIssue.IssueTask = JsonConvert.DeserializeObject<Task>(await res.Content.ReadAsStringAsync());
                }
                else
                {
                    MessageBox.Show($"Status code: {res.StatusCode}\n{res.ReasonPhrase}", "Oops!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            if (SelectedIssue.IssuerObj is null)
            {
                var res = await APIClient.client.GetAsync($"Users/{SelectedIssue.Issuer}");
                if (res.IsSuccessStatusCode)
                {
                    SelectedIssue.IssuerObj = JsonConvert.DeserializeObject<User>(await res.Content.ReadAsStringAsync());
                }
                else
                {
                    MessageBox.Show($"Status code: {res.StatusCode}\n{res.ReasonPhrase}", "Oops!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            OnPropertyChanged(nameof(SelectedIssue));
        }
    }
}
