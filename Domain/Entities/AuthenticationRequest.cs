using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class AuthenticationRequest
    {
        public string UsernameOrEmail { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
