using SQLite;

namespace PlanningBudget.Models
{
    [Table("Expense")]
    public class Expense : ItemWithNameColorAndIcon
    {
        public Expense()
        {
            ParentId = -1;
        }

        public decimal Budget { get; set; }

        public int ProfileId { get; set; }
        
        public int ParentId { get; set; }

        public override bool Equals(object obj)
        {
            return (obj is Expense) && ((Expense)obj).Id == Id;
        }
    }
}
