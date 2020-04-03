using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpiderMan.Model
{
    public class WBBase<T>
    {
        [JsonProperty("ok")]
        public int Status { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
