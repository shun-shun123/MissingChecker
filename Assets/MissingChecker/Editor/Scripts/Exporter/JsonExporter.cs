namespace MissingChecker
{
    public class JsonExporter : BaseExporter
    {
        public static JsonExporter Default => new JsonExporter();

        private JsonExporter() : base()
        { }

        public override void Export()
        {
            LocalStorageUtility.SaveAsJson(_report, _report.Title + _report.Time + ".json");
        }
    }
}
