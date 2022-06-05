namespace MissingChecker
{
    public abstract class BaseExporter : IExporter
    {
        protected BaseExporter()
        {
            _report = new Report();
            _report.Time = LocalStorageUtility.TimeAsString;
        }

        protected Report _report;

        public void Add(string assetPath)
        {
            _report.Results.Add(new CheckResult(assetPath));
        }

        public abstract void Export();

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
