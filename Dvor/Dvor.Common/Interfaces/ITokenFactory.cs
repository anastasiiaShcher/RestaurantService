using System.Collections.Generic;
using System.Security.Claims;

namespace Dvor.Common.Interfaces
{
    public interface ITokenFactory
    {
        string Create(IList<Claim> claims);
    }
}