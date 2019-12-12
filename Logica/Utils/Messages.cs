using CSharpFunctionalExtensions;
using System;

namespace Logica.Utils
{
    public sealed class Messages
    {
        private readonly IServiceProvider _serviceProvider;

        public Messages(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Result Dispatch(ICommand command)
        {
            var type = typeof(ICommandHandler<>);
            Type[] typeArgs = { command.GetType() };

            var handlerType = type.MakeGenericType(typeArgs);

            var handler = (dynamic)_serviceProvider.GetService(handlerType);

            var result = (Result)handler.Handle((dynamic)command);

            return result;
        }


        public TResult Dispatch<TResult>(IQuery<TResult> query)
        {
            var type = typeof(IQueryHandler<,>);

            Type[] typeArgs = { query.GetType(), typeof(TResult) };
            var handlerType = type.MakeGenericType(typeArgs);

            var handler = (dynamic)_serviceProvider.GetService(handlerType);
            var result = (TResult)handler.Handle((dynamic)query);

            return result;
        }
    }
}
