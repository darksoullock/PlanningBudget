/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:PlanningBudget.Windows"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using PlanningBudget.ViewModels;
using PlanningBudget.ViewModels.HomeView;

namespace PlanningBudget.Windows.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<AddTransactionViewModel>();
            SimpleIoc.Default.Register<EditCategoryViewModel>();
            SimpleIoc.Default.Register<TransactionListViewModel>();
            SimpleIoc.Default.Register<ProfileViewModel>();
            SimpleIoc.Default.Register<AddAccountViewModel>();
            SimpleIoc.Default.Register<AddExpenseViewModel>();
            SimpleIoc.Default.Register<AddIncomeViewModel>();
        }

        public HomeViewModel Home
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HomeViewModel>();
            }
        }

        public AddTransactionViewModel AddTransaction
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddTransactionViewModel>();
            }
        }

        public EditCategoryViewModel EditCategory
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EditCategoryViewModel>();
            }
        }

        public TransactionListViewModel TransactionList
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TransactionListViewModel>();
            }
        }

        public ProfileViewModel AddProfile
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ProfileViewModel>();
            }
        }

        public AddAccountViewModel AddAccount
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddAccountViewModel>();
            }
        }

        public AddExpenseViewModel AddExpense
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddExpenseViewModel>();
            }
        }

        public AddIncomeViewModel AddIncome
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddIncomeViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}