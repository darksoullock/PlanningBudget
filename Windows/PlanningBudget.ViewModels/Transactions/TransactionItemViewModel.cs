using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningBudget.ViewModels
{
    public class TransactionItemViewModel : ViewModelBase
    {
        protected string name;

        protected string date;

        protected string description;

        protected string budget;

        protected string spent;

        public string Name
        {
            get { return name; }
            set
            {
                this.name = value;
                RaisePropertyChanged("Name");
            }
        }

        public string Date
        {
            get { return date; }
            set
            {
                this.date = value;
                RaisePropertyChanged("Date");
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                this.description = value;
                RaisePropertyChanged("Description");
            }
        }

        public string Budget
        {
            get { return budget; }
            set
            {
                this.budget = value;
                RaisePropertyChanged("Budget");
            }
        }

        public string Spent
        {
            get { return spent; }
            set
            {
                this.spent = value;
                RaisePropertyChanged("Spent");
            }
        }

        public string Saved
        {
            get
            {
                return (decimal.Parse(budget) - decimal.Parse(spent)).ToString();
            }
        }
    }
}
