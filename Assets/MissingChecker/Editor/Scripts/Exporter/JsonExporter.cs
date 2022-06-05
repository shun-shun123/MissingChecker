namespace MissingChecker
{
    public class JsonExporter : IExporter
    {
        public static JsonExporter Default
        {
            get
            {
                return new JsonExporter();
            }
        }

        private JsonExporter()
        {
            _report = new Report();
            _report.Time = LocalStorageUtility.TimeAsString;
        }

        private Report _report;

        public void Export()
        {
            LocalStorageUtility.SaveAsJson(_report, _report.Title + _report.Time + ".json");
        }

        public void Reuse()
        {
            _report.Title = "";
            _report.Time = LocalStorageUtility.TimeAsString;
            _report.Results.Clear();
        }

        public void Add(string assetPath)
        {
            _report.Results.Add(new CheckResult(assetPath));
        }

        public void SetTitle(string title)
        {
            _report.Title = title;
        }
    }
}
