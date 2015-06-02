using SQLite;
using System;

namespace PlanningBudget.Models
{
    [Table("Transaction")]
    public class Transaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public bool IsIncome { get; set; }
        public string Name { get; set; }
        public int AccountId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}
