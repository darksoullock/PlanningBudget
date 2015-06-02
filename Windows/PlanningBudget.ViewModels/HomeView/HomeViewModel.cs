using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using PlanningBudget.DataAccess;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using Windows.UI;
using Windows.UI.Xaml.Media;
using PlanningBudget.Models;
using PlanningBudget.Core;
using Windows.Storage;
using PlanningBudget.ViewModels;

namespace PlanningBudget.ViewModels.HomeView
{
    public class HomeViewModel : ViewModelBase
    {
        private INavigationService navigationService;

        private int selectedAccount = -1;

        private int selectedIncome = -1;

        private int selectedExpense = -1;

        public HomeViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            EditCategoryCommand = new RelayCommand(GoToEditCategory);
            AddTransactionCommand = new RelayCommand(GoToAddTransaction);
            DeleteCategoryCommand = new RelayCommand(DeleteCategory);
            GoToProfilesCommand = new RelayCommand(GoToProfiles);
            GoToTransactionsCommand = new RelayCommand<object>(GoToTransactions);
            AccounsClickCommand = new RelayCommand<object>(AccounsClick);
            ExpensesClickCommand = new RelayCommand<object>(ExpensesClick);
            IncomesClickCommand = new RelayCommand<object>(IncomesClick);

            Expenses = new ObservableCollection<ExpenseCategoryItem>();
            Incomes = new ObservableCollection<IncomeCategoryItem>();
            Accounts = new ObservableCollection<AccountCategoryItem>();

            LoadCategories();
            LoadProfiles();

            DataAccessProvider.AccountAdded += AccountAdded;
            DataAccessProvider.AccountChanged += AccountChanged;
            DataAccessProvider.ExpenseAdded += ExpenseAdded;
            DataAccessProvider.ExpenseChanged += ExpenseChanged;
            DataAccessProvider.ProfileAdded += ProfileAdded;
            DataAccessProvider.ProfileChanged += ProfileChanged;
            DataAccessProvider.ProfileDeleted += ProfileDeleted;
            DataAccessProvider.TransactionAdded += TransactionAdded;
            //      profiles added...
        }

        void TransactionAdded(object sender, Transaction e)
        {
            var c = Expenses.FirstOrDefault(i=>i.Id == e.CategoryId);
            if (c!=null)
            {
                c.Spent = (e.Amount+decimal.Parse(c.Spent)).ToString();
            }
        }

        public ObservableCollection<ExpenseCategoryItem> Expenses { get; set; }

        public ObservableCollection<IncomeCategoryItem> Incomes { get; set; }

        public ObservableCollection<AccountCategoryItem> Accounts { get; set; }

        public ObservableCollection<ProfileItem> Profiles { get; set; }

        public ProfileItem SelectedProfile { get; set; }

        public RelayCommand<object> ExpensesClickCommand { get; set; }

        public RelayCommand<object> IncomesClickCommand { get; set; }

        public RelayCommand DeleteCategoryCommand { get; set; }

        public RelayCommand GoToProfilesCommand { get; set; }

        public RelayCommand EditCategoryCommand { get; set; }

        public RelayCommand<object> AccounsClickCommand { get; set; }
        
        public bool isOpen { get { return true; } }

        public int SelectedAccount
        {
            get { return selectedAccount; }
            set { ListSelectetionChanged(out selectedAccount, value); }
        }

        public int SelectedIncome
        {
            get { return selectedIncome; }
            set { ListSelectetionChanged(out selectedIncome, value); }
        }

        public int SelectedExpense
        {
            get { return selectedExpense; }
            set { ListSelectetionChanged(out selectedExpense, value); }
        }

        public Visibility IsCategorySelected
        {
            get
            {
                return selectedIncome == -1 && selectedExpense == -1 && selectedAccount == -1 ?
                    Visibility.Collapsed : Visibility.Visible;
            }
        }

