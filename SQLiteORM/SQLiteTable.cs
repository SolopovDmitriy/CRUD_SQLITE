using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteORM
{
    class SQLiteTable
    {
        private string _name;
        private SQLiteRow _headRow;
        private SortedList<long, List<string>> _bodyRows;
        public SQLiteRow HeadRowInfo
        {
            get
            {
                return _headRow;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
        }
        public SortedList<long, List<string>> BodyRows {
            get
            {
                return _bodyRows;
            }
        }
        public SQLiteTable(string tableName, SQLiteRow headRow, SortedList<long, List<string>> bodyRows)
        {
            _name = tableName; //валидация?
            _headRow = headRow;
            _bodyRows = bodyRows;
        }

        public void RemoveRowById(long id)
        {          
            SQLiteConnector.Connection.Open();
            string queryGetTablesData = $"DELETE FROM {_name} WHERE id = {id}";
            SQLiteCommand sQLiteCommand = new SQLiteCommand(queryGetTablesData, SQLiteConnector.Connection);
            int deletedCount = sQLiteCommand.ExecuteNonQuery();
            Console.WriteLine( "deleted " + deletedCount + " row(s)" );
            SQLiteConnector.Connection.Close();
            _bodyRows.Remove(id);
        }

       public void CreateRow(List<string> row)
        {          
            SQLiteConnector.Connection.Open();
            string queryGetTablesData = $"INSERT INTO {_name} (";  // делаем текст запроса        

            foreach (SQLiteColumn column in _headRow)
            {
                if (!column.IsPrimaryKey)
                {
                    queryGetTablesData += $"'{column.Name}',";
                }
            }
            queryGetTablesData = queryGetTablesData.Substring(0, queryGetTablesData.Length - 1);
            queryGetTablesData += ") VALUES (";

            foreach (string columnValue in row)
            {                
                queryGetTablesData += $"'{columnValue}',";               
            }
            queryGetTablesData = queryGetTablesData.Substring(0, queryGetTablesData.Length - 1);
            queryGetTablesData += ")";

            SQLiteCommand sQLiteCommand = new SQLiteCommand(queryGetTablesData, SQLiteConnector.Connection);
            int insertedCount = sQLiteCommand.ExecuteNonQuery();//запуск sql запрос и возвращает сколько строк добавлено
            Console.WriteLine("inserted " + insertedCount + " row(s)");
            queryGetTablesData = $"SELECT MAX(id) FROM {_name}";// возвращает только что добавленный id студента
            sQLiteCommand = new SQLiteCommand(queryGetTablesData, SQLiteConnector.Connection);
            long lastId = (long)sQLiteCommand.ExecuteScalar();//возврашает одну ячейку первого рядка первого столбца           
            SQLiteConnector.Connection.Close();

            _bodyRows.Add(lastId, row);//SortedList

        }
       
        //UPDATE student SET fio = 'DFcz', age = '24' WHERE id = 16;
        public void UpdateRow(long id, List<string> row)
        {
            SQLiteConnector.Connection.Open();
            string queryGetTablesData = $"UPDATE {_name} SET ";//делаем текст запроса
            int i = 0;
            foreach (SQLiteColumn column in _headRow)//column - данные о столбце (cid, Name, IsPrimaryKey, .....), _headRow - все столбцы - columns
            {
                if (!column.IsPrimaryKey)
                {
                    queryGetTablesData += $"{column.Name} = '{row[i]}',"; // {column.Name} = '{row[i]}',    -     fio = 'DFcz',
                    i++;
                }
               
            }
            queryGetTablesData = queryGetTablesData.Substring(0, queryGetTablesData.Length - 1);           
            queryGetTablesData += $" WHERE id = {id} ";

            Console.WriteLine(queryGetTablesData);

            SQLiteCommand sQLiteCommand = new SQLiteCommand(queryGetTablesData, SQLiteConnector.Connection);
            int updatedCount = sQLiteCommand.ExecuteNonQuery();//запуск sql запрос и возвращает сколько строк добавлено
            Console.WriteLine("updated " + updatedCount + " row(s)");
            SQLiteConnector.Connection.Close();

            _bodyRows[id] = row;
        }



    }
}
