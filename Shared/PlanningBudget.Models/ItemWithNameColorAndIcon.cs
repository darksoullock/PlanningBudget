using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningBudget.Models
{
    public abstract class ItemWithNameColorAndIcon
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        
        public uint Color { get; set; }
        
        public string Icon { get; set; }
    }
}
