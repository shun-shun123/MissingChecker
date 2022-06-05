using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MissingChecker
{
    [Serializable]
    internal class ExecuteSetting 
    {
        [SerializeField]
        internal List<string> CheckAssetPaths;

        [SerializeField]
        internal List<string> CheckFileExtensions;

        internal ExecuteSetting()
        {
            CheckAssetPaths = new List<string>();
            CheckFileExtensions = new List<string>();
        }

        internal bool IsValid()
        {
            foreach (var path in CheckAssetPaths)
            {
                if (!path.EndsWith("/"))
                {
                    return false;
                }
            }

            var extRegex = new Regex(@"^\.[a-z]*");
            foreach (var ext in CheckFileExtensions)
            {
                if (!extRegex.IsMatch(ext))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
