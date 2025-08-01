using System;
using System.IO;

namespace EnvironmentVariableExplorer.Data
{
    public static class LiteDbContext
    {
        public static string DbPath { get; private set; }

        static LiteDbContext()
        {
            string dbFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AlgoReportsUi", "database.db");

            if (!Directory.Exists(Path.GetDirectoryName(dbFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(dbFilePath));
            }

            DbPath = dbFilePath;
        }
    }
}
