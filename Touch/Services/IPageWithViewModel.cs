namespace Touch.Services
{
    internal interface IPageWithViewModel<T> where T : class
    {
        T ViewModel { get; set; }
    }
}