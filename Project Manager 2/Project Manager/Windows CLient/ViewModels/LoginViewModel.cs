using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Windows_CLient.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace Windows_CLient.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            Auth = new RelayCommand(AuthHelper);

            if (DateTime.Now.Hour < 11 && DateTime.Now.Hour > 8)
                StatusMessage = "Good Morning";
            else if (DateTime.Now.Hour < 18)
                StatusMessage = "Good Evening";
            else
                StatusMessage = "Good Night";

        }
        public LoginViewModel(Window window) : this() { this.window = window; }

        private Window window;
        private Window child;

        public string StatusMessage { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }

        public ICommand Auth { get; set; }

        public async void AuthHelper()
        {
            using (StringContent cords = new StringContent(JsonConvert.SerializeObject(new { username = UserName, password = UserPassword }),
                                                    Encoding.UTF8, "application/json"))
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

                    window.Hide();
                    if (child != null) child.Close();

                    switch (APIClient.User.Role1.RoleName)
                    {
                        case "Manager":
                            child = new Views.ManagerViews.ManagerMainView();
                            child.Title = $"Managers Area - Mr.{APIClient.User.LastName} {APIClient.User.FirstName}";
                            child.ShowDialog();
                            break;

                        case "TeamLeader":
                            child = new Views.LeaderViews.LeaderMainView();
                            child.Title = $"Team Leaders Area - Mr.{APIClient.User.LastName} {APIClient.User.FirstName}";
                            child.ShowDialog();
                            break;

                        case "Member":
                            child = new Views.TasksView();
                            child.Title = $"Team Members Area - Mr.{APIClient.User.LastName} {APIClient.User.FirstName}";
                            child.ShowDialog();
                            break;

                        case "Client":
                            MessageBox.Show("You are a client, how did you get here??",
                                "-__-",
                                MessageBoxButton.OK,
                                MessageBoxImage.Question);

                            StatusMessage = "Please Try another account";
                            UserName = UserPassword = string.Empty;

                            OnPropertyChanged(nameof(UserName));
                            OnPropertyChanged(nameof(UserPassword));
                            break;

                        default: 
                            MessageBox.Show("This User has an unsopported role, please call the developer ASAP!",
                                "Warning",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);

                            StatusMessage = "Please Try another account";
                            UserName = UserPassword = string.Empty;
                            
                            OnPropertyChanged(nameof(UserName));
                            OnPropertyChanged(nameof(UserPassword));
                            break;
                    }

                    window.Close();

                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    StatusMessage = "Your User name or password is incorrect";
                }
                else
                {
                    throw new Exception("Something Went Wrong!!\n" + response.ReasonPhrase + "\nStatus Code:\t" + response.StatusCode.ToString());
                }
            }

            OnPropertyChanged(nameof(StatusMessage));
        }
    }
}
