using ELog.Core.Printer;

using System.Threading.Tasks;

namespace ELog.Connector
{
    public interface IPrinterConnector
    {
        Task<bool> Print(PrinterInput printerInput);
        Task<bool> PrinterAvailable(PrinterInput printerInput);
    }
}