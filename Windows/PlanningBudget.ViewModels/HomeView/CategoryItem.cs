using GalaSoft.MvvmLight;
using PlanningBudget.Core;
using PlanningBudget.Models;
using System;
using Windows.UI;

namespace PlanningBudget.ViewModels.HomeView
{
    public abstract class CategoryItem : ItemWithNameColorAndIconForBinding
    {
        public abstract string Percentage { get; }

        public int Id { get; set; }

        public CategoryItem()
        {
        }

        public CategoryItem(string name, uint color, string icon, int id)
        {
            Name = name;
            IconColor = ColorTools.ColorFromArgb(color);
            Icon = icon;
            Id = id;
        }
    }
}
