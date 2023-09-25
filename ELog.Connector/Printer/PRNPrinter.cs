using ELog.Connector;
using ELog.Core.Printer;

using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ELog.HardwareConnector.Printer
{
    public class PRNPrinter : IPrinterConnector
    {
        public async Task<bool> Print(PrinterInput printerInput)
        {
            if (!string.IsNullOrEmpty(printerInput.IPAddress) && !string.IsNullOrEmpty(printerInput.PrintBody) && printerInput.Port > 0)
            {
                var tcpClient = new TcpClient();
                //ie: 10.0.0.91
                //ie: 6101
                try
                {
                    tcpClient.Connect(printerInput.IPAddress, printerInput.Port);
                }
                catch (Exception)
                {
                    //throw;
                    return false;
                }
                //Connect to Printer and throw exception if not connected.
                var writer = new StreamWriter(tcpClient.GetStream());
                writer.Write(printerInput.PrintBody);
                writer.Flush();
                writer.Close();
                tcpClient.Close();
                return true;
            }
            else
            {
                throw new Exception("IPAddress and PrintBody cannot be empty or Port should be greater than 0");
            }
        }

        public async Task<bool> PrinterAvailable(PrinterInput printerInput)
        {
            var tcpClient = new TcpClient();
            //ie: 10.0.0.91
            //ie: 6101
            try
            {
                tcpClient.Connect(printerInput.IPAddress, printerInput.Port);
                tcpClient.Close();
            }
            catch (Exception)
            {
                //throw;
                return false;
            }

            return true;

        }
    }
}