using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ApartmentDTO
    {
        public int APId { get; set; }
        public float Debt { get; set; }
        public float Balance { get; set; }
        public UserForRegistrationDTO? Resident { get; set; }
        public ExpenseDTO? Expense { get; set; }
        public ActivityDTO? Activity { get; set; }
    }
}
