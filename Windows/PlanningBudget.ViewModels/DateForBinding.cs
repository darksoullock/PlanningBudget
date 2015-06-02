using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningBudget.ViewModels
{
    public class DateForBinding : ObservableObject
    {
        public DateForBinding()
        {
            Days = new ObservableCollection<string>();
            Months = new ObservableCollection<string>();
            Years = new ObservableCollection<string>();

            FillMonth();

            int current = DateTime.Now.Year;
            for (int i = -1; i < 10; ++i)
            {
                Years.Add((current - i).ToString());
            }

            date = DateTime.Now;

            FillDays();
        }

        private void FillMonth()
        {
            Months.Add("January");
            Months.Add("February");
            Months.Add("March");
            Months.Add("April");
            Months.Add("May");
            Months.Add("June");
            Months.Add("July");
            Months.Add("August");
            Months.Add("September");
            Months.Add("October");
            Months.Add("November");
            Months.Add("December");
        }

        private void FillDays()
        {
            Days.Clear();
            int count = DateTime.DaysInMonth(date.Year, date.Month);
            for (int i = 1; i <= count; ++i)
            {
                Days.Add(i.ToString());
            }
        }

        private DateTime date;

        public string Day
        {
            get { return date.Day.ToString(); }
            set
            {
                if (Days.Contains(value))
                {
                    date = new DateTime(date.Year, date.Month, date.Day);
                }
            }
        }

        public ObservableCollection<string> Days { get; set; }

        public string Month
        {
            get { return Months[date.Month - 1]; }
            set
            {
                UpdateDate(date.Year, Months.IndexOf(value) + 1, date.Day);
            }
        }

        public ObservableCollection<string> Months { get; set; }

        public string Year
        {
            get { return date.Year.ToString(); }
            set
            {
                UpdateDate(int.Parse(value), date.Month, date.Day);
            }
        }

        public ObservableCollection<string> Years { get; set; }

        public DateTime Date
        {
            get { return date; }
        }

        private void UpdateDate(int day, int month, int year)
        {
            int oldday = day;

        }
    }
}
