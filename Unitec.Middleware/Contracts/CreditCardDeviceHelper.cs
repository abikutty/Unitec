using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unitec.Middleware.Contracts
{
    public class PeripheralConnection
    {
        public string ComPort { get; private set; }
        public int BaudRate { get; private set; }
        public Parity Parity { get; private set; }
        public int DataBits { get; private set; }
        public StopBits StopBits { get; private set; }

        private string peripheralsConfigFile = "";
        public PeripheralConnection(string file)
        {
            peripheralsConfigFile = file;
            //TODO:- Read Config file to populate the fields

            var configFile = ConfigurationManager.OpenExeConfiguration(file);
            var settings = configFile.AppSettings.Settings;
            ComPort = settings["ComPort"].Value ?? "COM3";
            int temp = 0;
            BaudRate = int.TryParse(settings["BaudRate"].Value, out temp) ? temp : 38400;
            Parity = (Parity)(int.TryParse(settings["Parity"].Value, out temp) ? temp : 0);
            DataBits = int.TryParse(settings["DataBits"].Value, out temp) ? temp : 8;
            StopBits = (StopBits)(int.TryParse(settings["StopBits"].Value, out temp) ? temp : 1);
        }
    }

}
