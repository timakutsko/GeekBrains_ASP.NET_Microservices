using System;
using System.Collections.Generic;

namespace MetricAgent.DAL.Interfaces
{
    public interface IRepository<T> where T :class 
    { 
        public void Create(T metric);
        public IList<T> GetByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime); 
    }
}
