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

        internal List<IExporter> Exporters;

        public ExecuteRequest(
            IEnumerable<string> paths,
            IEnumerable<string> extensions = null,
            Action<string, bool> onChecked = null,
            Action onSuccess = null,
            Action<Exception> onException = null,
            List<IExporter> exporters = null)
        {
            Paths = paths.ToArray();
            Extensions = extensions.ToArray();
            OnChecked = onChecked;
            OnSuccess = onSuccess;
            OnException = onException;
            Exporters = exporters;
            if (Exporters == null)
            {
                Exporters = new List<IExporter>(new IExporter[] { JsonExporter.Default });
            }
            if (Exporters.All(i => (i as JsonExporter) == null))
            {
                Exporters.Add(JsonExporter.Default);
            }

            Validate();
        }

        private void Validate()
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
