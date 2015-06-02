using SQLite;

namespace PlanningBudget.Models
{
    [Table("Income")]
    public class Income : ItemWithNameColorAndIcon
    {
        public int ProfileId { get; set; }

        public override bool Equals(object obj)
        {
            return (obj is Income) && ((Income)obj).Id == Id;
        }
    }
}
