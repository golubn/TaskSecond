using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_B1_Task2.Models
{
    public class AccountBalance
    {
        public int ClassId;
        public int BalanceID;
        public Classes Classes { get; set; }
        public Balance Balance { get; set; }
        public SumForPart SumsForPart { get; set; } 
    }
}
