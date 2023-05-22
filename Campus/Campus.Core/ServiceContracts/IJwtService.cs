using Campus.Core.DTO;
using Campus.Core.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Core.ServiceContracts
{
    public interface IJwtService
    {
        AuthorizationResponse GetJwt(ApplicationUser user);
    }
}
