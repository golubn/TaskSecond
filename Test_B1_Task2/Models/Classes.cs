using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_B1_Task2.Models
{
    public class Classes
    {
        public int ClassID;
        public string ClassName;
        public string FileName;

        public List<AccountBalance> AccountBalances { get; set; } = new List<AccountBalance>();
        public List<TotalSumInClass> TotalSumsInClass { get; set; } = new List<TotalSumInClass>();
    }
}
