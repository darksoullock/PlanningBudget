using PlanningBudget.Core;
using PlanningBudget.Models;
using System;
using Windows.UI;

namespace PlanningBudget.ViewModels.HomeView
{
    public class ExpenseCategoryItem : CategoryItem
    {
        protected string spent;

        protected string budgeted;

        public string Spent
        {
            get { return this.spent; }
            set
            {
                this.spent = value;
                RaisePropertyChanged("Spent");
                RaisePropertyChanged("Percentage");
            }
        }

        public string Budget
        {
            get { return this.budgeted; }
            set
            {
                this.budgeted = value;
                RaisePropertyChanged("Budgeted");
                RaisePropertyChanged("Percentage");
            }
        }

        public override string Percentage
        {
            get
            {
                float budgeted = 0;
                float.TryParse(Budget, out budgeted);
                if (budgeted == 0)
                {
                    return "0";
                }

                var spent = float.Parse(Spent);
                return Math.Min(1.0f, spent / budgeted).ToString();
            }
        }

        public ExpenseCategoryItem()
            : base()
        {
        }

        public ExpenseCategoryItem(Expense category, string spent)
            : base(category.Name, category.Color, category.Icon, category.Id)
        {
            Budget = category.Budget.ToString();
            Spent = spent;
        }
    }
}
