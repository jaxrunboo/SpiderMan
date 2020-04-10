using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Weibo
{
    public class CatchByApiService
    {
        private static readonly HttpClient HttpClient;
        static CatchByApiService()
        {
            HttpClient = new HttpClient();
        }

        public void Set(Dictionary<string, string> args)
        {
            foreach (var item in args)
            {
                HttpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
            }
        }

        public async Task<string> Get(string uri)
        {
            try
            {
                HttpResponseMessage response = await HttpClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task Get()
        {

        }

        public async Task Test()
        {
            List<Task> list = new List<Task>();
            list.Add(Get());
            list.Add(Get());

            Task.WaitAll(list.ToArray());
        }

        public async Task DownloadImg(string imgUrl, string path, string name)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            await Download(imgUrl, path, name);
        }

        public async Task DownloadImg(List<string> imgUrlList, string path)
        {
            int index = 1;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            foreach (var item in imgUrlList)
            {
                await Download(item, path, index.ToString());
                index++;
                //if (index % 10 == 0)
                //{
                //    await Task.Delay(1000);
                //}
            }
        }

        private async Task Download(string url, string path, string name, string type = "jpg")
        {
            using (var res = await HttpClient.GetAsync(url))
            {
                res.EnsureSuccessStatusCode();
                using (var reader = await res.Content.ReadAsStreamAsync())
                using (FileStream writer = new FileStream($"{path}/{name}.{type}", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    byte[] buff = new byte[512];
                    int c = 0;
                    while ((c = reader.Read(buff, 0, buff.Length)) > 0)
                    {
                        writer.Write(buff, 0, c);
                    }
                }
            }
        }

    }
}
