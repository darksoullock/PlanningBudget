using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace PlanningBudget.Windows.Views
{
    public class ExtendedGridView : GridView
    {
        public ExtendedGridView()
        {
            this.SelectionChanged += ExtendedGridView_SelectionChanged;
        }

        void ExtendedGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Contains(this.Items[0]))
            {
                this.SelectedIndex = -1;
            }
        }
    }
}
