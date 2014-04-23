using System.ComponentModel.DataAnnotations;

namespace Trex.SmartClient.Core.Interfaces
{
    /// <summary>
    /// Interface for validation rules used directly from viewmodels. 
    /// (Not to be confused with other validation-strategy: ValidationRule)
    /// This utilizes the IDataErrorInfo WPF validation framework.   
    /// Works with bindings by using ValidatesOnDataErrors=True
    /// </summary>
    public interface IViewModelValidationRule
    {
        ValidationResult ValidateViewModel(IViewModel viewModel, string propertyName, object value);
    }
}