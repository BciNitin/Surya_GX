using ELog.Core;

using Serilog.Core;
using Serilog.Events;

using System.Collections.Generic;

namespace ELog.Web.Core.Auditing
{
    public class RemovePropertyBagEnricher : ILogEventEnricher
    {
        private readonly List<string> _properties;

        internal static readonly List<string> propertyNames = new List<string>
        {
            PMMSConsts.LogPropertyActionId,
            PMMSConsts.LogPropertyActionName,
            PMMSConsts.LogPropertyRequestPath,
            PMMSConsts.LogPropertySourceContext,
            PMMSConsts.LogPropertySpanId,
            PMMSConsts.LogPropertyTraceId,
            PMMSConsts.LogPropertyParentId,
            PMMSConsts.LogPropertyConnectionId,

        };

        /// <summary>
        /// Creates a new <see cref="PropertyBagEnricher" /> instance.
        /// </summary>
        public RemovePropertyBagEnricher()
        {
            _properties = propertyNames;// new Dictionary<string, Tuple<object, bool>>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Enriches the <paramref name="logEvent" /> using the values from the property bag.
        /// </summary>
        /// <param name="logEvent">The log event to enrich.</param>
        /// <param name="propertyFactory">The factory used to create the property.</param>
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            foreach (string prop in _properties)
            {
                logEvent.RemovePropertyIfPresent(prop);
            }
        }
    }
}
