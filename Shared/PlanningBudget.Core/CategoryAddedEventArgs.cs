using PlanningBudget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningBudget.Core
{
    public class CategoryAddedEventArgs<T> : EventArgs where T : ItemWithNameColorAndIcon
    {
        private T category;


        public CategoryAddedEventArgs(T category)
        {
            this.category = category;
        }

        public T NewCategory { get { return category; } }
    }
}
