using GalaSoft.MvvmLight;
using PlanningBudget.Core;
using PlanningBudget.Models;
using Windows.UI;

namespace PlanningBudget.ViewModels.HomeView
{
    public class IncomeCategoryItem : CategoryItem
    {
        protected string value;

        public string Value
        {
            get { return this.value; }
            set
            {
                this.value = value;
                RaisePropertyChanged("Value");
            }
        }

        public IncomeCategoryItem()
            :base()
        {
        }

        public IncomeCategoryItem(Income category)
            :base(category.Name, category.Color, category.Icon, category.Id)
        {
            Value = "1000";
        }

        public override string Percentage
        {
            get
            {
                return "1";
            }
        }
    }
}
