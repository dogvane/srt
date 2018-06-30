using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dogvane.Srt;

namespace Tools
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = "翻译中字英语 (自动生成)When Fast is Faster Than Fastest.srt";

            var srt = new SrtManager();
            var battute = srt.LoadBattuteByFile(fileName);
            var rets = 合并字幕(battute, 5.1);
            rets = 合并字幕(rets, 6.0);

            var newfile = fileName.Replace(".srt", "");
            newfile += "_fix.srt";
            Console.WriteLine("{1}/{0}", battute.Count, rets.Count);
            SrtManager.SaveBattute(newfile, rets);
        }

        private static List<Battuta> 合并字幕(List<Battuta> battute, double timeSpan = 5.0)
        {
            var rets = new List<Battuta>();

            for (var i = 0; i < battute.Count - 1; i += 1)
            {
                var item1 = battute[i];
                var item2 = battute[i + 1];

                // 如果2个字幕总显示时间的间隔在5s以内，则进行合并
                var span = item2.ToSec - item1.FromSec;
                if (span > timeSpan)
                {
                    item1.Text = item1.Text.Replace(" ", "");
                    rets.Add(item1);
                    continue;
                }

                rets.Add(new Battuta(0, item1.From, item2.To, (item1.Text + item2.Text).Replace(" ", "")));
                i += 1;
            }

            var index = 1;
            foreach (var item in rets)
            {
                item.Id = index++;
                //Console.WriteLine("{0:F2}  {1}  {2}", item.Duration, item.Text.Length, item.Text);
            }

            return rets;
        }
    }
}
