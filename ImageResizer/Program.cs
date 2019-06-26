using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ImageResizer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string sourcePath = Path.Combine(Environment.CurrentDirectory, "images");
            string destinationPath = Path.Combine(Environment.CurrentDirectory, "output"); ;
            long totalms=0;
            var runtimes = 5;
            ImageProcess imageProcess = new ImageProcess();

            Stopwatch sw = new Stopwatch();

            // ---------------------------------------------------
            var specificName = "sync";
            for (int i = 0; i < runtimes; i++)
            {
                imageProcess.Clean(destinationPath);

                sw.Reset();
                sw.Start();
                imageProcess.ResizeImages(sourcePath, destinationPath, 2.0);
                sw.Stop();

                totalms += sw.ElapsedMilliseconds;
                Console.WriteLine($"{specificName} 花費時間: {sw.ElapsedMilliseconds} ms");
            }

            var syncAvg = totalms / runtimes;
            Console.WriteLine($"{specificName} 執行 {runtimes} 次，平均花費時間: {syncAvg} ms");

            // ------------------------------------------------------------------
            specificName = "async";
            var asyncDestinationPath = $"{destinationPath}{specificName}";
            totalms = 0;
            for (int i = 0; i < runtimes; i++)
            {
                imageProcess.Clean(asyncDestinationPath);

                sw.Reset();
                sw.Start();
                await imageProcess.ResizeImagesAsync(sourcePath, asyncDestinationPath, 2.0);
                sw.Stop();

                Console.WriteLine($"async 花費時間: {sw.ElapsedMilliseconds} ms");
                totalms += sw.ElapsedMilliseconds;
            }

            var asyncAvg = totalms / runtimes;
            Console.WriteLine($"{specificName} 執行 {runtimes} 次，平均花費時間: {asyncAvg} ms");
            var perfomance = Math.Truncate((decimal)asyncAvg / (decimal)syncAvg * 1000) / 10;
            Console.WriteLine($"{specificName} 效率提升 {100-perfomance}%");

            // --------------------------------------------------
            specificName = "parallelasync";
            var paralleldestinationPath = $"{destinationPath}{specificName}";
            totalms = 0;

            for (int i = 0; i < runtimes; i++)
            {
                imageProcess.Clean(paralleldestinationPath);

                sw.Reset();
                sw.Start();
                await imageProcess.ResizeImagesParallelAsync(sourcePath, paralleldestinationPath, 2.0);
                sw.Stop();

                totalms += sw.ElapsedMilliseconds;
                Console.WriteLine($"{specificName} 花費時間: {sw.ElapsedMilliseconds} ms");
            }

            var paraelleAsyncAvg = totalms / runtimes;
            perfomance = Math.Truncate((decimal)paraelleAsyncAvg / (decimal)syncAvg * 1000) / 10;
            Console.WriteLine($"{specificName} 執行 {runtimes} 次，平均花費時間: {paraelleAsyncAvg} ms");
            Console.WriteLine($"{specificName} 效率提升 {100-perfomance}%");

            Console.ReadKey();
        }
    }
}
