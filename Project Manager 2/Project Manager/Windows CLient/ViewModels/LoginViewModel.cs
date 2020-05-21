using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using TT = System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Windows_CLient.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Timers;
using Windows_CLient.Views;

namespace Windows_CLient.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private Login window;
        private Window child;

        public LoginViewModel()
        {
            UserName = string.Empty;

            Auth = new RelayCommandAsync(AuthHelper);
            Exit = new RelayCommand(() => window.Close());

            if (DateTime.Now.Hour < 11 && DateTime.Now.Hour > 8)
                StatusMessage = "Good Morning";
            else if (DateTime.Now.Hour < 18 && DateTime.Now.Hour > 11)
                StatusMessage = "Good Evening";
            else
                StatusMessage = "Good Night";
        }
        public LoginViewModel(Login window) : this() { this.window = window; }

        public string StatusMessage { get; set; } // Welcome Message
        public string ErrorMessage { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get => window.txtPassword.Password; }
        public Visibility PasswordVisibility { 
            get
            {
                if (UserPassword.Length > 0) return Visibility.Hidden;
                else return Visibility.Visible;
            } 
        }

        public ICommand Auth { get; set; }
        public ICommand Exit { get; set; }
        public Window Child 
        { 
            get => child;
            set
            {
                child = value;
                if (value != null)
                {
                    child.Closed += delegate { window.Show(); };
                }
            }
        }

        public async TT.Task AuthHelper()
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(UserPassword))
            {
                ErrorMessage = "Please provide a username and a password";
                OnPropertyChanged(nameof(ErrorMessage));
            }
            else
            {
                using (StringContent cords = APIClient.GetStringContent(new { username = UserName, password = UserPassword }))
                {
                    string url = APIClient.API_HOST + "Auth";
                    var response = await APIClient.client.PostAsync(url, cords);

                    if (response.IsSuccessStatusCode)
                    {
                        APIClient.User = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync(),
                            new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

                        string base64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(UserName + ":" + UserPassword));
                        APIClient.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64);

                        StatusMessage = "Hi Mr." + APIClient.User.LastName;
                        OnPropertyChanged(nameof(APIClient.User));

                        if (Child != null) Child.Close();

                        switch (APIClient.User.Role1.RoleName)
                        {
                            case "Manager":
                                Child = new Views.ManagerViews.ManagerMainView();
                                Child.Title = $"Managers Area - Mr.{APIClient.User.LastName} {APIClient.User.FirstName}";
                                Child.Show();
                                break;

                            case "TeamLeader":
                                Child = new Views.LeaderViews.LeaderMainView();
                                Child.Title = $"Team Leaders Area - Mr.{APIClient.User.LastName} {APIClient.User.FirstName}";
                                Child.Show();
                                break;

                            case "Member":
                                Child = new Views.TasksView();
                                Child.Title = $"Team Members Area - Mr.{APIClient.User.LastName} {APIClient.User.FirstName}";
                                Child.Show();
                                break;

                            case "Client":
                                MessageBox.Show("You are a client, how did you get here??",
                                    "-__-",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Question);

                                ErrorMessage = "Please Try another account";
                                UserName = string.Empty;
                                window.txtPassword.Clear();

                                OnPropertyChanged(nameof(UserName));
                                OnPropertyChanged(nameof(UserPassword));
                                break;

                            default:
                                MessageBox.Show("This User has an unsopported role, please call the developer ASAP!",
                                    "Warning",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);

                                ErrorMessage = "Please Try another account";
                                UserName = string.Empty;
                                window.txtPassword.Clear();

                                OnPropertyChanged(nameof(UserName));
                                OnPropertyChanged(nameof(UserPassword));
                                break;
                        }

                        window.Hide();
                        UserName = ErrorMessage = string.Empty;
                        OnPropertyChanged(nameof(UserName));
                        OnPropertyChanged(nameof(ErrorMessage));
                        window.txtPassword.Clear();
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        ErrorMessage = "Your User name or password is incorrect";
                    }
                    else
                    {
                        throw new Exception("Something Went Wrong!!\n" + response.ReasonPhrase + "\nStatus Code:\t" + response.StatusCode.ToString());
                    }
                }

                OnPropertyChanged(nameof(StatusMessage));
                OnPropertyChanged(nameof(ErrorMessage)); 
            }
        }
    }
}
