using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using PlanningBudget.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace PlanningBudget.ViewModels
{
    public abstract class CategoryViewModel : ViewModelBase
    {
        protected string incomeTypeName = "Income";

        protected string expenseTypeName = "Expense";

        protected INavigationService navigationService;

        public CategoryViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.Types = new ObservableCollection<string>() { expenseTypeName, incomeTypeName };
            var iconColours = ColorTools.GetColorsList();
            this.IconColours = new ObservableCollection<Color>();
            foreach (var  i in iconColours)
            {
                this.IconColours.Add(i);
            }

            this.Icons = DataAccess.DataAccessProvider.GetIcons();
            GoBackCommand = new RelayCommand(GoBack);
            SaveAndReturnCommand = new RelayCommand(SaveAndReturn);
        }

        public string Name { get; set; }

        public string Type { get; set; }    // string (SelectedItem) or int (SelectedIndex) ?

        public ObservableCollection<string> Types { get; protected set; }

        public Color IconColor { get; set; }

        public ObservableCollection<Color> IconColours { get; protected set; }

        public string Icon { get; set; }

        public ObservableCollection<string> Icons { get; protected set; }

        public RelayCommand GoBackCommand { get; set; }

        public RelayCommand SaveAndReturnCommand { get; set; }

        protected void SaveAndReturn()
        {
            if (Save())
            {
                GoBack();
            }
        }

        protected abstract bool Save();

        protected bool CheckValues()
        {
            bool b = true;
            b = b && (Name != null && Name != string.Empty);
            b = b && (Type != null && Type != string.Empty);
            b = b && (Icon != null && Icon != string.Empty);
            b = b && (IconColor != null);
            return b;
        }

        protected void ClearFields()
        {
            Name = null;
            Type = null;
            Icon = null;
            RaisePropertyChanged("Name");
            RaisePropertyChanged("Type");
            RaisePropertyChanged("Icon");
            RaisePropertyChanged("IconColor");
        }

        protected void GoBack()
        {
            navigationService.GoBack();
        }
    }
}
