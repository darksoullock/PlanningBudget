using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using PlanningBudget.DataAccess;
using PlanningBudget.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace PlanningBudget.ViewModels
{
    public class AddTransactionViewModel : ViewModelBase
    {
        struct id
        {
            public int Id;
            public bool IsIncome;
        }

        Dictionary<string, id> ids = new Dictionary<string, id>();

        INavigationService navigationService;

        public AddTransactionViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            GoBackCommand = new RelayCommand(GoBack);
            SaveAndContinueCommand = new RelayCommand(SaveAndContinue);
            SaveAndReturnCommand = new RelayCommand(SaveAndReturn);
            Date = DateTime.Now;
            LoadCategories();
            LoadAccounts();
        }

        private async void SaveAndReturn()
        {
            if (await Save())
            {
                GoBack();
            }
        }

        private async void SaveAndContinue()
        {
            Save();
        }

        async Task<bool> Save()
        {
            if (CheckData())
            {
                var t = ids[SelectedCategory];
                DataAccessProvider.AddTransaction(
                    new Transaction()
                    {
                        IsIncome = t.IsIncome,
                        CategoryId = t.Id,
                        Amount = decimal.Parse(Amount),
                        AccountId = ids[SelectedAccount].Id,
                        Date = this.Date,
                        Name = this.Name
                    });

                ClearFields();
                return true;
            }
            else
            {
                await new MessageDialog("All fields should be filled. Amount accepts only numbers").ShowAsync();
            }

            return false;
        }

        private void ClearFields()
        {
            Amount = string.Empty;
            Name = string.Empty;
            SelectedAccount = string.Empty;
            SelectedCategory = string.Empty;
            RaisePropertyChanged("Amount");
            RaisePropertyChanged("Name");
        }

        private bool CheckData()
        {
            if (SelectedAccount==null||SelectedCategory==null)
            {
                return false;
            }

            decimal t;
            return decimal.TryParse(Amount, out t);
        }

        async void LoadCategories()
        {
            Categories = new List<string>();
            List<Expense> expenses = await DataAccessProvider.GetAll<Expense>();
            foreach (var i in expenses)
            {
                Categories.Add(i.Name);
                ids.Add(i.Name, new id() { Id = i.Id, IsIncome = false });
            }

            List<Income> incomes = await DataAccessProvider.GetAll<Income>();
            foreach (var i in incomes)
            {
                Categories.Add(i.Name);
                ids.Add(i.Name, new id() { Id = i.Id, IsIncome = true });
            }

            RaisePropertyChanged("Categories");

            //DataAccessProvider.CategoryAdded += CategoryAdded; ;
        }

        private async void LoadAccounts()
        {
            Accounts = new List<string>();
            List<Account> accounts = await DataAccessProvider.GetAll<Account>();
            foreach (var i in accounts)
            {
                Accounts.Add(i.Name);
                ids.Add(i.Name, new id() { Id = i.Id, IsIncome = false });
            }

            RaisePropertyChanged("Accounts");

        }

        //void CategoryAdded(object sender, Core.CategoryAddedEventArgs e)
        //{
        //    Categories.Add(e.NewCategory.Name);
        //    RaisePropertyChanged("Categories");
        //}

        public List<string> Categories { get; set; }

        public List<string> Accounts { get; set; }

        public string SelectedCategory { get; set; }

        public string SelectedAccount { get; set; }

        public string Name { get; set; }

        public string Amount { get; set; }

        public DateTime Date { get; set; }

        public RelayCommand GoBackCommand { get; set; }

        public RelayCommand SaveAndReturnCommand { get; set; }

        public RelayCommand SaveAndContinueCommand { get; set; }

        private void GoBack()
        {
            navigationService.GoBack();
        }
    }
}
