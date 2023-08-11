using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_B1_Task2.Models
{
    public class TotalSumInClass 
    {
        public int ClassID;
        public decimal InActSaldo;
        public decimal InPassiveSaldo;
        public decimal TurnDebit;
        public decimal TurnCredit;
        public decimal OutActSaldo;
        public decimal OutPassiveSaldo;

        public Classes Classes { get; set; }

    }
}
