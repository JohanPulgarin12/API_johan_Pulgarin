using Dapper;
using Models.Dto;
using Models.Repositories._UnitOfWork;
using Models.Repositories.Interfaces;

namespace Models.Repositories
{
    public class TokenRepository : Repository, ITokenRepository
    {
        public TokenRepository()
        {

        }

        public TokenRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public JwtUser GetUserByUserName(string User)
        {
            DynamicParameters prms = new DynamicParameters();
            prms.Add("User", User);
            string sql = @"select Login, Password, Salt from Usuarios where Login = @User";
            return Get<JwtUser>(sql, prms);
        }
    }
}
