using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Core.Implemented
{
    public abstract class ValidationEnabledViewModelBase : ViewModelBase, IDataErrorInfo
    {
        private readonly Dictionary<string, string> _validatedProperties = new Dictionary<string, string>();
        private string _validationMessage;

        private string _errorText;
        [NoDirtyCheck]
        public string ErrorText
        {
            get
            {
                return _errorText;
            }
            set
            {
                _errorText = value;
                if (value != null)
                    OnPropertyChanged(() => ErrorText);
            }
        }

        private bool _hasErrors;
        [NoDirtyCheck]
        public bool HasErrors
        {
            get
            {
                return _hasErrors;
            }
            set
            {
                _hasErrors = value;
                OnPropertyChanged(() => HasErrors);
            }
        }

        protected ValidationEnabledViewModelBase()
        {
            ValidationRules = new Dictionary<string, List<IViewModelValidationRule>>();
            PropertyChanged += LocalViewModelBase_PropertyChanged;
        }

        private void LocalViewModelBase_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "ErrorText")
            {
                ErrorText = _validationMessage;
            }
        }

        public void ValidateProperty<TResult>(Expression<Func<TResult>> propertyExpression)
        {
            var memberExpr = propertyExpression.Body as MemberExpression;
            if (memberExpr == null) 
                return;

            var propertyName = memberExpr.Member.Name;
            OnPropertyChanged(() => propertyName);         
        }

        protected void AddValidationRule<TResult>(Expression<Func<TResult>> propertyExpression, IViewModelValidationRule rule)
        {
            if (propertyExpression.Body.NodeType == ExpressionType.MemberAccess)
            {
                var memberExpr = propertyExpression.Body as MemberExpression;
                if (memberExpr != null)
                {
                    string propertyName = memberExpr.Member.Name;
                    AddValidationRule(propertyName, rule);
                }
                else
                {
                    throw new Exception("Property not found");
                }
            }            
        }

        private void AddValidationRule(string propertyName, IViewModelValidationRule rule)
        {
            if (ValidationRules.ContainsKey(propertyName))
            {
                ValidationRules[propertyName].Add(rule);
            }
            else
            {
                ValidationRules.Add(propertyName, new List<IViewModelValidationRule> { rule });
            }

        }

        [NoDirtyCheck]
        protected Dictionary<string, List<IViewModelValidationRule>> ValidationRules
        {
            get;
            set;
        }

        [NoDirtyCheck]
        protected bool UseValidation
        {
            get;
            set;
        }

        public string ShowOnScreen()
        {
            var sb = new StringBuilder();
            if (ValidationResults != null)
            {
                foreach (ValidationResult validationResult in ValidationResults)
                {
                    sb.AppendLine((validationResult.ErrorMessage));
                    if (validationResult.MemberNames != null && validationResult.MemberNames.Count() > 1)
                        foreach (var memberName in validationResult.MemberNames)
                        {
                            sb.AppendLine("\t " + memberName);
                        }
                }
            }
            return sb.ToString();
        }

        public virtual bool IsValid()
        {

            var validationContext = new ValidationContext(this, null, null);
            ValidationResults = new List<ValidationResult>();
            Validator.TryValidateObject(this, validationContext, ValidationResults, true);

            if (ValidationRules.Count > 0)
            {
                foreach (KeyValuePair<string, List<IViewModelValidationRule>> validationRule in ValidationRules)
                {
                    string propertyName = validationRule.Key;

                    var prop = GetType().GetProperty(propertyName);
                    var value = prop.GetValue(this, null);
                    foreach (IViewModelValidationRule innerRule in validationRule.Value)
                    {
                        var returnResult = innerRule.ValidateViewModel(this, propertyName, value);
                        if (returnResult != null)
                        {
                            ValidationResults.Add(returnResult);
                        }                     
                    }
                }
            }

            return ValidationResults.Count <= 0;
        }

        [NoDirtyCheck]
        public List<ValidationResult> ValidationResults
        {
            get;
            set;
        }

        public string this[string columnName]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(columnName))
                    return string.Empty;


                var prop = GetType().GetProperty(columnName);
                var validationMap = prop
                    .GetCustomAttributes(typeof(ValidationAttribute), true)
                    .Cast<ValidationAttribute>();

                UpdateValidatedProperties(columnName);

                foreach (var v in validationMap)
                {
                    try
                    {
                        var val = prop.GetValue(this, null);
                        var validationContext = new ValidationContext(this, null, null) {MemberName = columnName};

                        v.Validate(val, validationContext);
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }

                if (ValidationRules.ContainsKey(columnName))
                {
                    var rules = ValidationRules[columnName];
                    foreach (IViewModelValidationRule validationRule in rules)
                    {
                        try
                        {
                            var returnValue = validationRule.ValidateViewModel(this as IViewModel, columnName, prop.GetValue(this, null));
                            UpdateValidatedProperties(columnName);
                            if (returnValue != null)
                            {
                                return returnValue.ErrorMessage;
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                     
                    }
                }
                return null;
            }
        }



        private void UpdateValidatedProperties(string propName)
        {
            IsValid();

            foreach (var valResult in ValidationResults.Where(x => x.MemberNames.Contains(propName)))
            {
                foreach (string mn in valResult.MemberNames.Where(x => x == propName))
                {
                    if (!_validatedProperties.ContainsKey(mn))
                    {
                        _validatedProperties.Add(mn, valResult.ErrorMessage);
                    }
                }
            }

            IList<string> members = ValidationResults.SelectMany(result => result.MemberNames).ToList();
            var toBeRemoved = false;

            foreach (var pName in _validatedProperties.Keys)
            {
                if (pName == propName)
                {
                    if (!members.Contains(pName))
                    {
                        toBeRemoved = true;
                    }
                }
            }

            if (toBeRemoved)
            {
                _validatedProperties.Remove(propName);
            }

            string message = String.Join("\n", _validatedProperties.Values);

            _validationMessage = message;
            HasErrors = _validatedProperties.Count > 0;
        }


        public string Error
        {
            get { return this[string.Empty]; }
        }
    }
}