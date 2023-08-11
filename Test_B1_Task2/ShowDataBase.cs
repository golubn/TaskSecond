using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Test_B1_Task2
{
    internal class ShowDataBase
    {
        private const string ConnectionString = "Data Source=G:\\TestB1Second\\Vedomost.db";
        internal void ShowData(DataGrid dataGrid)
        {
            using (SqliteConnection connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                // Запрос для получения данных
                string query = @"
            SELECT C.ClassName, B.*, TCl.*
            FROM Classes AS C
            JOIN AccountBalance AS A ON C.ClassID = A.ClassId
            JOIN Balance AS B ON A.BalanceID = B.BalanceID
            JOIN TotalSumInClass AS TCl ON A.ClassId = TCl.ClassID
            ORDER BY C.ClassName";

                SqliteCommand command = new SqliteCommand(query, connection);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    List<ClassData> classDataList = new List<ClassData>();
                    ClassData currentClassData = null;

                    while (reader.Read())
                    {
                        string className = reader.GetString(0);

                        if (currentClassData == null || currentClassData.ClassName != className)
                        {
                            currentClassData = new ClassData { ClassName = className };
                            classDataList.Add(currentClassData);
                        }

                        var balanceData = new BalanceData
                        {
                            AccountNumber = (int)reader.GetInt64(1),
                            InActSaldo = reader.GetDecimal(2),
                            InPassiveSaldo = reader.GetDecimal(3),
                            TurnDebit = reader.GetDecimal(4),
                            TurnCredit = reader.GetDecimal(5),
                            OutActSaldo = reader.GetDecimal(6),
                            OutPassiveSaldo = reader.GetDecimal(7)
                        };

                        var totalsumInClass = new TotalSumInClass
                        {
                            TotalSumName = "total sum",
                            InActSaldo = reader.GetDecimal(reader.GetOrdinal("TotalActIn")),
                            InPassiveSaldo = reader.GetDecimal(reader.GetOrdinal("TotalPassIn")),
                            TurnDebit = reader.GetDecimal(reader.GetOrdinal("TotalDebit")),
                            TurnCredit = reader.GetDecimal(reader.GetOrdinal("TotalCredit")),
                            OutActSaldo = reader.GetDecimal(reader.GetOrdinal("TotalActOut")),
                            OutPassiveSaldo = reader.GetDecimal(reader.GetOrdinal("TotalPassOut"))
                        };

                        currentClassData.Balances.Add(balanceData);
                        currentClassData.TotalSumsInClass.Add(totalsumInClass);
                    }

                    dataGrid.ItemsSource = classDataList;
                }
            }
        }
        public class ClassData
        {
            public string ClassName { get; set; }
            public List<BalanceData> Balances { get; set; } = new List<BalanceData>();
            public List<TotalSumInClass> TotalSumsInClass { get; set; } = new List<TotalSumInClass>();
        }

        public class BalanceData
        {
            public int AccountNumber { get; set; }
            public decimal InActSaldo { get; set; }
            public decimal InPassiveSaldo { get; set; }
            public decimal TurnDebit { get; set; }
            public decimal TurnCredit { get; set; }
            public decimal OutActSaldo { get; set; }
            public decimal OutPassiveSaldo { get; set; }
        }

        public class TotalSumInClass
        {
            public string TotalSumName { get; set; }
            public decimal InActSaldo { get; set; }
            public decimal InPassiveSaldo { get; set; }
            public decimal TurnDebit { get; set; }
            public decimal TurnCredit { get; set; }
            public decimal OutActSaldo { get; set; }
            public decimal OutPassiveSaldo { get; set; }
        }

    }
}

