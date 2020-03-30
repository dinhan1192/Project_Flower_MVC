using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Project_MVC.Models
{
    [DataContract]
    public class DataPoint
    {
        public DataPoint(DateTime x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public DataPoint(double y, string label)
        {
            this.Y = y;
            this.Label = label;
        }

        //Explicitly setting the name to be used while serializing to JSON.
        //[JsonProperty]
        //[JsonConverter(typeof(IsoDateTimeConverter))]
        [DataMember(Name = "x")]
        public Nullable<DateTime> X = null;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<double> Y = null;

        [DataMember(Name = "label")]
        public string Label = null;
    }
}