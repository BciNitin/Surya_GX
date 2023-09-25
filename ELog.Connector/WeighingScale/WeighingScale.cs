using ELog.Core.Hardware.WeighingMachine;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ELog.Connector.Weighing
{
    public class WeighingScale : IWeighingScaleConnector
    {
        private readonly ILogger<WeighingScale> _logger;

        public WeighingScale(ILogger<WeighingScale> logger)
        {
            _logger = logger;
        }
        public Task<double> GetWeight(WeighingMachineInput weighingMachineInput)
        {
            var weight = 0.0;
            var tcpClient = new TcpClient();
            try
            {
                Task t = Task.Run(() =>
                {
                    tcpClient.Connect(weighingMachineInput.IPAddress, weighingMachineInput.Port);
                    Task.Delay(8000).Wait();
                });
                TimeSpan ts = TimeSpan.FromMilliseconds(5000);
                t.Wait(ts);

                // use the ipaddress as in the server program
                var stm = tcpClient.GetStream();

                var weightInBytes = new byte[2048];

                // Read Stream data
                stm.Read(weightInBytes, 0, 2048);

                //Convert bytes to string to get actual weight
                var res = Encoding.UTF8.GetString(weightInBytes, 0, weightInBytes.Length);

                res = res.Replace("\0", string.Empty);
                res = res.Replace("    ", ",");
                res = res.Replace("   ", ",");
                res = res.Replace("  ", ",");
                string[] lines;
                if (res.Contains(","))
                {
                    lines = res.Split(",", StringSplitOptions.None);
                }
                else
                {
                    lines = res.Split("\r\n", StringSplitOptions.None);
                }
                var filtered = lines.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

                tcpClient.Close();

                var weightReceived = filtered.Length > 0 ? filtered[filtered.Length - 1] : filtered[0];

                weight = Convert.ToDouble(weightReceived);

                return Task.FromResult(weight);
            }
            catch (Exception)
            {
                //throw ex;
                return Task.FromResult(0.0);
            }
        }
        public Task<double> GetWeightForTesting(string IPAddress, int Port)
        {
            var weight = 0.0;
            var tcpClient = new TcpClient();
            try
            {
                Task t = Task.Run(() =>
                {
                    tcpClient.Connect(IPAddress, Port);
                    Task.Delay(8000).Wait();
                });
                TimeSpan ts = TimeSpan.FromMilliseconds(5000);
                t.Wait(ts);

                // use the ipaddress as in the server program
                var stm = tcpClient.GetStream();

                var weightInBytes = new byte[2048];

                // Read Stream data
                stm.Read(weightInBytes, 0, 2048);

                //Convert bytes to string to get actual weight
                var res = Encoding.UTF8.GetString(weightInBytes, 0, weightInBytes.Length);

                res = res.Replace("\0", string.Empty);
                res = res.Replace("    ", ",");
                string[] lines;
                if (res.Contains(","))
                {
                    lines = res.Split(",", StringSplitOptions.None);
                }
                else
                {
                    lines = res.Split("\r\n", StringSplitOptions.None);
                }
                var filtered = lines.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

                tcpClient.Close();

                var weightReceived = filtered.Length > 0 ? filtered[filtered.Length - 1] : filtered[0];

                weight = Convert.ToDouble(weightReceived);

                return Task.FromResult(weight);

            }
            catch (Exception)
            {

                // throw ex;
                return Task.FromResult(0.0);
            }
        }

        public Task<bool> IsWeighingMachineAvailable(WeighingMachineInput weighingMachineInput)
        {
            try
            {
                var tcpClient = new TcpClient();
                tcpClient.Connect(weighingMachineInput.IPAddress, weighingMachineInput.Port);
                tcpClient.Close();
                return Task.FromResult(true);
            }
            catch (Exception)
            {
                //throw;
                return Task.FromResult(false);
            }
        }
    }
}