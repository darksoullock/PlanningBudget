using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using PlanningBudget.Core;
using PlanningBudget.DataAccess;
using PlanningBudget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace PlanningBudget.ViewModels
{
    public class AddExpenseViewModel : AddCategoryViewModel
    {
        private Expense editedExpense;

        public AddExpenseViewModel(INavigationService navigationService)
            :base(navigationService)
        {
            this.navigationService = navigationService;
            GoBackCommand = new RelayCommand(navigationService.GoBack);
        }

        public string Budget { get; set; }

        public override async void Activate(object parameter)
        {
            base.Activate(parameter);

            if (Mode == EditMode.Edit)
            {
                this.editedExpense = await DataAccessProvider.GetCategoryByName<Expense>(parameter as string);


                this.Name = editedExpense.Name;
                this.Budget = editedExpense.Budget.ToString();
                this.Profile = Profiles.Where(i => i.Id == editedExpense.ProfileId).First();

                this.Icon = editedExpense.Icon;
                if (!Icons.Contains(editedExpense.Icon))
                {
                    Icons.Add(editedExpense.Icon);
                }

                this.IconColor = ColorTools.ColorFromArgb(editedExpense.Color);
                if (!IconColors.Contains(IconColor))
                {
                    IconColors.Add(IconColor);
                }

                RaisePropertyChanged("Name");
                RaisePropertyChanged("Balance");
                RaisePropertyChanged("Profile");
                RaisePropertyChanged("Icon");
                RaisePropertyChanged("IconColor");
            }
        }

        protected override async Task<bool> Save()
        {
            if (!CheckValues())
            {
                await new MessageDialog("All fields should be filled. Budget accepts only numbers").ShowAsync();
                return false;
            }

            bool r;

            if (Mode == EditMode.Add)
            {
                r = await DataAccessProvider.AddCategory<Expense>(new Expense()
                    {
                        Name = this.Name,
                        Budget = Decimal.Parse(this.Budget),
                        Icon = this.Icon,
                        Color = ColorTools.ColorToArgb(this.IconColor),
                        ProfileId = this.Profile.Id
                    });
            }
            else
            {
                string t = editedExpense.Name;
                editedExpense.Name = this.Name;
                editedExpense.Budget = Decimal.Parse(this.Budget);
                editedExpense.ProfileId = this.Profile.Id;
                editedExpense.Icon = this.Icon;
                editedExpense.Color = ColorTools.ColorToArgb(this.IconColor);

                r = await DataAccessProvider.UpdateCategory<Expense>(editedExpense);
            }

            if (!r)
            {
                await new MessageDialog("Expense with that name already exists").ShowAsync();
            }

            return r;
        }

        protected override bool CheckValues()
        {
            bool b = base.CheckValues();
            decimal t;
            b = b && Decimal.TryParse(Budget, out t);
            return b;
        }

        protected override void ClearFields()
        {
            base.ClearFields();
            this.Budget = "";
            RaisePropertyChanged("Budget");
        }
    }
}
