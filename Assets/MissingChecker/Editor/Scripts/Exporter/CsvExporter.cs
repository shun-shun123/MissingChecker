using System.Text;

namespace MissingChecker
{
    public class CsvExporter : IExporter
    {
        public static CsvExporter Default => new CsvExporter();

        internal CsvExporter()
        {
            _report = new Report();
        }

        private Report _report;

        public void Add(string assetPath)
        {
            _report.Results.Add(new CheckResult(assetPath));
        }

        public void Export()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(_report.Title);
            builder.Append(',');
            builder.Append(_report.Time);
            builder.Append('\n');
            builder.Append("AssetPath, ComponentName\n");
            for (var i = 0; i < _report.Results.Count; i++)
            {
                builder.Append(_report.Results[i].AssetPath);
                builder.Append(',');
                builder.Append(_report.Results[i].ComponentName);
                if (i != _report.Results.Count - 1)
                {
                    builder.Append(',');
                    builder.Append('\n');
                }
            }
            LocalStorageUtility.Save(builder.ToString(), _report.Title + _report.Time + ".csv");
        }

        public void Reuse()
        {
            _report.Title = "";
            _report.Time = LocalStorageUtility.TimeAsString;
            _report.Results.Clear();
        }

        public void SetTitle(string title)
        {
            _report.Title = title;
        }
    }
}
