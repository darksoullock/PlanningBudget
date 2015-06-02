using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace PlanningBudget.ViewModels
{
    public class Helpers
    {
        public static async Task<bool> DeleteConfirmationMessage(string deletedobject = "")
        {
            var msg = new MessageDialog(string.Format("Do you want to delete \"{0}\" ?", deletedobject));
            msg.Commands.Add(new UICommand("yes"));
            msg.Commands.Add(new UICommand("no"));
            var result = await msg.ShowAsync();

            return result == msg.Commands[0];
        }
    }
}
