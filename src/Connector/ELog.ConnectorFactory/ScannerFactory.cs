using Abp.Dependency;

using ELog.Connector;
using ELog.Connector.Scanner;

using static ELog.Core.PMMSEnums;

namespace ELog.ConnectorFactory
{
    public class ScannerFactory
    {
        public IScannerConnector GetPrintConnector(ScannerType scannerType)
        {
            switch (scannerType)
            {
                case ScannerType.Barcode:
                default:
                    return IocManager.Instance.ResolveAsDisposable<BarcodeScanner>().Object;
            }
        }
    }
}