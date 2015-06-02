using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using PlanningBudget.Core;
using PlanningBudget.DataAccess;
using PlanningBudget.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace PlanningBudget.ViewModels
{
    public abstract class AddCategoryViewModel : ViewModelBase, INavigable
    {
        protected INavigationService navigationService;

        protected EditMode Mode;
        
        protected AddCategoryViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            GoBackCommand = new RelayCommand(navigationService.GoBack);

            var iconColours = ColorTools.GetColorsList();
            this.IconColors = new ObservableCollection<Color>();
            foreach (var i in iconColours)
            {
                this.IconColors.Add(i);
            }

            this.Icons = GetIcons();

            this.Profiles = new ObservableCollection<ProfileItem>();
            LoadProfiles();

            SaveAndReturnCommand = new RelayCommand(SaveAndReturn);
            SaveAndContinueCommand = new RelayCommand(SaveAndContinue);
        }

        public string Name { get; set; }

        public ProfileItem Profile { get; set; }

        public ObservableCollection<ProfileItem> Profiles { get; protected set; }

        public string Icon { get; set; }

        public ObservableCollection<string> Icons { get; protected set; }

        public Color IconColor { get; set; }

        public ObservableCollection<Color> IconColors { get; protected set; }

        public RelayCommand GoBackCommand { get; set; }

        public RelayCommand SaveAndReturnCommand { get; set; }

        public RelayCommand SaveAndContinueCommand { get; set; }

        public int SaveButtonColumnSpan { get; set; }

        public Visibility ShowSaveAndAdd { get; set; }

        protected async void LoadProfiles()
        {
            var profiles = await DataAccessProvider.GetAll<Profile>();
            foreach (var i in profiles)
            {
                Profiles.Add(new ProfileItem(i));
            }

            Profile = Profiles.Where(j => j.Id == DataAccessProvider.CurrentProfileId).First();
            RaisePropertyChanged("Profile");
        }

        protected ObservableCollection<string> GetIcons()
        {
            ObservableCollection<string> Icons = new ObservableCollection<string>();
            for (int i = 0xE000; i < 0xE300; i += 8)
            {
                Icons.Add(((char)i).ToString());
            }
            return Icons;
        }

        protected async void SaveAndReturn()
        {
            if (await Save())
            {
                navigationService.GoBack();
            }
        }

        protected async void SaveAndContinue()
        {
            if (await Save())
            {
                ClearFields();
            }
        }

        protected virtual bool CheckValues()
        {
            bool b = true;
            b = b && (Name != null && Name != string.Empty);
            b = b && Profile != null;
            b = b && (Icon != null && Icon != string.Empty);
            b = b && (IconColor != null);
            return b;
        }

        protected virtual void ClearFields()
        {
            Name = null;
            Profile = null;
            Icon = null;

            RaisePropertyChanged("Name");
            RaisePropertyChanged("Profile");
            RaisePropertyChanged("IconColor");
        }

        public virtual void Activate(object categoryNameParameter)
        {
            var categoryName = categoryNameParameter as string;
            if (null == categoryName || string.Empty == categoryName)
            {
                Mode = EditMode.Add;
                SaveButtonColumnSpan = 1;
                ShowSaveAndAdd = Visibility.Visible;
                ClearFields();
            }
            else
            {
                Mode = EditMode.Edit;
                SaveButtonColumnSpan = 3;
                ShowSaveAndAdd = Visibility.Collapsed;
            }

            RaisePropertyChanged("SaveButtonColumnSpan");
            RaisePropertyChanged("ShowSaveAndAdd");
        }

        protected abstract Task<bool> Save();

    }
}
