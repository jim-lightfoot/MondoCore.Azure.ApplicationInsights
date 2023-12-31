﻿/*************************************************************************** 
 *                                                                           
 *    The MondoCore Libraries  							                     
 *                                                                           
 *        Namespace: MondoCore.Common							             
 *             File: ObjectExtensions.cs					    		         
 *        Class(es): ObjectExtensions				         		             
 *          Purpose: Extensions for objects. Note: This code copied from the main MondoCore repository
 *                                                                           
 *  Original Author: Jim Lightfoot                                           
 *    Creation Date: 1 Jan 2020                                              
 *                                                                           
 *   Copyright (c) 2005-2023 - Jim Lightfoot, All rights reserved            
 *                                                                           
 *  Licensed under the MIT license:                                          
 *    http://www.opensource.org/licenses/mit-license.php                     
 *                                                                           
 ****************************************************************************/

using System.Collections.Generic;

namespace MondoCore.Azure.ApplicationInsights
{
    internal static class DictionaryExtensions
    {
        /****************************************************************************/
        internal static IDictionary<K, V> Merge<K, V>(this IDictionary<K, V> dict1, IDictionary<K, V> dict2)
        {
            if(dict2 == null || dict2.Count == 0)
                return dict1;
       
            if(dict1 == null)
                return dict2;
       
            foreach(var kv in dict2)
                dict1[kv.Key] = kv.Value;

            return dict1;
        }    
    }
}
