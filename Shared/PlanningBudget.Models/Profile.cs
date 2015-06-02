using SQLite;

namespace PlanningBudget.Models
{
    [Table("Profile")]
    public class Profile : ItemWithNameColorAndIcon
    {
        public override bool Equals(object obj)
        {
            return (obj is Profile) && ((Profile)obj).Id == Id;
        }
    }
}
