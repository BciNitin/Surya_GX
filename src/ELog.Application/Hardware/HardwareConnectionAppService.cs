using Abp.Application.Services;

using ELog.Application.Hardware.Dto;
using ELog.ConnectorFactory;
using ELog.HardwareConnectorFactory;
using System.Threading.Tasks;

namespace ELog.Application.Hardware
{
    public class HardwareConnectionAppService : ApplicationService, IHardwareConnectionAppService
    {
        private readonly PrinterFactory _printerFactory;
        private readonly WeighingScaleFactory _weighingScaleFactory;

        public HardwareConnectionAppService(PrinterFactory printerFactory, WeighingScaleFactory weighingScaleFactory)
        {
            _printerFactory = printerFactory;
            _weighingScaleFactory = weighingScaleFactory;
        }

        public Task<bool> PrinterAvailable(Connection input)
        {
            var prnPrinter = _printerFactory.GetPrintConnector(Core.PMMSEnums.PrinterType.PRN);
            var res = prnPrinter.PrinterAvailable(new Core.Printer.PrinterInput
            {
                IPAddress = input.IPAddress,
                Port = input.Port
            });
            return res;
        }

        public Task<bool> WeighingMachineAvailable(Connection input)
        {
            var weighingScale = _weighingScaleFactory.GetPrintConnector(Core.PMMSEnums.WeighingScaleType.Normal);
            var res = weighingScale.IsWeighingMachineAvailable(new Core.Hardware.WeighingMachine.WeighingMachineInput
            {
                IPAddress = input.IPAddress,
                Port = input.Port
            });
            return res;
        }
    }
}
