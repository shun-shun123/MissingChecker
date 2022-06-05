using System;
using System.Collections.Generic;

namespace MissingChecker
{
    [Serializable]
    public class ExecuteSetting 
    {
        public List<string> CheckAssetPaths;

        public List<string> CheckFileExtensions;
    }
}
