using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions
{
    public sealed class ForbiddenAccessException : ApplicationException
    {
        public ForbiddenAccessException()
            : base("You do not have permission to perform this action.") { }
    }
}
