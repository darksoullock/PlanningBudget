using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using PlanningBudget.Core;
using PlanningBudget.DataAccess;
using PlanningBudget.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace PlanningBudget.ViewModels
{
    public class AddAccountViewModel : AddCategoryViewModel
    {
        protected Account editedAccount;

        public AddAccountViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }

        public string Balance { get; set; }

        protected override async Task<bool> Save()
        {
            if (!CheckValues())
            {
                await new MessageDialog("All fields should be filled. Balance accepts only numbers").ShowAsync();
                return false;
            }

            bool r;

            if (Mode == EditMode.Add)
            {
                r = await DataAccessProvider.AddCategory<Account>(new Account()
                    {
                        Name = this.Name,
                        Balance = Decimal.Parse(this.Balance),
                        Icon = this.Icon,
                        Color = ColorTools.ColorToArgb(this.IconColor),
                        ProfileId = this.Profile.Id
                    });
            }
            else
            {
                string t = editedAccount.Name;
                editedAccount.Name = this.Name;
                editedAccount.Balance = Decimal.Parse(this.Balance);
                editedAccount.ProfileId = this.Profile.Id;
                editedAccount.Icon = this.Icon;
                editedAccount.Color = ColorTools.ColorToArgb(this.IconColor);

                r = await DataAccessProvider.UpdateCategory<Account>(editedAccount);
            }

            if (!r)
            {
                await new MessageDialog("Account with that name already exists").ShowAsync();
            }

            return r;
        }

        protected override bool CheckValues()
        {
            bool b = base.CheckValues();
            decimal t;
            b = b && Decimal.TryParse(Balance, out t);
            return b;
        }

        protected override void ClearFields()
        {
            base.ClearFields();
            Balance = null;
            RaisePropertyChanged("Balance");
        }

        public async override void Activate(object accountNameParameter)
        {
            base.Activate(accountNameParameter);

            if (Mode == EditMode.Edit)
            {
                this.editedAccount = await DataAccessProvider.GetCategoryByName<Account>(accountNameParameter as string);


                this.Name = editedAccount.Name;
                this.Balance = editedAccount.Balance.ToString();
                this.Profile = Profiles.Where(i => i.Id == editedAccount.ProfileId).First();

                this.Icon = editedAccount.Icon;
                if (!Icons.Contains(editedAccount.Icon))
                {
                    Icons.Add(editedAccount.Icon);
                }

                this.IconColor = ColorTools.ColorFromArgb(editedAccount.Color);
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
    }
}
