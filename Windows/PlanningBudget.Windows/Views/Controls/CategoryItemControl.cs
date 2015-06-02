using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace PlanningBudget.Windows.Views.Controls
{
    public sealed class CategoryItemControl : Control
    {
        public CategoryItemControl()
        {
            this.DefaultStyleKey = typeof(CategoryItemControl);
        }

        public static readonly DependencyProperty ColorProperty =
               DependencyProperty.Register("Color", typeof(Color), typeof(CategoryItemControl),
                   new PropertyMetadata(null));

        public static readonly DependencyProperty OverrunProperty =
               DependencyProperty.Register("Overrun", typeof(bool), typeof(CategoryItemControl),
                   new PropertyMetadata(null));

        public static readonly DependencyProperty LabelProperty =
               DependencyProperty.Register("Label", typeof(string), typeof(CategoryItemControl),
                   new PropertyMetadata(null));

        public static readonly DependencyProperty PercentageProperty =
               DependencyProperty.Register("Percentage", typeof(string), typeof(CategoryItemControl),
                   new PropertyMetadata(null));

        public static readonly DependencyProperty IconProperty =
               DependencyProperty.Register("Icon", typeof(string), typeof(CategoryItemControl),
                   new PropertyMetadata(null));

        public static readonly DependencyProperty ValueProperty =
              DependencyProperty.Register("Value", typeof(string), typeof(CategoryItemControl),
                  new PropertyMetadata(null));

        public static readonly DependencyProperty SecondaryValueProperty =
              DependencyProperty.Register("SecondaryValue", typeof(string), typeof(CategoryItemControl),
                  new PropertyMetadata(null));

        public static DependencyProperty AddIconProperty =
            DependencyProperty.Register("IsAddIcon", typeof(bool), typeof(CategoryItemControl),
                  new PropertyMetadata(null));

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public SolidColorBrush BorderColor
        {
            get { return new SolidColorBrush(Overrun?Colors.Red:Colors.White); }
        }

        public bool Overrun
        {
            get { return (bool)GetValue(OverrunProperty); }
            set { SetValue(OverrunProperty, value); }
        }

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public string Percentage
        {
            get { return (string)GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public string SecondaryValue
        {
            get { return (string)GetValue(SecondaryValueProperty); }
            set { SetValue(SecondaryValueProperty, value); }
        }

        public bool IsAddIcon
        {
            get { return (bool)GetValue(AddIconProperty); }
            set { SetValue(AddIconProperty, value); }
        }

        public Visibility AddIconVisibility
        {
            get { return IsAddIcon ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility GeneralIconVisibility
        {
            get { return (!IsAddIcon) ? Visibility.Visible : Visibility.Collapsed; }
        }
    }
}
