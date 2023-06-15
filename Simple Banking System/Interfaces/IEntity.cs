using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Banking_System.Interfaces
{
    public interface IEntity
    {
        Guid Id { get; }
        DateTime Created { get; }
        DateTime LastUpdated { get; }
    }
}
