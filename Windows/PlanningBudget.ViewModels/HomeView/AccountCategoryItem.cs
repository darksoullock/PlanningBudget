using PlanningBudget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningBudget.ViewModels.HomeView
{
    public class AccountCategoryItem : CategoryItem
    {
        protected string balance;

        public string Balance
        {
            get 
            { 
                return this.balance; 
            }
            set
            {
                this.balance = value;
                RaisePropertyChanged();
            }
        }

        public override string Percentage
        {
            get { return "1"; }
        }

        public AccountCategoryItem()
        {
        }

        public AccountCategoryItem(Account category)
            :base(category.Name, category.Color, category.Icon, category.Id)
        {
            this.balance = category.Balance.ToString();
        }
    }
}
