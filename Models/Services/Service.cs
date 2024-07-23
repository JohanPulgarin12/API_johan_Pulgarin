using Models.Context;
using Models.Repositories._UnitOfWork;
using Models.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Utils.Util;

namespace Models.Services
{
    public class Service
    {
        private string _Cnx = null;
        public Service(string BaseCnx) { this._Cnx = BaseCnx; }

        protected TResult WrapExecute<TResult, TRepository>(Func<TRepository, IUnitOfWork, TResult> exWp)
        where TRepository : IRepository, new()
        where TResult : ResultOperation, new()
        {
            TResult result = new TResult();
            using (var ctx = new ContextFactory(_Cnx))
            {
                try
                {
                    var repo = new TRepository();
                    repo.SetUnitOfWork(ctx.UnitOfWork);

                    result = exWp(repo, ctx.UnitOfWork);

                }
                catch (Exception ex)
                {
                    result.MessageExceptionTechnical = ex.ToString();
                    result.stateOperation = false;
                }
            }
            return result;
        }


        protected TResult WrapExecuteTrans<TResult, TRepository>(Func<TRepository, IUnitOfWork, TResult> exWp)
        where TRepository : IRepository, new()
        where TResult : ResultOperation, new()
        {
            TResult result = new TResult();
            using (var ctx = new ContextFactory(_Cnx))
            {
                try
                {
                    //ctx.UnitOfWork.Begin();

                    var repo = new TRepository();
                    repo.SetUnitOfWork(ctx.UnitOfWork);

                    result = exWp(repo, ctx.UnitOfWork);
                    if (result.RollBack) { throw new Exception(); }
                    //ctx.UnitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    ctx.UnitOfWork.Rollback();
                    result.MessageExceptionTechnical = ex.ToString();
                    result.stateOperation = false;
                }
            }
            return result;
        }
    }
}