        public Visibility IsNotCategorySelected
        {
            get
            {
                return selectedIncome == -1 && selectedExpense == -1 && selectedAccount == -1 ?
                    Visibility.Visible : Visibility.Collapsed;
            }
        }

        public string PageHeader
        {
            get
            {
                return "Planning budget";
            }
        }


        void ProfileChanged(object sender, CategoryAddedEventArgs<Profile> e)
        {
            var p = Profiles.First(i => i.Id == e.NewCategory.Id);
            p.Name = e.NewCategory.Name;
            p.Icon = e.NewCategory.Icon;
            p.IconColor = ColorTools.ColorFromArgb(e.NewCategory.Color);
        }

        private void ProfileDeleted(object sender, CategoryAddedEventArgs<Profile> e)
        {
            Profiles.Remove(Profiles.FirstOrDefault(i => e.NewCategory.Name == i.Name));
        }

        private void ProfileAdded(object sender, CategoryAddedEventArgs<Profile> e)
        {
            Profiles.Add(new ProfileItem(e.NewCategory));
        }

        private void AccountAdded(object sender, CategoryAddedEventArgs<Account> e)
        {
            Accounts.Add(new AccountCategoryItem(e.NewCategory));
        }

        private void AccountChanged(object sender, CategoryAddedEventArgs<Account> e)
        {
            var c = Accounts.Where(i => i.Id == e.NewCategory.Id).First();
            if (c != null)
            {
                c.Name = e.NewCategory.Name;
                c.Balance = e.NewCategory.Balance.ToString();
                c.Icon = e.NewCategory.Icon;
                c.IconColor = ColorTools.ColorFromArgb(e.NewCategory.Color);
            }
        }

        private async void ExpenseAdded(object sender, CategoryAddedEventArgs<Expense> e)
        {
            Expenses.Add(new ExpenseCategoryItem(e.NewCategory, "0"));
        }

        private void ExpenseChanged(object sender, CategoryAddedEventArgs<Expense> e)
        {
            var c = Expenses.Where(i => i.Id == e.NewCategory.Id).First();
            if (c != null)
            {
                c.Name = e.NewCategory.Name;
                c.Budget = e.NewCategory.Budget.ToString();
                c.Icon = e.NewCategory.Icon;
                c.IconColor = ColorTools.ColorFromArgb(e.NewCategory.Color);
            }
        }

        private void ExpensesClick(object parameter)
        {
            if ((parameter as ExpenseCategoryItem).IsAddIcon)
            {
                navigationService.NavigateTo("AddExpenseView", string.Empty);
            }
            else
            {
                navigationService.NavigateTo("TransactionListView", parameter);
            }
        }

        private void IncomesClick(object parameter)
        {
            if ((parameter as IncomeCategoryItem).IsAddIcon)
            {
                navigationService.NavigateTo("AddIncomeView", string.Empty);
            }
            else
            {
                navigationService.NavigateTo("TransactionListView", parameter);
            }
        }

        private void AccounsClick(object parameter)
        {
            var choosed = ((parameter) as AccountCategoryItem);
            if (choosed.IsAddIcon)
            {
                navigationService.NavigateTo("AddAccountView", string.Empty);
            }
        }

        private void GoToProfiles()
        {
            navigationService.NavigateTo("ProfilesView");
        }

        private void GoToEditCategory()
        {
            if (selectedAccount > 0)
            {
                navigationService.NavigateTo("AddAccountView", SelectedCategoryName);
            }

            if (selectedExpense > 0)
            {
                navigationService.NavigateTo("AddExpenseView", SelectedCategoryName);
            }

            if (selectedIncome > 0)
            {
                navigationService.NavigateTo("AddIncomeView", SelectedCategoryName);
            }
        }

        public RelayCommand AddCategoryCommand { get; set; }

        private void GoToAddCategory()
        {
            navigationService.NavigateTo("AddCategoryView");
        }

        public RelayCommand AddTransactionCommand { get; set; }

