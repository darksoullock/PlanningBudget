using SQLite;

namespace PlanningBudget.Models
{
    [Table("Account")]
    public class Account : ItemWithNameColorAndIcon
    {
        public decimal Balance { get; set; }
     
        public int ProfileId { get; set; }

        public override bool Equals(object obj)
        {
            return (obj is Account) && ((Account)obj).Id == Id;
        }
    }
}
