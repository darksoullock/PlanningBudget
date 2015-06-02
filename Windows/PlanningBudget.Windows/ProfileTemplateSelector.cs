using PlanningBudget.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PlanningBudget.Windows
{
    class ProfileTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AddIcon { get; set; }

        public DataTemplate Profile { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is string)
                return AddIcon;
            else if (item is ProfileItem)
                return Profile;

            return base.SelectTemplateCore(item, container);
        }

    }
}
