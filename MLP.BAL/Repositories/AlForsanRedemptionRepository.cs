using MLP.BAL.BaseObjects;
using MLP.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.Repositories
{
    public class AlForsanRedemptionRepository : BaseRepository<AlForsanRedemption>
    {
        private MLPDB01Entities DataContext { get; set; }
        public AlForsanRedemptionRepository(DbContext context)
          : base(context)
        {
            DataContext = (MLPDB01Entities)context;
        }
    }
}