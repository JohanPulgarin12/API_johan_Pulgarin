using AutoMapper.Configuration;
using Entities.Core;
using Entities.DTO;
using Models.Dto;
using Models.Repositories;
using Models.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Utils.Util;

namespace Models.Services
{
    public class UserService : Service, IUserService
    {
        private ConfigurationSectionWebApi _config;

        public UserService(ConfigurationSectionWebApi config) : base(config.Repository)
        {
            this._config = config;
        }

        private string encodePass(string pass)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(pass));
        }

        private string decodePass(string pass)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(pass));
        }

        public ResultOperation<List<User>> GetUser()
        {
            ResultOperation<List<User>> result =
                WrapExecuteTrans<ResultOperation<List<User>>, UserRepository>((repo, uow) =>
                {
                    ResultOperation<List<User>> rst = new ResultOperation<List<User>>();

                    try
                    {
                        rst.Result = repo.GetUser();
                    }
                    catch (Exception err)
                    {
                        rst.stateOperation = false;
                        rst.RollBack = true;
                        rst.Result = null;
                        rst.MessageExceptionTechnical = err.Message + Environment.NewLine + err.StackTrace;
                    }

                    return rst;
                });

            return result;
        }

        public ResultOperation<List<UserDTO>> GetUserData()
        {
            ResultOperation<List<UserDTO>> result =
                WrapExecuteTrans<ResultOperation<List<UserDTO>>, UserRepository>((repo, uow) =>
                {
                    ResultOperation<List<UserDTO>> rst = new ResultOperation<List<UserDTO>>();

                    try
                    {
                        rst.Result = repo.GetUserData();
                    }
                    catch (Exception err)
                    {
                        rst.stateOperation = false;
                        rst.RollBack = true;
                        rst.Result = null;
                        rst.MessageExceptionTechnical = err.Message + Environment.NewLine + err.StackTrace;
                    }

                    return rst;
                });

            return result;
        }

        public ResultOperation<bool> PostUser(UserDTO userDTO)
        {
            ResultOperation<bool> result =
                WrapExecuteTrans<ResultOperation<bool>, UserRepository>((repo, uow) =>
                {
                    ResultOperation<bool> rst = new ResultOperation<bool>();

                    try
                    {
                        UserData userData = new UserData();
                        userData.DocNum = userDTO.DocNum;
                        userData.FirstName = userDTO.FirstName;
                        userData.LastName = userDTO.LastName;
                        userData.Telf = userDTO.Telf;
                        userData.Email = userDTO.Email;
                        var idResponse = repo.PostUserData(userData);
                        
                        if(idResponse != null)
                        {
                            User user = new User();
                            user.Username = userDTO.Username;
                            user.Pass = encodePass(userDTO.Pass);
                            user.Info = idResponse;
                            rst.Result = repo.PostUser(user);
                        }
                        else
                        {
                            rst.Result = false;
                        }
                    }
                    catch (Exception err)
                    {
                        rst.stateOperation = false;
                        rst.RollBack = true;
                        rst.Result = false;
                        rst.MessageExceptionTechnical = err.Message + Environment.NewLine + err.StackTrace;
                    }

                    return rst;
                });

            return result;
        }

        public ResultOperation<bool> PutUser(User user)
        {
            ResultOperation<bool> result =
                WrapExecuteTrans<ResultOperation<bool>, UserRepository>((repo, uow) =>
                {
                    ResultOperation<bool> rst = new ResultOperation<bool>();

                    try
                    {
                        user.Pass = encodePass(user.Pass);
                        rst.Result = repo.PutUser(user);
                    }
                    catch (Exception err)
                    {
                        rst.stateOperation = false;
                        rst.RollBack = true;
                        rst.Result = false;
                        rst.MessageExceptionTechnical = err.Message + Environment.NewLine + err.StackTrace;
                    }

                    return rst;
                });

            return result;
        }

        public ResultOperation<bool> DeleteUser(UserDeleteDTO userDeleteDTO)
        {
            ResultOperation<bool> result =
                WrapExecuteTrans<ResultOperation<bool>, UserRepository>((repo, uow) =>
                {
                    ResultOperation<bool> rst = new ResultOperation<bool>();

                    try
                    {
                        rst.Result = repo.DeleteUser(userDeleteDTO.Id);
                    }
                    catch (Exception err)
                    {
                        rst.stateOperation = false;
                        rst.RollBack = true;
                        rst.Result = false;
                        rst.MessageExceptionTechnical = err.Message + Environment.NewLine + err.StackTrace;
                    }

                    return rst;
                });

            return result;
        }

        private ResultOperation<string> GetUserByUsername(string username)
        {
            ResultOperation<string> result =
                WrapExecuteTrans<ResultOperation<string>, UserRepository>((repo, uow) =>
                {
                    ResultOperation<string> rst = new ResultOperation<string>();

                    try
                    {
                        rst.Result = repo.GetUserByUsername(username);
                    }
                    catch (Exception err)
                    {
                        rst.stateOperation = false;
                        rst.RollBack = true;
                        rst.Result = null;
                        rst.MessageExceptionTechnical = err.Message + Environment.NewLine + err.StackTrace;
                    }

                    return rst;
                });

            return result;
        }

        private bool VerifyPass(string password, string enPassword)
        {
            var encodePass = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
            if (encodePass == enPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Login(UserLoginDTO userLoginDTO)
        {
            var userPass = GetUserByUsername(userLoginDTO.Username).Result;

            if (userPass != null && VerifyPass(userLoginDTO.Pass, userPass))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
