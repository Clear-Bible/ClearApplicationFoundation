using System;
using System.ComponentModel;
using System.Linq;
using Autofac;
using Caliburn.Micro;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClearApplicationFoundation.ViewModels.Infrastructure;

public abstract class ValidatingApplicationScreen<TEntity> : ApplicationScreen, IDataErrorInfo
{
    public IValidator<TEntity>? Validator { get; }

    private ValidationResult? _validationResult;
    public ValidationResult? ValidationResult
    {
        get => _validationResult;
        set => Set(ref _validationResult, value);
    }

    protected ValidatingApplicationScreen(INavigationService? navigationService, ILogger? logger,  IEventAggregator? eventAggregator, IMediator? mediator, ILifetimeScope? lifetimeScope,IValidator<TEntity>? validator) :
        base(navigationService, logger,  eventAggregator, mediator, lifetimeScope)
    {
        Validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public  string Error
    {
        get
        {

            ValidationResult = Validate();
            if (ValidationResult != null && ValidationResult.Errors.Any())
            {
                var errors = string.Join(Environment.NewLine, ValidationResult.Errors.Select(x => x.ErrorMessage).ToArray());
                return errors;
            }
            return string.Empty;
        }
    }

    public  string this[string columnName]
    {
        get
        {
            var emptyString = string.Empty;

            ValidationResult = Validate();

            if (ValidationResult != null)
            {
                var firstOrDefault = ValidationResult.Errors
                    .FirstOrDefault(validationFailure => validationFailure.PropertyName == columnName);
                if (firstOrDefault != null)
                {
                    return Validator != null ? firstOrDefault.ErrorMessage : emptyString;
                }
            }
               
            return emptyString;

        }
    }

    protected abstract ValidationResult? Validate();
 
}