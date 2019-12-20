using CSharpFunctionalExtensions;
using Logica.Utils;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Logica.Decorators
{
    public sealed class AuditLoggingDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _handler;

        public AuditLoggingDecorator(ICommandHandler<TCommand> handler)
        {
            _handler = handler;
        }

        public Result Handle(TCommand command)
        {
            using (var stream = File.AppendText("log.txt"))
            {
                var now = DateTime.Now;
                var serializedLog = $"{now.ToString("dd/MM/yyyy")} - {now.ToString("HH:mm:ss")} - {JsonConvert.SerializeObject(command)}";
                stream.WriteLine(serializedLog);

                return _handler.Handle(command);
            }
        }
    }

}
