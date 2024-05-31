using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ParkingServis.Server.Database
{
    public class DatabaseConn
    {
        bool initializedOnce = false;
        string cofnigFileName = "NAConfig.app";
        public Dictionary<string, string> server_string = new Dictionary<string, string>();
        public DatabaseConn()
        {
            if (initializedOnce == false)
            {

                if (File.Exists(cofnigFileName))
                {

                    string[] _fileLines = File.ReadAllLines(cofnigFileName);
                    foreach (string _line in _fileLines)
                    {
                        string[] _splitedLine = _line.Split(',');
                        server_string.Add(_splitedLine[0], _splitedLine[1]);
                    }
                    initializedOnce = true;
                }
                else
                {
                    MessageBox.Show("Doslo je do greske. Missing configuration files.");
                }
            }
        }
    }
}
