using System;

namespace MissingChecker
{
    [Serializable]
    public struct CheckResult
    {
        public string AssetPath;

        public string ComponentName;

        public CheckResult(string assetPath) : this(assetPath, "")
        {
        }

        public CheckResult(string assetPath, string componentName)
        {
            AssetPath = assetPath;
            ComponentName = componentName;
        }
    }
}
