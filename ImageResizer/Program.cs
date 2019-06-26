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

            ImageProcess imageProcess = new ImageProcess();

            imageProcess.Clean(destinationPath);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            imageProcess.ResizeImages(sourcePath, destinationPath, 2.0);
            sw.Stop();
            Console.WriteLine($"sync 花費時間: {sw.ElapsedMilliseconds} ms");

            var asyncdestinationPath = $"{destinationPath}async";
            imageProcess.Clean(asyncdestinationPath);

            sw.Reset();
            sw.Start();
            await imageProcess.ResizeImagesAsync(sourcePath, asyncdestinationPath, 2.0);
            sw.Stop();

            Console.WriteLine($"async 花費時間: {sw.ElapsedMilliseconds} ms");

            var paralleldestinationPath = $"{destinationPath}parallelasync";
            imageProcess.Clean(paralleldestinationPath);

            sw.Reset();
            sw.Start();
            await imageProcess.ResizeImagesParallelAsync(sourcePath, paralleldestinationPath, 2.0);
            sw.Stop();

            Console.WriteLine($"parallelasync 花費時間: {sw.ElapsedMilliseconds} ms");


            Console.ReadKey();
        }
    }
}
