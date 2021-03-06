﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TT = System.Threading.Tasks;
using System.Windows.Input;
using Windows_CLient.Models;
using Newtonsoft.Json;

using static Windows_CLient.APIClient;
using System.Configuration;
using System.Windows;

namespace Windows_CLient.ViewModels
{
    public class PaiementsViewModel : BaseViewModel
    {
        private Paiment selectedPaiement = new Paiment();

        public PaiementsViewModel()
        {
            Paiments = new ObservableCollection<Paiment>();
            Projects = new ObservableCollection<Project>();

            GetPaiements = new RelayCommandAsync(getPaiments);
            GetProjects = new RelayCommandAsync(getProjects);
            MakePaiement = new RelayCommandAsync(makePaiment);
            PayWorkers = new RelayCommandAsync(payWorkers);
            NewPaiment = new RelayCommand(newPaiment);

            GetPaiements.Execute(null);

            if (Projects.Count == 0) GetProjects.Execute(null);
        }

        public ObservableCollection<Paiment> Paiments { get; set; }
        public ObservableCollection<Project> Projects { get; set; }
        public Paiment SelectedPaiment
        {
            get => selectedPaiement;
            set
            {
                if (selectedPaiement != value)
                {
                    selectedPaiement = value;
                    OnPropertyChanged(nameof(SelectedPaiment));
                }
            }
        }

        public ICommand GetPaiements { get; set; }
        public ICommand GetProjects { get; set; }
        public ICommand NewPaiment { get; set; }
        public ICommand MakePaiement { get; set; }
        public ICommand PayWorkers { get; set; }

        async TT.Task getPaiments()
        {
            Paiments = JsonConvert.DeserializeObject<ObservableCollection<Paiment>>(await MakeApiCall(APIClient.Action.GET, Controller.Paiment));
            OnPropertyChanged(nameof(Paiments));
        }

        async TT.Task getProjects()
        {
            Projects = JsonConvert.DeserializeObject<ObservableCollection<Project>>(await MakeApiCall(APIClient.Action.GET, Controller.Projects));
            OnPropertyChanged(nameof(Projects));
        }

        async TT.Task makePaiment()
        {
            string res = await MakeApiCall(APIClient.Action.POST, Controller.Paiment, SelectedPaiment);
            if (res.Contains("PaymentID")) MessageBox.Show("Paiement was made succesflully!!!\n😎😎😎", "Great!!", MessageBoxButton.OK, MessageBoxImage.Information);
            else MessageBox.Show("Your paiment didn't go through", "Oops!", MessageBoxButton.OK, MessageBoxImage.Warning);

            GetPaiements.Execute(null);

        }
        async TT.Task payWorkers()
        {
            if (MessageBox.Show("Are you sure you want to pay everyone today?", "Huh?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                List<User> users = JsonConvert.DeserializeObject<List<User>>(await MakeApiCall(APIClient.Action.GET, Controller.Users));
                int count = users.Count, paied = 0;
                foreach (User user in users)
                {
                    string res = await MakeApiCall(APIClient.Action.POST, Controller.Paiment,
                        new Paiment()
                        {
                            Amount = decimal.Parse(ConfigurationManager.AppSettings[user.Role1.RoleName + "Salary"]),
                            Date = DateTime.Today,
                            isSalary = true,
                            SenderID = APIClient.User.UserID,
                            RecieverID = user.UserID,
                            SenderFullName = APIClient.User.LastName + " " + APIClient.User.FirstName,
                            RecieverFullName = user.LastName + " " + user.FirstName,
                            PaymentDescription = "Salary"
                        });
                    if (res.Contains("PaymentID"))
                        paied++;
                }

                if (count == paied) MessageBox.Show("Operation was successful!", "Great!", MessageBoxButton.OK, MessageBoxImage.Information);
                else MessageBox.Show("Some paiements didn't go through", "Oops!", MessageBoxButton.OK, MessageBoxImage.Warning);

                if (MessageBox.Show("Would you like to pay yourself too?", "Huh?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    string res = await MakeApiCall(APIClient.Action.POST, Controller.Paiment,
                        new Paiment()
                        {
                            Amount = decimal.Parse(ConfigurationManager.AppSettings[APIClient.User.Role1.RoleName + "Salary"]),
                            Date = DateTime.Today,
                            isSalary = true,
                            SenderID = APIClient.User.UserID,
                            RecieverID = APIClient.User.UserID,
                            SenderFullName = APIClient.User.LastName + " " + APIClient.User.FirstName,
                            RecieverFullName = APIClient.User.LastName + " " + APIClient.User.FirstName,
                            PaymentDescription = "Salary to yourself"
                        });
                    if (res.Contains("PaymentID")) MessageBox.Show("Operation was successful!", "Great!", MessageBoxButton.OK, MessageBoxImage.Information);
                    else MessageBox.Show("Your paiement didn't go through", "Oops!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else MessageBox.Show("Operation was canceled!", "Okay!", MessageBoxButton.OK, MessageBoxImage.Information);

            GetPaiements.Execute(null);
        }
        void newPaiment()
        {
            SelectedPaiment = new Paiment() { SenderFullName = APIClient.User.FullName, SenderID = APIClient.User.UserID };
            Paiments.Add(SelectedPaiment);
        }

    }
}
