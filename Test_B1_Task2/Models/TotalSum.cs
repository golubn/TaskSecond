using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_B1_Task2.Models
{
    public class TotalSum 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TotalSumID { get; set; }

        public decimal InActSaldo;
        public decimal InPassiveSaldo;
        public decimal TurnDebit;
        public decimal TurnCredit;
        public decimal OutActSaldo;
        public decimal OutPassiveSaldo;
    }
}
