using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace demon_cat_boris
{
    class Program
    {
        public static string CreateMD5(string input)
        {
            string password = input;
            byte[] encodedPassword = new UTF8Encoding().GetBytes(password);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            string encoded = BitConverter.ToString(hash)
               .Replace("-", string.Empty)
               .ToLower();
            return encoded;
        }

        public static void SendPOST(string req)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://xmas2018.dev.bytexgames.ru/postresult");
            request.Method = "POST";
            request.UserAgent = "UnityPlayer/2018.3.0f2 (UnityWebRequest/1.0, libcurl/7.52.0-DEV)";
            request.Accept = "*/*";
            request.Headers["Accept-Encoding"] = "identity";
            request.Headers["X-Unity-Version"] = "2018.3.0f2";
            request.Expect = "";
            string postData = req;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            Console.WriteLine(responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();
        }

        static void Main(string[] args)
        {
            //SendPOST(CreateMD5("killedAsteroids=28&killedBug=1&killedRouter=2&name=%D0%9A%D0%BE%D1%82+%D0%91%D0%BE%D1%80%D0%B8%D1%81&score=37&shotCount=48&time=21.23342&wave=2"));
            //Console.WriteLine(CreateMD5("killedAsteroids=7&killedBug=3&killedRouter=3&name=Boreeeeeeeeeees&score=25&shotCount=27&time=16.85555&wave=2"));
            int count = 0;
            while (true)
            {
                Random rnd = new Random(Guid.NewGuid().GetHashCode());
                string[,] namearr = new string[,] { {"", "XxX_", "super", "wow", "1969", "69", "1337", "such", "lol", "feline" },
                {"cat", "kot", "kotofei", "kotei4", "koteika", "kotan", "", "kotor", "kek", "cot" }, {"ops", "cheburek", "boris", "borat", "boriska", "borisych", "boris-san", "barboris", "borya", "borisko" },
            {"69", "_XxX", "destroyer", "", "", "wow", "ggwp", "hackerman", "xD", "_))000" } };
                string name = "";
                if (rnd.Next(2) == 0)
                    name += namearr[0, rnd.Next(8)];
                if (rnd.Next(2) == 0)
                    name += namearr[1, rnd.Next(8)];
                name += namearr[2, rnd.Next(8)];
                if (rnd.Next(2) == 0)
                    name += namearr[3, rnd.Next(8)];
                int[] r = new int[] { 10, 100, 1000, 10000, 100000, 1000000, 10000000, 300000000 };
                int astr = rnd.Next(r[rnd.Next(r.Length)]);
                int bugs = rnd.Next(astr / 6, astr / 4 + 5);
                int routs = rnd.Next(astr / 6, astr / 4 + 5);
                int score = astr + bugs * 3 + routs * 3;
                int shots = rnd.Next(astr + bugs + routs, (astr + bugs + routs) * 3 + 10);
                double time = rnd.NextDouble() + shots / 3;
                time = Math.Round(time, 5);
                int wave = (int)(time / 4);
                string md5r =
                    "killedAsteroids=" + astr +
                    "&killedBug=" + bugs +
                    "&killedRouter=" + routs +
                    "&name=" + name +
                    "&score=" + score +
                    "&shotCount=" + shots +
                    "&time=" + time +
                    "&wave=" + wave;
                //{"name":"Boreeeeeeeeeees","wave":2,"score":31,"shotCount":41,"time":"19.86357","killedAsteroids":19,"killedBug":1,"killedRouter":3,"sign":"4DB02E3407321D73503569F76785FC74"}
                //{"name":"Fff","wave":7,"score":227,"shotCount":344,"time":"92.23271","killedAsteroids":86,"killedBug":26,"killedRouter":21,"sign":"984B1BF69C0C47BD71321E875DCCE02D"}
                //Console.WriteLine(md5r);
                md5r = CreateMD5(md5r);
                string req =
                    "{\"name\":\"" + name +
                    "\",\"wave\":" + wave +
                    ",\"score\":" + score +
                    ",\"shotCount\":" + shots +
                    ",\"time\":\"" + time +
                    "\",\"killedAsteroids\":" + astr +
                    ",\"killedBug\":" + bugs +
                    ",\"killedRouter\":" + routs +
                    ",\"sign\":\"" + md5r +
                    "\"}";
                SendPOST(req);
                Console.WriteLine(++count);
            }
            Console.ReadLine();
        }
    }
}
