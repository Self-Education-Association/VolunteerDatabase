using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Entity
{
    public class DatabaseContext
    {
        private static Database database;

        private static readonly object locker = new object();

        public static Database GetInstance()
        {
            if (database == null)
            {
                lock(locker)
                {
                    if (database == null)
                    {
                        database = new Database();
                    }
                }
            }
            return database;
        }

        public async static Task<Database> GetInstanceAsync()
        {
            return await Task.Run(() => GetInstance());
        }
    }
}
