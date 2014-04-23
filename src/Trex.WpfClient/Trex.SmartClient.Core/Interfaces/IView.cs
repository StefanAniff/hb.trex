namespace Trex.SmartClient.Core.Interfaces
{
    public interface IView
    {
        void ApplyViewModel(IViewModel viewModel);
        object DataContext { get; set; }
    }
}