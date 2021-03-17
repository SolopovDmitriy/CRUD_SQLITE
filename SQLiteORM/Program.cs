using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteORM
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                /*SQLiteConnector.CreateDatabaseSource("test.db");
                SQLiteConnector.Connection.Open();
                string createStudentTableQuery = "CREATE TABLE IF NOT EXISTS students (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, fio VARCHAR(128) NOT NULL, age INTEGER )";
                    SQLiteCommand sQLiteCommand = new SQLiteCommand(createStudentTableQuery, SQLiteConnector.Connection);
                sQLiteCommand.ExecuteNonQuery();
                SQLiteConnector.Connection.Close();*/

                string pathTofile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database", "test.db");
                SQLiteDBEngine dBEngine = new SQLiteDBEngine(pathTofile, SQLIteMode.EXISTS);


                Console.WriteLine(dBEngine["student"].Name);

                SQLiteTable Students = dBEngine["student"];
                foreach (SQLiteColumn col in Students.HeadRowInfo)
                {
                    Console.WriteLine(col);
                }

                // Students.RemoveRowById(6);
                //Console.WriteLine("name of table is "+ Students.Name );


                //// create 
                //List<string> newStudent = new List<string>();
                //newStudent.Add("Petr5");
                //newStudent.Add("45");
                //Students.CreateRow(newStudent);


                // update 
                List<string> newStudent = new List<string>();
                newStudent.Add("aaaaa");
                newStudent.Add("100");
                Students.UpdateRow(16, newStudent);

              
                Console.WriteLine("Данные таблицы");
                foreach (var col in Students.BodyRows)
                {
                    Console.WriteLine($"ID: {col.Key}");

                    Console.WriteLine("size of col.Value = " + col.Value.Count);
                    foreach (var item in col.Value)
                    {
                        Console.Write(item + " ");
                    }
                    Console.WriteLine();
                    Console.WriteLine("======================");
                }

                /*foreach (var item in dBEngine.Tables)
                {
                    Console.WriteLine(item);
                }
                dBEngine.getTableFields("student");*/

                //string[] str = { };
                /*SQLiteColumn sQLiteColumn = new SQLiteColumn(str);
                //sQLiteColumn.GetDateType("VARCHAR(128)");
                Console.WriteLine(sQLiteColumn.DataType);
                */

                /* SQLiteRow sQLiteRow = new SQLiteRow(3);
                 foreach (var item in sQLiteRow)
                 {
                     Console.WriteLine(item);
                 }*/
                //Console.WriteLine(sQLiteRow[0]);
                //Console.WriteLine(sQLiteRow["id"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
    }
}
