using Microsoft.Extensions.Options;
using NPOI.SS.Formula.Functions;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Helpers
{
    public class DatabaseBackupService
    {
        private readonly AppSettings _appSet;

        public DatabaseBackupService(IOptions<AppSettings> appSet)
        {
            _appSet = appSet!.Value!;
        }

        public void BackupDatabase()
        {

            var mysqldumpPath = _appSet.Database!.MySqlDumpPath;
            var backupDir = _appSet.Database.BackupDatabaseDir;
            var mysqlUser = _appSet.Database.UserDB;
            var mysqlPassword = _appSet.Database.PasswordDB;
            var mysqlDatabase = "SalesPipeline";

            var timestamp = DateTime.Now.ToString("yyyyMMdd");
            var yearstamp = DateTime.Now.ToString("yyyy");
            var backupPath = $@"{backupDir}\{yearstamp}";
            string databaseBackupName = $"{mysqlDatabase}_{timestamp}";

            // สร้างไดเรกทอรีสำรองข้อมูล
            Directory.CreateDirectory(backupPath);

            // คำสั่งสำหรับการสำรองข้อมูล
            // --single-transaction จะไม่ lock table ขณะทำการ backup
            var dumpCommand = $"\"{mysqldumpPath}\" --single-transaction -u {mysqlUser} -p{mysqlPassword} {mysqlDatabase} > \"{Path.Combine(backupPath, databaseBackupName + ".sql")}\"";

            // เรียกใช้งาน batch file
            var processInfo = new ProcessStartInfo("cmd.exe", $"/C \"{dumpCommand}\"")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process
            {
                StartInfo = processInfo
            };

            process.Start();

            process.StandardOutput.ReadToEnd();
            process.StandardError.ReadToEnd();

            process.WaitForExit();

        }

    }
}
