using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_B1_Task2.Models
{
    public class SumForPart 
    {

        public int BalanceID { get; set; }
        
        public decimal InActSaldo { get; set; }
        public decimal InPassiveSaldo { get; set; }
        public decimal TurnDebit { get; set; }
        public decimal TurnCredit { get; set; }
        public decimal OutActSaldo { get; set; }
        public decimal OutPassiveSaldo { get; set; }
        public AccountBalance AccountBalance { get; set; }
    }
}
