using Entities.Core;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Utils.Util;

namespace Models.Services.Interface
{
    public interface IUserService
    {
        ResultOperation<List<User>> GetUser();
        ResultOperation<List<UserDTO>> GetUserData();
        ResultOperation<bool> PostUser(UserDTO UserDTO);
        ResultOperation<bool> PutUser(User user);
        ResultOperation<bool> DeleteUser(UserDeleteDTO userDeleteDTO);
        bool Login(UserLoginDTO userLoginDTO);
    }
}
