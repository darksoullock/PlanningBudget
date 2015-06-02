using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using PlanningBudget.Core;
using PlanningBudget.DataAccess;
using PlanningBudget.Models;
using PlanningBudget.ViewModels.HomeView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace PlanningBudget.ViewModels
{
    public class TransactionListViewModel : ViewModelBase, INavigable
    {
        protected INavigationService navigationService;

        bool income;

        public TransactionListViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            GoBackCommand = new RelayCommand(GoBack);
            TransactionClickCommand = new RelayCommand<object>(AddTransaction);
            Transactions = new ObservableCollection<TransactionItem>();
            Transactions.Add(new TransactionItem(){IsAddIcon=true});
        }

        public ObservableCollection<TransactionItem> Transactions { get; protected set; }

        public RelayCommand GoBackCommand { get; set; }

        public RelayCommand<object> TransactionClickCommand { get; set; }

        public async void Activate(object parameter)
        {
            Transactions.Clear();
            Transactions.Add(new TransactionItem() { IsAddIcon = true });

            income = parameter is IncomeCategoryItem;
            int id = (parameter as CategoryItem).Id;

            var tr = await DataAccessProvider.GetAllTransactions();
            tr = tr.Where(i => i.CategoryId == id).ToList();

            foreach (var i in tr)
            {
                Transactions.Add(new TransactionItem() { Amount = i.Amount.ToString(), Name = i.Name, Date = i.Date});
            }
            
        }

        private void AddTransaction(object obj)
        {
            if ((obj as TransactionItem).IsAddIcon)
            {
                navigationService.NavigateTo("AddTransactionView");
            }
        }

        protected void GoBack()
        {
            navigationService.GoBack();
        }
    }
}
