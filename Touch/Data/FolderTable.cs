#region

using Microsoft.Data.Sqlite;

#endregion

namespace Touch.Data
{
    /// <summary>
    ///     Folder table in database
    /// </summary>
    internal class FolderTable : Database
    {
        /// <summary>
        ///     Folder table name
        /// </summary>
        public const string TableName = "FolderTable";

        /// <summary>
        ///     Create folder table
        /// </summary>
        public override void Create()
        {
            Create(TableName, "Path NVARCHAR(2048) PRIMARY KEY, " +
                              "Token NVARCHAR(2048) NULL");
        }

        /// <summary>
        ///     Drop folder table
        /// </summary>
        public override void Drop()
        {
            Drop(TableName);
        }

        /// <summary>
        ///     Select all folder items
        /// </summary>
        /// <returns>Result of all folder items</returns>
        public override SqliteDataReader SelectAll()
        {
            return SelectAll(TableName);
        }

        /// <summary>
        ///     Insert folder item
        /// </summary>
        /// <param name="path">Folder path</param>
        /// <param name="token">Token in FutureAccessList</param>
        public void Insert(string path, string token)
        {
            using (var db = new SqliteConnection(FileName))
            {
                db.Open();
                var cmd = new SqliteCommand
                {
                    Connection = db,
                    CommandText = "INSERT INTO " + TableName + " VALUES (@path, @token)"
                };
                // Use parameterized query to prevent SQL injection attacks
                cmd.Parameters.AddWithValue("@path", path);
                cmd.Parameters.AddWithValue("@token", token);
                cmd.ExecuteReader();
            }
        }

        /// <summary>
        ///     Delete item based on folder path
        /// </summary>
        /// <param name="path">Folder path</param>
        public void Delete(string path)
        {
            using (var db = new SqliteConnection(FileName))
            {
                db.Open();
                var cmd = new SqliteCommand
                {
                    Connection = db,
                    CommandText = "DELETE FROM " + TableName + " WHERE Path = @path"
                };
                // Use parameterized query to prevent SQL injection attacks
                cmd.Parameters.AddWithValue("@path", path);
                cmd.ExecuteReader();
            }
        }
    }
}