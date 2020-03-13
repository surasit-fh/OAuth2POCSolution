using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth2POC.DAL.Repositories
{
    public abstract class Repositories<S> : IRepositories
    {
        public virtual List<S> GetAll<T>() { return null; }

        public virtual S GetById<T>(string Id) { return default(S); }

        public virtual List<S> GetByCriteria<T>(S modelInfo) { return null; }

        public virtual string Insert<T>(S modelInfo) { return string.Empty; }

        public virtual bool Update<T>(S modelInfo) { return default(bool); }

        public virtual bool Delete<T>(string Id) { return default(bool); }
    }
}