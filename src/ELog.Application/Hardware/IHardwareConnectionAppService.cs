using ELog.Application.Hardware.Dto;
using System.Threading.Tasks;

namespace ELog.Application.Hardware
{
    public interface IHardwareConnectionAppService
    {
        Task<bool> PrinterAvailable(Connection input);
        Task<bool> WeighingMachineAvailable(Connection input);
    }
}
