using System;
using System.Collections.Generic;

namespace FirstWebApplication
{
    public class ValuesHolder
    {
        public List<MyValue> Values { get; set; } = new List<MyValue>();

        public void Add (string date, string value)
        {

            Values.Add(new MyValue
            {
                Date = date,
                Value = value
            }); 
        }

        public List<MyValue> Get()
        {
            return Values;
        }
    }

    public class MyValue
    {
        public string Date { get; set; }
        public string Value { get; set; }
    }
}
