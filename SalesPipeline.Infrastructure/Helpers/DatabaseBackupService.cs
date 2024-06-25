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
		private string mysqldumpPath = @"C:\Program Files\MariaDB 10.11\bin\mysqldump.exe";
		private string backupDir = @"C:\DataRM\backups\database";
		private string mysqlUser = "SA";
		private string mysqlPassword = "Ibusiness02";
		private string mysqlDatabase = "SalesPipeline";

		private readonly AppSettings _appSet;

		public DatabaseBackupService(IOptions<AppSettings> appSet)
		{
			_appSet = appSet.Value;
		}

		public void BackupDatabase()
		{
			if (_appSet != null && _appSet.Database != null)
			{
				backupDir = _appSet.Database.BackupDatabaseDir;
				mysqldumpPath = _appSet.Database.MySqlDumpPath;
				mysqlUser = _appSet.Database.UserDB;
				mysqlPassword = _appSet.Database.PasswordDB;
			}

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

			string output = process.StandardOutput.ReadToEnd();
			string error = process.StandardError.ReadToEnd();

			process.WaitForExit();

			if (process.ExitCode != 0)
			{
				//throw new ExceptionCustom($"Error: {error}");
			}
			//else
			//{
			//	Console.WriteLine("Backup completed successfully.");
			//}
		}

	}
}
