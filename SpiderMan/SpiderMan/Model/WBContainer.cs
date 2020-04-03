using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpiderMan.Model
{
    public class WBContainer : WBBase<WBContainerData>
    {
    }

    public class WBContainerData
    {
        [JsonProperty("tabsInfo")]
        public TabsInfo TabsInfo { get; set; }
    }

    public class TabsInfo
    {
        [JsonProperty("tabs")]
        public Tab[] Tabs { get; set; }
    }

    public class Tab
    {
        [JsonProperty("tab_type")]
        public string TabType { get; set; }

        [JsonProperty("containerid")]
        public string Containerid { get; set; }
    }
}
