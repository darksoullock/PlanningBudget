using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace PlanningBudget.Windows.Views.Controls
{
    public sealed class HeaderControl : Control
    {
        public HeaderControl()
        {
            this.DefaultStyleKey = typeof(HeaderControl);
        }

        public static DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(HeaderControl), null);

        public static DependencyProperty BackCommandProperty = DependencyProperty.Register(
            "BackCommand", typeof(ICommand), typeof(HeaderControl), null);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public ICommand BackCommand
        {
            get { return (ICommand)GetValue(BackCommandProperty); }
            set { SetValue(BackCommandProperty, value); }
        }
    }
}
