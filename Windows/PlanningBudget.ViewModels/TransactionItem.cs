using GalaSoft.MvvmLight;
using PlanningBudget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PlanningBudget.ViewModels
{
    public class TransactionItem : ObservableObject
    {
        public TransactionItem()
        {
        }

        public TransactionItem(Transaction transaction)
        {
            this.Id = transaction.Id;
            this.name = transaction.Name;
            this.Date = transaction.Date;
            this.amount = transaction.Amount;
            this.IsAddIcon = false;
        }

        protected string name;

        protected decimal amount;

        public int Id { get; set; }

        public bool IsAddIcon { get; set; }

        public string Amount 
        {
            get { return "$" + amount; }
            set
            {
                amount = decimal.Parse(value);
                RaisePropertyChanged();
            }
        }

        public DateTime Date { get; set; }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged();
            }
        }

        public Visibility AddTileVisibility
        {
            get { return (!IsAddIcon) ? Visibility.Collapsed : Visibility.Visible; }
        }

        public Visibility TransactionTileVisibility
        {
            get { return IsAddIcon ? Visibility.Collapsed : Visibility.Visible; }
        }
    }
}
