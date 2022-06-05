using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MissingChecker
{
    public class ExecuteRequest
    {
        internal string[] Paths;

        internal string[] Extensions;

        internal Action<string, bool> OnChecked;

        internal Action OnSuccess;

        internal Action<Exception> OnException;

        internal List<BaseExporter> Exporters;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="paths">all paths to check missing properties</param>
        /// <param name="extensions">all extensions to check missing properties <remark>If empty, check all assets specified in path</remark></param>
        /// <param name="onChecked">onChecked asset callback passed assetPath and hasMissingProperty</param>
        /// <param name="onSuccess">success callback</param>
        /// <param name="onException">exception callback</param>
        /// <param name="exporters">exporters which determine which format to export</param>
        public ExecuteRequest(
            IEnumerable<string> paths,
            IEnumerable<string> extensions = null,
            Action<string, bool> onChecked = null,
            Action onSuccess = null,
            Action<Exception> onException = null,
            List<BaseExporter> exporters = null)
        {
            Paths = paths.ToArray();
            Extensions = extensions.ToArray();
            OnChecked = onChecked;
            OnSuccess = onSuccess;
            OnException = onException;
            Exporters = exporters;
            if (Exporters == null)
            {
                Exporters = new List<BaseExporter>(new BaseExporter[] { JsonExporter.Default });
            }
            if (Exporters.All(i => (i as JsonExporter) == null))
            {
                Exporters.Add(JsonExporter.Default);
            }

            Validate();
        }

        internal ExecuteRequest(ExecuteSetting setting) : this(
            setting.CheckAssetPaths,
            setting.CheckFileExtensions)
        {

        }

        private void Validate()
        {
            {
                foreach (var path in Paths)
                {
                    if (!path.EndsWith("/"))
                    {
                        throw new Exception($"path must be finished by \'/\'");
                    }
                }
            }

            {
                var regex = new Regex(@"^\.[a-z]*");
                foreach (var ext in Extensions)
                {
                    if (!regex.IsMatch(ext))
                    {
                        throw new Exception($"extension must be \"^\\.[a-z]*\" format");
                    }
                }
            }
        }
    }
}
