using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weibo.Model;

namespace Weibo
{
    class Program
    {
        static string uid = string.Empty;

        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                Console.WriteLine("请输入用户Id:");
                uid = Console.ReadLine();
                uid = uid.Trim();
                Console.WriteLine("下载开始:");
                await Run();
            }).Wait();

            Console.WriteLine("下载结束，按任意键退出");
            Console.Read();
        }

        public static async Task Run()
        {
            CatchByApiService client = new CatchByApiService();
            //
            string url1 = $"https://m.weibo.cn/api/container/getIndex?type=uid&value={uid}";
            string tabres = await client.Get(url1);
            var tab = JsonConvert.DeserializeObject<WBContainer>(tabres);
            string containerId = string.Empty;
            foreach (var item in tab.Data.TabsInfo.Tabs)
            {
                if (item.TabType == "weibo")
                {
                    containerId = item.Containerid;
                    break;
                }
            }

            //用来获取基础数据 包括： 微博总条数 用户名称
            string url2 = $"https://m.weibo.cn/api/container/getIndex?type=uid&value={uid}&containerid={containerId}&page=1";
            string cardres = await client.Get(url2);
            var cards = JsonConvert.DeserializeObject<WBPage>(cardres);

            int total = cards.Data.CardlistInfo.Total;
            //cards中MBlog可能为空
            string username = cards.Data.Cards.Where(e => e.MBlog != null && e.MBlog.User != null && e.MBlog.User.UserName != null)
            .FirstOrDefault().MBlog.User.UserName;
            int pages = total % 10 != 0 ? (total / 10 + 1) : (total / 10);

            for (int i = 1; i <= pages; i++)
            {
                await Load(uid, containerId, i, client, username);
                Console.WriteLine($"下载进度-----   {i}/{pages}");
            }
        }

        public static async Task Load(string uid, string containerId, int pageIdnex, CatchByApiService client, string username)
        {
            //用来获取基础数据 包括： 微博总条数 用户名称
            string url2 = $"https://m.weibo.cn/api/container/getIndex?type=uid&value={uid}&containerid={containerId}&page={pageIdnex}";
            string cardres = await client.Get(url2);
            var cards = JsonConvert.DeserializeObject<WBPage>(cardres);
            var imgUrls = new List<string>();
            if (cards.Data.Cards == null || cards.Data.Cards.Length == 0)
            {
                Console.WriteLine("用户数据不存在");
            }
            else
            {
                foreach (var card in cards.Data.Cards)
                {
                    try
                    {
                        var list = card?.MBlog?.Pics?.Where(e => e.Large != null)?.Select(e => e.Large.Url);
                        if (list != null)
                        {
                            imgUrls.AddRange(list);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            var root = System.Environment.CurrentDirectory;
            await client.DownloadImg(imgUrls, $"{root}//{username}//{pageIdnex}");
        }
    }
}
