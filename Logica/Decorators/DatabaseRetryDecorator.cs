using CSharpFunctionalExtensions;
using Logica.Utils;
using System;

namespace Logica.Decorators
{
    public sealed class DatabaseRetryDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _handler;
        private readonly Config _config;

        public DatabaseRetryDecorator(ICommandHandler<TCommand> handler, Config config)
        {
            _handler = handler;
            _config = config;
        }

        public Result Handle(TCommand command)
        {
            for (int i = 0; i < _config.NumeroDeTentativasNoBanco; i++)
            {
                try
                {
                    var result = _handler.Handle(command);
                    return result;
                }
                catch (Exception ex)
                {
                    if (i > _config.NumeroDeTentativasNoBanco || !IsDatabaseException(ex))
                        throw;
                }
            }

            throw new InvalidOperationException();
        }

        private bool IsDatabaseException(Exception ex)
        {
            var message = ex.InnerException?.Message;

            if (message == null)
                return false;

            return message.Contains("The connection is broken and recovery is not possible") || message.Contains("error occurred while establishing a connection");
        }
    }
}
