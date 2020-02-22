using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using BookOrganizer.Domain;

namespace BookOrganizer.UI.WPFCore.Wrappers
{
    public class BaseWrapper<T> : NotifyDataErrorInfo 
        where T : IIdentifiable
    {
        public BaseWrapper(T model)
        {
            Model = model;
        }

        public Guid Id { get { return Model.Id; } }

        public T Model { get; }

        protected virtual void SetValue<TValue>(TValue value, [CallerMemberName]string propertyName = null)
        {
            typeof(T).GetProperty(propertyName).SetValue(Model, value);
            OnPropertyChanged(propertyName);
            ValidatePropertyInternal(propertyName, value);
        }

        protected virtual TValue GetValue<TValue>([CallerMemberName]string propertyName = null) 
            => (TValue)typeof(T).GetProperty(propertyName).GetValue(Model);

        public dynamic ValidateDataAnnotations(string propertyName, object currentValue)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(Model) { MemberName = propertyName };
            Validator.TryValidateProperty(currentValue, context, results);

            foreach (var result in results)
            {
                AddError(propertyName, result.ErrorMessage);
            }

            switch (Type.GetTypeCode(GetType().GetProperty(propertyName).PropertyType))
            {
                case TypeCode.Int32:
                    return (int)currentValue;
                case TypeCode.String:
                    return (string)currentValue;
                default:
                    return currentValue;
            }
        }

        public void ValidatePropertyInternal(string propertyName, object currentValue)
        {
            ClearErrors(propertyName);

            ValidateDataAnnotations(propertyName, currentValue);

            ValidateCustomErrors(propertyName);
        }

        public void ValidateCustomErrors(string propertyName)
        {
            var errors = ValidateProperty(propertyName);
            if (errors != null)
            {
                foreach (var error in errors)
                {
                    AddError(propertyName, error);
                }
            }
        }

        protected virtual IEnumerable<string> ValidateProperty(string propertyName)
        {
            return null;
        }
    }
}
