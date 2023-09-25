using System.Threading.Tasks;

namespace ELog.Connector
{
    public interface IScannerConnector
    {
        Task Scan();
    }
}