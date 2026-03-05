using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }


        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any()) 
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResult = await Task.WhenAll(_validators.Select(x => x.ValidateAsync(context, cancellationToken)));

                var failedResult = validationResult.SelectMany(x=> x.Errors)
                    .Where(x=> x != null)
                    .ToList();

                if (failedResult.Count != 0)
                {
                    throw new ValidationException(failedResult);
                    
                }
            }

            return await next();
        }
    }
}
