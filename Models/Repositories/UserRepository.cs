using Dapper;
using Entities.Core;
using Models.Dto;
using Models.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Repositories
{
    public class UserRepository : Repository, IUserRepository
    {
        public List<User> GetUser()
        {
            var response = GetDataListOfProcedure<User>("SpVt_GetUser");
            return response;
        }

        public List<UserDTO> GetUserData()
        {
            var sql = @"SELECT U.Username, U.Pass, UD.FirstName, UD.LastName, UD.DocNum, UD.Telf, UD.Email
                        FROM UserLogin U
                        INNER JOIN UserData UD ON UD.Id = U.Info";
            var response = GetList<UserDTO>(sql);
            return response;
        }

        public string GetUserByUsername(string username)
        {
            DynamicParameters prms = new DynamicParameters();
            prms.Add("Username", username);
            string sql = @"SELECT Pass FROM UserLogin WHERE Username = @Username";
            var response = Get<string>(sql, prms);
            return response;
        }

        public int PostUserData(UserData userData)
        {
            DynamicParameters prms = new DynamicParameters();
            prms.Add("DocNum", userData.DocNum);
            prms.Add("FirstName", userData.FirstName);
            prms.Add("LastName", userData.LastName);
            prms.Add("Telf", userData.Telf);
            prms.Add("Email", userData.Email);
            string sql = @"INSERT INTO UserData(DocNum, FirstName, LastName, Telf, Email) VALUES(@DocNum, @FirstName, @LastName, @Telf, @Email)
                            SELECT @@IDENTITY";
            var response = Get<int>(sql, prms);
            return response;
        }
        public bool PostUser(User user)
        {
            DynamicParameters prms = new DynamicParameters();
            prms.Add("Username", user.Username);
            prms.Add("Pass", user.Pass);
            prms.Add("Info", user.Info);
            string sql = @"INSERT INTO UserLogin(Username, Pass, Info) VALUES(@Username, @Pass, @Info)";
            var response = Execute<bool>(sql, prms) == 1 ? true : false;
            return response;
        }

        public bool PutUser(User user)
        {
            DynamicParameters prms = new DynamicParameters();
            prms.Add("Id", user.Id);
            prms.Add("Username", user.Username);
            prms.Add("Pass", user.Pass);
            string sql = @"UPDATE UserLogin
                            SET Username = @Username,
                                Pass = @Pass
                            WHERE Id = @Id";

            var response = Execute<bool>(sql, prms) == 1 ? true : false;
            return response;
        }

        public bool PutUserData(UserData userData)
        {
            DynamicParameters prms = new DynamicParameters();
            prms.Add("DocNum", userData.DocNum);
            prms.Add("FirstName", userData.FirstName);
            prms.Add("LastName", userData.LastName);
            prms.Add("Telf", userData.Telf);
            prms.Add("Email", userData.Email);
            string sql = @"INSERT INTO UserData(DocNum, Firstname, LastName, Telf, Email) VALUES(@DocNum, FirstName, LastName, Telf, Email)";
            var response = Execute<bool>(sql, prms) == 1 ? true : false;
            return response;
        }

        public bool DeleteUser(int id)
        {
            DynamicParameters prms = new DynamicParameters();
            prms.Add("Id", id);
            string sql = @"DELETE FROM UserLogin WHERE Id = @Id";
            var response = Execute<bool>(sql, prms) == 1 ? true : false;
            return response;
        }
    }
}
