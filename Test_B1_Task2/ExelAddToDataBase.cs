global using Microsoft.Data.Sqlite;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Test_B1_Task2
{
    internal class ExelAddToDataBase
    {
        private CultureInfo _culture = new CultureInfo("en-US");
        private const string ConnectionString = "Data Source=G:\\TestB1Second\\Vedomost.db"; // строка подключения к базе данных

        internal void AddExelFileToDb()
        {
            string filePath = "C:\\Users\\Admin\\Desktop\\TestB1\\Тестовое.xlsx"; // путь к загружаемому файлу

            using (var wb = new XLWorkbook(filePath))
            {
                var worksheet = wb.Worksheet(1);
                int currentClassId = -1;

                using (SqliteConnection connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();

                    AddUploadedFile(filePath, connection); // добавление информации о загруженном файле в таблицу бд
                    // перебор строк файла
                    foreach (var row in worksheet.RowsUsed().Skip(7)) // skip(7) -  так как в начале файла есть заголовки, переходим сразу к данным
                    {
                        string cellValue = worksheet.Cell(row.RowNumber(), 1).Value.ToString();
                        // обработка данных по строкам
                        if (!cellValue.StartsWith("КЛАСС") && !cellValue.StartsWith("ПО КЛАССУ") && !cellValue.StartsWith("БАЛАНС"))
                        {
                            if (currentClassId == -1)
                            {
                                continue;
                            }
                            string tableName = GetTableName(row);
                            if (tableName == "")
                            {
                                continue;
                            }

                            decimal[] values = GetDecimalValues(row);
                            if (values == null)
                            {
                                continue;
                            }

                            int accountId = AddAccount(row.Cell(1).Value.ToString(), currentClassId, connection);

                            if (accountId != -1)
                            {
                                AddBalance(accountId, values, connection, tableName);
                            }
                        }
                        else if (!string.IsNullOrEmpty(cellValue) && cellValue.StartsWith("КЛАСС"))
                        {
                            currentClassId = AddClassGetID(cellValue.Trim(), filePath, connection);
                        }
                        else if (cellValue.StartsWith("ПО КЛАССУ"))
                        {
                            TotalClassSum(connection);
                        }
                        else if (cellValue.StartsWith("БАЛАНС"))
                        {
                            AllSumForYear(connection);
                        }
                        else
                        {
                            MessageBox.Show("Проверьте поле Б/сч");
                        }
                    }
                }

                MessageBox.Show("Данные загружены.");
            }
        }

        private string GetTableName(IXLRow row) // имя таблицы. зависит от поля б/cч
        {
            string accountNumber = row.Cell(1).Value.ToString();
            if (accountNumber.Length < 3)
            {
                return "SumForPart";
            }
            else
            {
                return "Balance";
            }
        }

        private decimal[] GetDecimalValues(IXLRow row) // парсинг строк
        {
            decimal[] values = new decimal[6];
            for (int i = 0; i < values.Length; i++)
            {
                string valueStr = row.Cell(i + 2).Value.ToString();
                if (!decimal.TryParse(valueStr.Replace(",", ".").Replace(" ", ""), NumberStyles.Any, _culture, out values[i]))
                {
                    MessageBox.Show("Ошибка при парсинге значения: " + valueStr);
                    return values;
                }
            }
            return values;
        }


        private decimal OutActSalso(decimal InActSaldo, decimal Debit, decimal Credit)
        {
            return InActSaldo + Debit - Credit;
        }

        private decimal OutPassiveSalso(decimal InPassiveSaldo, decimal Debit, decimal Credit)
        {
            return InPassiveSaldo - Debit + Credit;
        }


        private void AddBalance(int accountId, decimal[] values, SqliteConnection connection, string tableName)
        {
            string insertBalanceQuery = $"INSERT INTO {tableName} (BalanceID, InActSaldo, InPassiveSaldo, TurnDebit, TurnCredit, OutActSaldo, OutPassiveSaldo) VALUES (@AccountID, @InActSaldo, @InPassiveSaldo, @TurnDebit, @TurnCredit, @OutActSaldo, @OutPassiveSaldo)";

            using (SqliteCommand command = new SqliteCommand(insertBalanceQuery, connection))
            {
                command.Parameters.AddWithValue("@AccountID", accountId);
                command.Parameters.AddWithValue("@InActSaldo", values[0]);
                command.Parameters.AddWithValue("@InPassiveSaldo", values[1]);
                command.Parameters.AddWithValue("@TurnDebit", values[2]);
                command.Parameters.AddWithValue("@TurnCredit", values[3]);
               // command.Parameters.AddWithValue("@OutActSaldo", values[4]);
                command.Parameters.AddWithValue("@OutActSaldo", OutActSalso(values[0], values[2], values[3]));
               // command.Parameters.AddWithValue("@OutPassiveSaldo", values[5]);
                command.Parameters.AddWithValue("@OutPassiveSaldo", OutPassiveSalso(values[1], values[2], values[3]));

                command.ExecuteNonQuery();
            }
        }
        private int AddClassGetID(string className, string fileName, SqliteConnection connection)
        {
            string getClassIdQuery = "SELECT ClassID FROM Classes WHERE ClassName = @ClassName";
            string insertClassQuery = "INSERT INTO Classes (ClassName, FileName) VALUES (@ClassName, @FileName); SELECT last_insert_rowid();";

            using (SqliteCommand command = new SqliteCommand(getClassIdQuery, connection))
            {
                command.Parameters.AddWithValue("@ClassName", className);
                command.Parameters.AddWithValue("@FileName", fileName);

                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    // Если класс не существует, создадим новый класс
                    using (SqliteCommand insertCommand = new SqliteCommand(insertClassQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@ClassName", className);
                        insertCommand.Parameters.AddWithValue("@FileName", fileName);
                        return Convert.ToInt32(insertCommand.ExecuteScalar());
                    }
                }
            }
        }
        private int AddAccount(string accountName, int classId, SqliteConnection connection)
        {
            string insertAccountQuery = "INSERT INTO AccountBalance (BalanceID, ClassId) VALUES (@AccountName, @ClassID); SELECT last_insert_rowid();";

            using (SqliteCommand command = new SqliteCommand(insertAccountQuery, connection))
            {
                command.Parameters.AddWithValue("@AccountName", accountName);
                command.Parameters.AddWithValue("@ClassID", classId);

                try
                {
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        return -1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении записи в базу данных: " + ex.Message);
                    return -1;
                }
            }
        }
        private void AllSumForYear(SqliteConnection connection)
        {
            string sumQuery = @"
        SELECT
            SUM(TotalActIn) AS TotalActIn,
            SUM(TotalPassIn) AS TotalPassIn,
            SUM(TotalDebit) AS TotalDebit,
            SUM(TotalCredit) AS TotalCredit,
            SUM(TotalActOut) AS TotalActOut,
            SUM(TotalPassOut) AS TotalPassOut
        FROM TotalSumInClass;";

            using (SqliteCommand sumCommand = new SqliteCommand(sumQuery, connection))
            {
                using (SqliteDataReader reader = sumCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        decimal TotalActIn = reader.GetDecimal(0);
                        decimal TotalPassIn = reader.GetDecimal(1);
                        decimal TotalDebit = reader.GetDecimal(2);
                        decimal TotalCredit = reader.GetDecimal(3);
                        decimal TotalActOut = reader.GetDecimal(4);
                        decimal TotalPassOut = reader.GetDecimal(5);

                        string updateTotalTableQuery = @"
                    INSERT INTO TotalSum (
                        TotalActIn,
                        TotalPassIn,
                        TotalDebit,
                        TotalCredit,
                        TotalActOut,
                        TotalPassOut
                    )
                    VALUES (
                        @TotalActIn,
                        @TotalPassIn,
                        @TotalDebit,
                        @TotalCredit,
                        @TotalActOut,
                        @TotalPassOut
                    );";

                        using (SqliteCommand updateTotalCommand = new SqliteCommand(updateTotalTableQuery, connection))
                        {
                            updateTotalCommand.Parameters.AddWithValue("@TotalActIn", TotalActIn);
                            updateTotalCommand.Parameters.AddWithValue("@TotalPassIn", TotalPassIn);
                            updateTotalCommand.Parameters.AddWithValue("@TotalDebit", TotalDebit);
                            updateTotalCommand.Parameters.AddWithValue("@TotalCredit", TotalCredit);
                           //updateTotalCommand.Parameters.AddWithValue("@TotalActOut", TotalActOut);
                            updateTotalCommand.Parameters.AddWithValue("@TotalActOut", OutActSalso(TotalActIn, TotalDebit, TotalCredit));
                            //updateTotalCommand.Parameters.AddWithValue("@TotalPassOut", TotalPassOut);
                            updateTotalCommand.Parameters.AddWithValue("@TotalPassOut", OutPassiveSalso(TotalPassIn, TotalDebit, TotalCredit));
                            updateTotalCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }


        private void AddUploadedFile(string fileName, SqliteConnection connection)
        {
            string insertFileQuery = "INSERT INTO UploadedFiles (FileName, DataLoad) VALUES (@FileName, @DataLoad)";

            using (SqliteCommand command = new SqliteCommand(insertFileQuery, connection))
            {
                command.Parameters.AddWithValue("@FileName", fileName);
                command.Parameters.AddWithValue("@DataLoad", DateTime.Now);
                command.ExecuteNonQuery();
            }
        }


        private void TotalClassSum(SqliteConnection connection)
        {
            string insertClassTotalQuery = @"
        INSERT INTO TotalSumInClass (
            ClassID,
            TotalActIn,
            TotalPassIn,
            TotalDebit,
            TotalCredit,
            TotalActOut,
            TotalPassOut
        )
        VALUES (
            @ClassId,
            @TotalActIn,
            @TotalPassIn,
            @TotalDebit,
            @TotalCredit,
            @TotalActOut,
            @TotalPassOut
        );";

            string getClassIdsQuery = "SELECT DISTINCT ClassId FROM AccountBalance";
            string sumByClassQuery = @"
        SELECT
            SUM(InActSaldo) AS TotalActIn,
            SUM(InPassiveSaldo) AS TotalPassIn,
            SUM(TurnDebit) AS TotalDebit,
            SUM(TurnCredit) AS TotalCredit,
            SUM(OutActSaldo) AS TotalActOut,
            SUM(OutPassiveSaldo) AS TotalPassOut
        FROM SumForPart
        WHERE BalanceId IN (SELECT BalanceID FROM AccountBalance WHERE ClassId = @ClassId)";
            string checkExistingRecordQuery = "SELECT COUNT(*) FROM TotalSumInClass WHERE ClassID = @ClassId";

            using (SqliteCommand insertClassTotalCommand = new SqliteCommand(insertClassTotalQuery, connection))
            using (SqliteCommand getClassIdsCommand = new SqliteCommand(getClassIdsQuery, connection))
            using (SqliteCommand sumByClassCommand = new SqliteCommand(sumByClassQuery, connection))
            using (SqliteCommand checkExistingRecordCommand = new SqliteCommand(checkExistingRecordQuery, connection))
            {
                connection.Open();

                using (SqliteDataReader classIdReader = getClassIdsCommand.ExecuteReader())
                {
                    while (classIdReader.Read())
                    {
                        int classId = classIdReader.GetInt32(0);

                        checkExistingRecordCommand.Parameters.Clear();
                        checkExistingRecordCommand.Parameters.AddWithValue("@ClassId", classId);
                        int existingRecordCount = Convert.ToInt32(checkExistingRecordCommand.ExecuteScalar());

                        // Если записи не существует, добавляем новую запись
                        if (existingRecordCount == 0)
                        {
                            sumByClassCommand.Parameters.Clear();
                            sumByClassCommand.Parameters.AddWithValue("@ClassId", classId);

                            using (SqliteDataReader reader = sumByClassCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    insertClassTotalCommand.Parameters.Clear();
                                    insertClassTotalCommand.Parameters.AddWithValue("@ClassId", classId);
                                    insertClassTotalCommand.Parameters.AddWithValue("@TotalActIn", reader.GetDecimal(0));
                                    insertClassTotalCommand.Parameters.AddWithValue("@TotalPassIn", reader.GetDecimal(1));
                                    insertClassTotalCommand.Parameters.AddWithValue("@TotalDebit", reader.GetDecimal(2));
                                    insertClassTotalCommand.Parameters.AddWithValue("@TotalCredit", reader.GetDecimal(3));
                                    //insertClassTotalCommand.Parameters.AddWithValue("@TotalActOut", reader.GetDecimal(4));
                                    insertClassTotalCommand.Parameters.AddWithValue("@TotalActOut", OutActSalso(reader.GetDecimal(0), reader.GetDecimal(2), reader.GetDecimal(3)));
                                    //insertClassTotalCommand.Parameters.AddWithValue("@TotalPassOut", reader.GetDecimal(5));
                                    insertClassTotalCommand.Parameters.AddWithValue("@TotalPassOut",OutPassiveSalso(reader.GetDecimal(1), reader.GetDecimal(2), reader.GetDecimal(3)));

                                    insertClassTotalCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
