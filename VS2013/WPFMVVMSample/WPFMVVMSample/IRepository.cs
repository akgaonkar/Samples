using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFMVVMSample
{
    interface IRepository<AnyType>
    {
        void Add(AnyType type);
        void Update(AnyType type);
        void Delete(AnyType type);        
        void Save();
    }

    abstract class CommonDAL<AnyType> : IRepository<AnyType>
    {
        public string ConnectionString { get; set; }

        protected List<AnyType> typeList = new List<AnyType>();
        public CommonDAL(string connectionString)
        {
            ConnectionString = connectionString;
        }


        public void Add(AnyType type)
        {
            throw new NotImplementedException();
        }

        public void Update(AnyType type)
        {
            throw new NotImplementedException();
        }

        public void Delete(AnyType type)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
