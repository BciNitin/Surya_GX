using Abp.Dependency;

using ELog.Connector;
using ELog.Connector.Weighing;

using static ELog.Core.PMMSEnums;

namespace ELog.ConnectorFactory
{
    public class WeighingScaleFactory
    {
        public IWeighingScaleConnector GetPrintConnector(WeighingScaleType weighingScaleType)
        {
            switch (weighingScaleType)
            {
                case WeighingScaleType.Normal:
                default:
                    return IocManager.Instance.ResolveAsDisposable<WeighingScale>().Object;
            }
        }
    }
}