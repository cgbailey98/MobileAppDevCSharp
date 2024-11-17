using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using C971.Models;
using SQLite;

namespace C971.Services
{
    public static class DatabaseService
    {
        private static SQLiteAsyncConnection? _db;
        static async Task Init()
        {
            if (_db != null)
            {
                return;
            }

            var databasePath =
                Path.Combine(FileSystem.AppDataDirectory, "Terms.db");
            _db = new SQLiteAsyncConnection(databasePath);

            await _db.CreateTableAsync<Term>();
            await _db.CreateTableAsync<Course>();
            await _db.CreateTableAsync<Assessment>();
        }
        #region Term methods



        #endregion

        #region Course methods



        #endregion

        #region Assessment methods

        

        #endregion
    }
}
