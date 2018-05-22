namespace Touch.Services
{
    /// <summary>
    ///     Mark for Page with ViewModel
    /// </summary>
    /// <typeparam name="T">Type of ViewModel</typeparam>
    internal interface IPageWithViewModel<T> where T : class
    {
        /// <summary>
        ///     ViewModel for page view
        /// </summary>
        T ViewModel { get; set; }

        /// <summary>
        ///     For async loading, update bindings.
        /// </summary>
        void UpdateBindings();
    }
}