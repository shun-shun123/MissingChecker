namespace MissingChecker
{
    internal interface IExporter
    {
        /// <summary>
        /// set title
        /// </summary>
        /// <param name="title">title</param>
        void SetTitle(string title);

        /// <summary>
        /// called when found asset which has missing properties
        /// </summary>
        /// <param name="assetPath">asset path</param>
        void Add(string assetPath);

        /// <summary>
        /// called when export the checked result
        /// </summary>
        void Export();

        /// <summary>
        /// called when finished one export process in order to reuse same instance
        /// </summary>
        void Reuse();
    }
}
