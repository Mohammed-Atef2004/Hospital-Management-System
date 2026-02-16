using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions
{
    public sealed class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object key)
            : base($"{name} with key '{key}' was not found.") { }
    }
}
