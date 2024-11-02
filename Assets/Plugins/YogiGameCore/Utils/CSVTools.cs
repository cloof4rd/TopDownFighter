using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace YogiGameCore.Utils
{
    public class CSVTools
    {
        public static List<string> PopulateFromCSV(TextAsset _csv,int startRowIndex = 1)
        {
            var strArrayList = new List<string>();
            string[] strArray1 = _csv.text.Split('\n');
            for (var index = startRowIndex; index < strArray1.Length; ++index)
                if (!string.IsNullOrEmpty(strArray1[index]))
                {
                    var strArray2 = strArray1[index].TrimEnd('\r');
                    strArrayList.Add(strArray2);
                }

            if (strArrayList.Count > 0)
                return strArrayList;
            Debug.LogError("There was no data in this CSV: " + _csv.name);
            return null;
        }
        
    }
}