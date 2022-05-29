using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metrics.Data.Interfaces
{
    public interface IMetric
    {
        public float Value { get; set; }

        public long Ticks { get; set; }
    }
}
