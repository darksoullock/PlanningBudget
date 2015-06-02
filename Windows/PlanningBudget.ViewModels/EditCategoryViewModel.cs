using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using PlanningBudget.Core;
using PlanningBudget.DataAccess;
using PlanningBudget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media;

namespace PlanningBudget.ViewModels
{
    public class EditCategoryViewModel : CategoryViewModel, INavigable
    {
        protected string categoryName;
        
        public EditCategoryViewModel(INavigationService navigationService)
            :base(navigationService)
        {
            
        }

        protected override bool Save()
        {
            if (CheckValues())
            {
                //DataAccessProvider.UpdateCategory(categoryName,
                //    new Expense()
                //    {
                //        Name=this.Name,
                //        IsIncome=this.Type==incomeTypeName,
                //        Color = ColorTools.ColorToArgb(this.IconColor.Color),
                //        Icon = this.Icon[0]
                //    });
                ClearFields();
                return true;
            }
            else
            {
                //new MessageDialog("All fields should be filled").ShowAsync();
                return false;
            }
        }

        public async void Activate(object parameter)
        {
            var currentCategory = await DataAccessProvider.GetCategoryByName<Expense>(parameter as string);
            this.Name = this.categoryName = currentCategory.Name;
            
            var tempicon = currentCategory.Icon;
            if (!this.Icons.Contains(tempicon))
            {
                this.Icons.Add(tempicon);
            }

            this.Icon = tempicon;
            
            var tempcolor = ColorTools.ColorFromArgb(currentCategory.Color);
            if (!this.IconColours.Contains(tempcolor))
            {
                this.IconColours.Add(tempcolor);
            }
            
            this.IconColor = tempcolor;
            
            RaisePropertyChanged("Name");
            RaisePropertyChanged("Type");
            RaisePropertyChanged("Icon");
            RaisePropertyChanged("IconColor");
            
        }
    }
}