        private void GoToAddTransaction()
        {
            navigationService.NavigateTo("AddTransactionView");
        }

        public RelayCommand<object> GoToTransactionsCommand { get; set; }

        private void GoToTransactions(object o)
        {
            navigationService.NavigateTo("TransactionListView", ((CategoryItem)o).Name);
        }

        private async void DeleteCategory()
        {
            var SelectedCategoryName = this.SelectedCategoryName;

            if (await Helpers.DeleteConfirmationMessage(SelectedCategoryName))
            {
                if (selectedAccount >= 0)
                {
                    Accounts.RemoveAt(selectedAccount);
                    await DataAccessProvider.DeleteCategoryByName<Account>(SelectedCategoryName);
                    selectedAccount = -1;
                    RaisePropertyChanged("SelectedAccount");
                }

                if (selectedIncome >= 0)
                {
                    Incomes.RemoveAt(selectedIncome);
                    await DataAccessProvider.DeleteCategoryByName<Income>(SelectedCategoryName);
                    selectedIncome = -1;
                    RaisePropertyChanged("SelectedIncome");
                }

                if (selectedExpense >= 0)
                {
                    Expenses.RemoveAt(selectedExpense);
                    await DataAccessProvider.DeleteCategoryByName<Expense>(SelectedCategoryName);
                    selectedExpense = -1;
                    RaisePropertyChanged("SelectedExpense");
                }

                RaisePropertyChanged("IsCategorySelected");
            }
        }

        private string SelectedCategoryName
        {
            get
            {
                if (selectedAccount > 0)
                {
                    return Accounts[selectedAccount].Name;
                }

                if (selectedIncome > 0)
                {
                    return Incomes[selectedIncome].Name;
                }

                if (selectedExpense > 0)
                {
                    return Expenses[selectedExpense].Name;
                }

                return null;
            }
        }

        protected void ListSelectetionChanged(out int selected, int value)
        {
            if (value != 0)
            {
                selectedAccount = -1;
                selectedIncome = -1;
                selectedExpense = -1;
                selected = value;
                RaisePropertyChanged("SelectedAccount");
                RaisePropertyChanged("SelectedIncome");
                RaisePropertyChanged("SelectedExpense");
                RaisePropertyChanged("IsCategorySelected");
                RaisePropertyChanged("IsNotCategorySelected");
            }
            else
            {
                selected = -1;
            }
        }

        private async void LoadProfiles()
        {
            Profiles = new ObservableCollection<ProfileItem>();
            var profiles = await DataAccessProvider.GetAll<Profile>();
            foreach (var i in profiles)
            {
                Profiles.Add(new ProfileItem(i));
            }

            foreach (var i in Profiles)
            {
                int id = DataAccessProvider.CurrentProfileId;
                if (i.Id == id)
                {
                    SelectedProfile = i;
                    RaisePropertyChanged("SelectedProfile");
                    break;
                }
            }

        }

        async void LoadCategories()
        {
            var expenses = (await DataAccessProvider.GetAll<Expense>()).Where(i => i.ParentId == -1);
            var incomes = await DataAccessProvider.GetAll<Income>();
            var accounts = await DataAccessProvider.GetAll<Account>();

            Expenses.Add(new ExpenseCategoryItem() { IsAddIcon = true });
            Incomes.Add(new IncomeCategoryItem() { IsAddIcon = true });
            Accounts.Add(new AccountCategoryItem() { IsAddIcon = true });

            var tr = await DataAccessProvider.GetAllTransactions();
            foreach (var i in expenses)
            {
                var total = tr.Where(j => j.CategoryId == i.Id).Select(j=>j.Amount).Sum();
                Expenses.Add(new ExpenseCategoryItem(i, total.ToString()));
            }

            foreach (var i in incomes)
            {
                Incomes.Add(new IncomeCategoryItem(i));
            }

            foreach (var i in accounts)
            {
                Accounts.Add(new AccountCategoryItem(i));
            }
        }
    }
}
