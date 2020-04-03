using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpiderMan.Model
{
    public class WBPage: WBBase<WBPageData>
    {
    }

    public class WBPageData
    {
        [JsonProperty("cardlistInfo")]
        public CardlistInfo CardlistInfo { get; set; }
        [JsonProperty("cards")]
        public Card[] Cards { get; set; }
    }
    public class CardlistInfo
    {
        [JsonProperty("total")]
        public int Total { get; set; }
        [JsonProperty("v_p")]
        public int V_p { get; set; }
    }
    public class Card
    {
        [JsonProperty("mblog")]
        public MBlog MBlog { get; set; }
    }
    public class MBlog
    {
        [JsonProperty("pics")]
        public Pic[] Pics { get; set; }
        [JsonProperty("user")]
        public User User { get; set; }
    }
    public class Pic
    {
        [JsonProperty("large")]
        public Large Large { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }  
    }
    public class Large
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class User
    {
        [JsonProperty("screen_name")]
        public string UserName { get; set; }
    }

}
