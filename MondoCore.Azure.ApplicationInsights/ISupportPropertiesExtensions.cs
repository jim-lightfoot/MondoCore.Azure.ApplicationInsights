/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  	                                            
 *                                                                          
 *      Namespace: MondoCore.ApplicationInsights	                                        
 *           File: ISupportPropertiesExtensions.cs                                       
 *      Class(es): ISupportPropertiesExtensions                                          
 *        Purpose: Implementation of extensions for ISupportProperties                           
 *                                                                          
 *  Original Author: Jim Lightfoot                                          
 *    Creation Date: 4 Apr 2022                                            
 *                                                                          
 *   Copyright (c) 2022-2024 - Jim Lightfoot, All rights reserved           
 *                                                                          
 *  Licensed under the MIT license:                                         
 *    http://www.opensource.org/licenses/mit-license.php                    
 *                                                                          
 ****************************************************************************/

using Microsoft.ApplicationInsights.DataContracts;

using MondoCore.Collections;
using MondoCore.Log;

namespace MondoCore.Azure.ApplicationInsights
{
    internal static class ISupportPropertiesExtensions
    {
        internal static void AppendProperties(this ISupportProperties aiTelemetry, Telemetry telemetry, bool childrenAsJson)
        {
            var props = telemetry.Properties?.ToReadOnlyDictionary();

            if(props == null || props.Count == 0)
                return;

            aiTelemetry.Properties.AppendStrings(props, childrenAsJson, transformKey: (name)=> name.Capitalize() );
        }
    }

    internal static class Extensions
    {
        internal static string Capitalize(this string? str)
        {
            if(string.IsNullOrWhiteSpace(str))
                return "";

            if(str.Length == 1)
                return str.ToUpper();

            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }
    }
}
