using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace PlanningBudget.Windows.Views.Controls
{
    public sealed class EditCategoryControl : Control
    {
        public EditCategoryControl()
        {
            this.DefaultStyleKey = typeof(EditCategoryControl);
            IsAdding = true;
        }

        public static readonly DependencyProperty CategoryNameProperty =
            DependencyProperty.Register("CategoryName", typeof(string), typeof(EditCategoryControl), null);

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(string), typeof(EditCategoryControl), null);

        public static readonly DependencyProperty TypesProperty =
                    DependencyProperty.Register("Types", typeof(ObservableCollection<string>), typeof(EditCategoryControl), null);

        public static readonly DependencyProperty IconColorProperty =
                   DependencyProperty.Register("IconColor", typeof(SolidColorBrush), typeof(EditCategoryControl), null);

        public static readonly DependencyProperty IconColoursProperty =
                   DependencyProperty.Register("IconColours", typeof(ObservableCollection<SolidColorBrush>), typeof(EditCategoryControl), null);

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string), typeof(EditCategoryControl), null);

        public static readonly DependencyProperty IconsProperty =
                    DependencyProperty.Register("Icons", typeof(ObservableCollection<string>), typeof(EditCategoryControl), null);

        public static readonly DependencyProperty SaveAndReturnCommandProperty =
                    DependencyProperty.Register("SaveAndReturnCommand", typeof(ICommand), typeof(EditCategoryControl), null);

        public static readonly DependencyProperty SaveAndContinueCommandProperty =
                    DependencyProperty.Register("SaveAndContinueCommand", typeof(ICommand), typeof(EditCategoryControl), null);

        public static readonly DependencyProperty IsAddingProperty =
                   DependencyProperty.Register("IsAdding", typeof(bool), typeof(EditCategoryControl), null);

        public string CategoryName
        {
            get { return (string)GetValue(CategoryNameProperty); }
            set { SetValue(CategoryNameProperty, value); }
        }

        public string Type
        {
            get { return (string)GetValue(TypeProperty); }
            set { SetValue(CategoryNameProperty, value); }
        }

        public ObservableCollection<string> Types
        {
            get { return (ObservableCollection<string>)GetValue(TypesProperty); }
            set { SetValue(CategoryNameProperty, value); }
        }

        public SolidColorBrush IconColor
        {
            get { return (SolidColorBrush)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }

        public ObservableCollection<SolidColorBrush> IconColours
        {
            get { return (ObservableCollection<SolidColorBrush>)GetValue(IconColoursProperty); }
            set { SetValue(IconColoursProperty, value); }
        }

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public ObservableCollection<string> Icons
        {
            get { return (ObservableCollection<string>)GetValue(IconsProperty); }
            set { SetValue(IconsProperty, value); }
        }

        public ICommand SaveAndReturnCommand
        {
            get { return (ICommand)GetValue(SaveAndReturnCommandProperty); }
            set { SetValue(SaveAndReturnCommandProperty, value); }
        }

        public ICommand SaveAndContinueCommand
        {
            get { return (ICommand)GetValue(SaveAndContinueCommandProperty); }
            set { SetValue(SaveAndContinueCommandProperty, value); }
        }

        public bool IsAdding
        {
            get { return (bool)GetValue(IsAddingProperty); }
            set { SetValue(IsAddingProperty, value); }
        }

        public Visibility ShowSaveAndAdd
        {
            get
            {
                return IsAdding ?
                    global::Windows.UI.Xaml.Visibility.Visible :
                    global::Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        public int SaveButtonColumnSpan
        {
            get
            {
                return IsAdding ? 1 : 3;
            }
        }
    }
}
