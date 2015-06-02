using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningBudget.ViewModels
{
    public class AddIncomeViewModel : AddCategoryViewModel
    {
        public AddIncomeViewModel(INavigationService navigationService)
            :base(navigationService)
        {
            Date = new DateForBinding();
        }

        public DateForBinding Date { get; set; }

        protected override Task<bool> Save()
        {
            throw new NotImplementedException();
        }

        protected override bool CheckValues()
        {
            return base.CheckValues();
        }

        public override void Activate(object categoryNameParameter)
        {
            base.Activate(categoryNameParameter);
        }

        protected override void ClearFields()
        {
            base.ClearFields();
        }
    }
}
