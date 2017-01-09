using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Entity
{
    /// <summary>
    /// 用于获取数据库Database实例的单例工厂类
    /// </summary>
    public class DatabaseContext
    {
        private static Database database;

        private static readonly object locker = new object();

        /// <summary>
        /// 获取数据库Database实例，请不要调用实例的Dispose方法或使用using代码块调用。
        /// </summary>
        /// <returns>Database实例</returns>
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
