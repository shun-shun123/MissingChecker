namespace MissingChecker
{
    public interface IExporter
    {
        void SetTitle(string title);

        void Add(string assetPath);

        void Export();

        void Reuse();
    }
}
