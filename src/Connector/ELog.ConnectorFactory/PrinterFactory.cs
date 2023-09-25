using Abp.Dependency;

using ELog.Connector;
using ELog.HardwareConnector.Printer;

using static ELog.Core.PMMSEnums;

namespace ELog.HardwareConnectorFactory
{
    public class PrinterFactory
    {
        public IPrinterConnector GetPrintConnector(PrinterType printerType)
        {
            switch (printerType)
            {
                case PrinterType.PRN:
                default:
                    return IocManager.Instance.ResolveAsDisposable<PRNPrinter>().Object;
            }
        }
    }
}