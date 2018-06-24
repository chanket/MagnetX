using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MagnetX.Searcher.HistorySearcher
{
    static class Utils
    {
        private static string File { get; } = @"History.accdb";

        private static string ConnectString { get; } = @"Provider = Microsoft.Ace.OLEDB.12.0; Data Source = " + File + "; Jet OLEDB:Database Password=MAGNETX";

        public static FileInfo DatabaseFile { get { return new FileInfo(File); } }

        public static bool Init()
        {
            try
            {
                //Delete Existed
                if (DatabaseFile.Exists) DatabaseFile.Delete();

                //Release New
                System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
                string name = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + File;
                using (Stream inStream = asm.GetManifestResourceStream(name))
                using  (FileStream outStream = DatabaseFile.OpenWrite())
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = inStream.Read(buffer, 0, buffer.Length);
                    while (bytesRead != 0)
                    {
                        outStream.Write(buffer, 0, bytesRead);
                        bytesRead = inStream.Read(buffer, 0, buffer.Length);
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static OleDbConnection CreateConnection()
        {
            OleDbConnection conn = new OleDbConnection(ConnectString);
            return conn;
        }

        public static OleDbCommand BuildSearch(OleDbConnection conn, string[] words)
        {
            string baseCmd = @"SELECT `ID`,`Promote`,`Size` FROM `Data`";

            for (int i = 0; i < words.Length; i++)
            {
                if (i == 0) baseCmd += " WHERE `Promote` LIKE ?";
                else baseCmd += " AND `Promote` LIKE ?";
            }

            OleDbCommand cmd = new OleDbCommand(baseCmd);
            for (int i = 0; i < words.Length; i++)
            {
                cmd.Parameters.AddWithValue("?", "%" + words[i] + "%");
            }

            cmd.Connection = conn;
            return cmd;
        }

        public static OleDbCommand BuildInsert(OleDbConnection conn, Result result)
        {
            OleDbCommand cmd = new OleDbCommand(@"INSERT INTO `Data` (`ID`, `Promote`, `Size`) VALUES (?, ?, ?)");
            cmd.Parameters.AddWithValue("?", result.Magnet.Substring(20, 40));
            cmd.Parameters.AddWithValue("?", result.Name);
            cmd.Parameters.AddWithValue("?", result.Size);

            cmd.Connection = conn;
            return cmd;
        }

        public static OleDbCommand BuildDelete(OleDbConnection conn)
        {
            OleDbCommand cmd = new OleDbCommand(@"DELETE FROM `Data`");

            cmd.Connection = conn;
            return cmd;
        }
    }
}
