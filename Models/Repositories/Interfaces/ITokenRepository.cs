using Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Repositories.Interfaces
{
    public interface ITokenRepository
    {
        JwtUser GetUserByUserName(string User);
    }
}
