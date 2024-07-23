using Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Utils.Util;

namespace Models.Services.Interface
{
    public interface ITokenService
    {
        bool Authentication(string User, string Password);
    }
}
