using System;
using System.Collections.Generic;

using Microsoft.ApplicationInsights.DataContracts;

using MondoCore.Log;

namespace MondoCore.Azure.ApplicationInsights
{
    internal static class ISupportPropertiesExtensions
    {
        internal static void MergeProperties(this ISupportProperties aiTelemetry, Telemetry telemetry, bool childrenAsJson)
        {
            var props = telemetry.Properties?.ToStringDictionary(childrenAsJson);

            if(props == null || props.Count == 0)
                return;

            aiTelemetry.Properties.Merge(props);
        }
    }
}
