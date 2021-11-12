using System;
using System.Globalization;
using System.IO;
using System.Threading;
using CsvHelper;

namespace Gaba1Aggregator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("ガバ1動画集計補助ツール\r\n");
            Console.WriteLine("※動画一覧は毎朝5時更新（スナップショット検索APIで取得しているため）\r\n");
            Console.WriteLine("--------------------");
            int count; //第count回

            do
            {
                Console.Write("第何回？ > ");
                var input = Console.ReadLine();
                if (!int.TryParse(input, out count) || count <= 0)
                {
                    Console.WriteLine("エラー：自然数を入力してください" + count);
                }
            } while (count <= 0);

            try
            {
                var searchWord = $"【第{count}回No.1ガバ王子決定戦】";
                var outputFile = $"gaba1-{count}.csv";
                var outputFileSmall = $"gaba1-{count}s.csv";

                using (var api = new NicoApi())
                {
                    var cts = new CancellationTokenSource();

                    Console.WriteLine($"{searchWord} で検索開始");
                    var result = api.SearchByTagsAsync(searchWord, cts.Token).Result;
                    Console.WriteLine($"検索完了　ヒット数：{result.Count}");

                    using (var csv = new CsvWriter(new StreamWriter(outputFile), CultureInfo.InvariantCulture))
                    {
                        csv.Context.RegisterClassMap<ContentData.ClassMap>();
                        csv.Context.Configuration.HasHeaderRecord = true;
                        csv.WriteRecords(result);
                    }
                    Console.WriteLine($"{outputFile} に出力しました");

                    using (var csv = new CsvWriter(new StreamWriter(outputFileSmall), CultureInfo.InvariantCulture))
                    {
                        csv.Context.RegisterClassMap<ContentData.ClassMapSmall>();
                        csv.Context.Configuration.HasHeaderRecord = true;
                        csv.WriteRecords(result);
                    }
                    Console.WriteLine($"{outputFileSmall} に縮小版を出力しました");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            Console.WriteLine("\r\n無事完了したようです");
            Console.WriteLine("何かキーを入力したら終了します");
            Console.ReadKey();
        }
    }
}