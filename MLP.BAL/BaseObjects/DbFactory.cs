using MLP.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL
{
    public interface IDbFactory
    {
        MLPDB01Entities Init();
    }

    public class DbFactory : IDbFactory
    {
        private MLPDB01Entities dataContext;

        public MLPDB01Entities Init()
        {
            return dataContext ?? (dataContext = new MLPDB01Entities());
        }


    }
}
