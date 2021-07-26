using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MLP.BAL.BaseObjects;
using MLP.DAL;

namespace MLP.BAL.Repositories
{
    public class AwardsTypesRepository : BaseRepository<AwardsType>
    {
        private MLPDB01Entities DataContext { get; set; }
        public AwardsTypesRepository(DbContext context)
            : base(context)
        {
            DataContext = (MLPDB01Entities)context;

        }
    }
}