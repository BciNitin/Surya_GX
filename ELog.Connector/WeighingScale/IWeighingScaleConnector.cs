using ELog.Core.Hardware.WeighingMachine;

using System.Threading.Tasks;

namespace ELog.Connector
{
    public interface IWeighingScaleConnector
    {
        Task<double> GetWeight(WeighingMachineInput weighingMachineInput);
        Task<double> GetWeightForTesting(string ipAddress, int portNumber);

        Task<bool> IsWeighingMachineAvailable(WeighingMachineInput weighingMachineInput);
    }
}