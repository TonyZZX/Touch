#region

using Windows.Storage;
using Microsoft.Data.Sqlite;

#endregion

namespace Touch.Data
{
    internal abstract class Database
    {
        /// <summary>
        ///     SQLite file name
        /// </summary>
        protected const string FileName = "Filename=Touch_SQLite.db";

        /// <summary>
        ///     Execute SQL Command
        /// </summary>
        /// <param name="cmd">Command</param>
        /// <returns>Result after a command executed</returns>
        private static SqliteDataReader ExecuteCmd(string cmd)
        {
            SqliteDataReader result;
            using (var db = new SqliteConnection(FileName))
            {
                db.Open();
                result = new SqliteCommand(cmd, db).ExecuteReader();
            }

            return result;
        }

        /// <summary>
        ///     Create table
        /// </summary>
        public abstract void Create();

        /// <summary>
        ///     Create table
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="columns">Columns</param>
        protected void Create(string tableName, string columns)
        {
            ExecuteCmd("CREATE TABLE IF NOT EXISTS " + tableName + " (" + columns + ")");

            // Add a mark that the table has been created to LocalSettings
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[FileName] = true;
        }

        /// <summary>
        ///     Drop table
        /// </summary>
        public abstract void Drop();

        /// <summary>
        ///     Drop table
        /// </summary>
        /// <param name="tableName">Table name</param>
        protected void Drop(string tableName)
        {
            ExecuteCmd("DROP TABLE IF EXISTS " + tableName);

            // Remove the mark in LocalSettings
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[FileName] = false;
        }

        /// <summary>
        ///     Select all items
        /// </summary>
        /// <returns>Result of all items</returns>
        public abstract SqliteDataReader SelectAll();

        /// <summary>
        ///     Select all items
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <returns>Result of all items</returns>
        protected SqliteDataReader SelectAll(string tableName)
        {
            return ExecuteCmd("SELECT * FROM " + tableName);
        }

        /// <summary>
        ///     Initialize all tables
        /// </summary>
        public static void InitTables()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            var ifCreated = localSettings.Values[FolderTable.TableName] as bool?;
            if (ifCreated == null || ifCreated == false)
                new FolderTable().Create();
        }
    }
}