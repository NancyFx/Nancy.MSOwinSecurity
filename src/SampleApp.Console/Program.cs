namespace SampleApp.Console
{
    using System;
    using Microsoft.Owin.Hosting;

    internal class Program
    {
        private static void Main(string[] args)
        {
            using (WebApp.Start<Startup>(new StartOptions {Port = 8888}))
            {
                Console.WriteLine("Listening at http://localhost:8888");
                Console.ReadLine();
            }
        }
    }
}