using System;
using System.Collections.Generic;
using System.Text;

namespace Logica.Utils
{
    public interface IQuery<TResult> // Marker interface
    {

    }

    public interface IQueryHandler<TQuery, TResult>
       where TQuery : IQuery<TResult>
    {
        TResult Handle(TQuery query);
    }
}
