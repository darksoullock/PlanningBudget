using GalaSoft.MvvmLight;
using Windows.UI;

namespace PlanningBudget.ViewModels
{
    public abstract class ItemWithNameColorAndIconForBinding : ObservableObject
    {
        protected string name;

        protected Color iconColor;

        protected string icon;

        public bool IsAddIcon { get; set; }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged();
            }
        }

        public Color IconColor
        {
            get { return iconColor; }
            set
            {
                iconColor = value;
                RaisePropertyChanged("IconColor");
            }
        }

        public string Icon
        {
            get { return icon; }
            set
            {
                icon = value;
                RaisePropertyChanged("Icon");
            }
        }
    }
}
