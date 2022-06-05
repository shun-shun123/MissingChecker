using System;
using System.Collections.Generic;

namespace MissingChecker
{
    [Serializable]
    internal class ExecuteSetting 
    {
        internal List<string> CheckAssetPaths;

        internal List<string> CheckFileExtensions;
    }
}
