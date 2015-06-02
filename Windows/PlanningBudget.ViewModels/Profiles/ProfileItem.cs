using GalaSoft.MvvmLight;
using PlanningBudget.Core;
using PlanningBudget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;

namespace PlanningBudget.ViewModels
{
    public class ProfileItem : ItemWithNameColorAndIconForBinding
    {
        public ProfileItem()
        {
        }

        public ProfileItem(Profile profile)
        {
            this.Id = profile.Id;
            this.name = profile.Name;
            this.icon = profile.Icon;
            this.iconColor = ColorTools.ColorFromArgb(profile.Color);
            this.IsAddIcon = false;
        }

        public bool IsAddTile { get; set; }

        public bool IsCurrent { get; set; }

        public int Id { get; set; }

        public Visibility AddTileVisibility
        {
            get { return (!IsAddIcon) ? Visibility.Collapsed : Visibility.Visible; }
        }

        public Visibility ProfileTileVisibility
        {
            get { return IsAddIcon ? Visibility.Collapsed : Visibility.Visible; }
        }
    }
}
