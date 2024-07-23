using Entities.Core;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Repositories.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetUser();
        List<UserDTO> GetUserData();
        bool PostUser(User user);
        int PostUserData(UserData userData);
        bool PutUser(User user);
        bool DeleteUser(int id);
        string GetUserByUsername(string username);
    }
}
