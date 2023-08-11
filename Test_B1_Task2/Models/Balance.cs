using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_B1_Task2.Models
{
    public class Balance
    {
        public int BalanceID;
        public decimal InActSaldo;
        public decimal InPassiveSaldo;
        public decimal TurnDebit;
        public decimal TurnCredit;
        public decimal OutActSaldo;
        public decimal OutPassiveSaldo;

        public AccountBalance AccountBalance { get; set; }
    }
}
