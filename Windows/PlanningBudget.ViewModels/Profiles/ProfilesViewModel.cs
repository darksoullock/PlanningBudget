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
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace PlanningBudget.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private INavigationService navigationService;

        private ProfileItem selectedProfile;

        private Visibility changeButtonsVisibility = Visibility.Collapsed;

        public ProfileViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            Profiles = new ObservableCollection<object>();
            LoadProfiles();

            AddingProfile = new EditProfile();
            EditingProfile = new EditProfile();

            GoBackCommand = new RelayCommand(navigationService.GoBack);
            AddCommand = new RelayCommand(AddProfile);
            SaveCommand = new RelayCommand(SaveProfile);
            ProfileClickCommand = new RelayCommand<object>(ProfileClick);
            DeleteProfileCommand = new RelayCommand(DeleteProfile);
            OpenEditCommand = new RelayCommand(OpenEdit);

            DataAccessProvider.ProfileAdded += ProfileAdded;
        }

        public RelayCommand SaveCommand { get; set; }

        public RelayCommand AddCommand { get; set; }

        public RelayCommand GoBackCommand { get; set; }

        public RelayCommand DeleteProfileCommand { get; set; }

        public RelayCommand OpenEditCommand { get; set; }

        public RelayCommand<object> ProfileClickCommand { get; set; }

        public bool IsAppBarOpen { get; set; }

        public ObservableCollection<object> Profiles { get; set; }

        public ProfileItem SelectedProfile
        {
            get { return selectedProfile; }
            set
            {
                ChangeButtonsVisibility = value == null ? Visibility.Collapsed : Visibility.Visible;
                selectedProfile = value;
                RaisePropertyChanged("SelectedProfile");
            }
        }

        public EditProfile AddingProfile { get; set; }

        public EditProfile EditingProfile { get; set; }

        public bool IsFlyoutClosed { get; set; }

        public Visibility ChangeButtonsVisibility
        {
            get { return changeButtonsVisibility; }
            set
            {
                changeButtonsVisibility = value;
                RaisePropertyChanged();
            }
        }

        public async void AddProfile()
        {
            if (AddingProfile.Name == string.Empty ||
                AddingProfile.Icon == string.Empty ||
                AddingProfile.Name == null ||
                AddingProfile.Icon == null)
            {
                await new MessageDialog("Not all fields filled").ShowAsync();
                return;
            }

            var profile = new Profile()
                {
                    Icon = AddingProfile.Icon,
                    Color = ColorTools.ColorToArgb(AddingProfile.IconColor),
                    Name = AddingProfile.Name
                };

            bool b;
            b = await DataAccessProvider.AddCategory<Profile>(profile);

            if (b)
            {
                CloseFlyout();
            }
            else
            {
                await new MessageDialog("This name has been already taken").ShowAsync();
            }
        }

        private async void DeleteProfile()
        {
            if (selectedProfile != null && await Helpers.DeleteConfirmationMessage())
            {
                await DataAccessProvider.DeleteCategoryByName<Profile>(selectedProfile.Name);
                Profiles.Remove(selectedProfile);
                selectedProfile = null;
            }
        }

        public void OpenEdit()
        {
            EditingProfile.Name = selectedProfile.Name;
            EditingProfile.IconColor = selectedProfile.IconColor;
            EditingProfile.Icon = selectedProfile.Icon;
            RaisePropertyChanged("EditingProfile");
        }

        private void CloseFlyout()
        {
            IsFlyoutClosed = false;
            RaisePropertyChanged("IsFlyoutClosed");
            IsFlyoutClosed = true;
            RaisePropertyChanged("IsFlyoutClosed");
        }

        private void ShowAppBar()
        {
            IsAppBarOpen = true;
            RaisePropertyChanged("IsAppBarOpen");
        }

        private void ProfileClick(object obj)
        {
            if ((obj as ProfileItem).IsAddIcon)
            {
                ShowAppBar();
                // TODO Open flyout
            }
        }

        public async void SaveProfile()
        {
            var p = await DataAccessProvider.GetCategoryById<Profile>(selectedProfile.Id);
            p.Name = EditingProfile.Name;
            p.Color = ColorTools.ColorToArgb(EditingProfile.IconColor);
            p.Icon = EditingProfile.Icon;

            if (await DataAccessProvider.UpdateCategory<Profile>(p))
            {
                selectedProfile.Name = EditingProfile.Name;
                selectedProfile.IconColor = EditingProfile.IconColor;
                selectedProfile.Icon = EditingProfile.Icon;
                RaisePropertyChanged("SelectedProfile");
                CloseFlyout();
            }
        }

        void ProfileAdded(object sender, Core.CategoryAddedEventArgs<Profile> e)
        {
            Profiles.Add(new ProfileItem(e.NewCategory));
        }

        private async void LoadProfiles()
        {
            Profiles.Add(new ProfileItem() { IsAddIcon = true });
            var profiles = await DataAccess.DataAccessProvider.GetAll<Profile>();
            foreach (var i in profiles)
            {
                Profiles.Add(new ProfileItem(i));
            }
        }
    }
}
