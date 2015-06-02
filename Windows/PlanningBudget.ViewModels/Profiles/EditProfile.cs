using PlanningBudget.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace PlanningBudget.ViewModels
{
    public class EditProfile: ItemWithNameColorAndIconForBinding
    {
        public EditProfile()
        {
            var iconColours = ColorTools.GetColorsList();
            this.IconColors = new ObservableCollection<Color>();
            foreach (var i in iconColours)
            {
                this.IconColors.Add(i);
            }

            this.Icons = DataAccess.DataAccessProvider.GetIcons();
        }

        public ObservableCollection<string> Icons { get; protected set; }

        public ObservableCollection<Color> IconColors { get; protected set; }
    }
}
