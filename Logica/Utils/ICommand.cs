using CSharpFunctionalExtensions;

namespace Logica.Utils
{
    public interface ICommand // Marker interface
    {

    }

    public interface ICommandHandler<TCommand>
     where TCommand : ICommand
    {
        Result Handle(TCommand command);
    }
}
