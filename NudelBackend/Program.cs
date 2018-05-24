using System;

namespace Nudel.Backend
{
    public class Program
    {
        static void Main(string[] args)
        {
            NetworkListener networkListener = new NetworkListener();
            networkListener.Start();

            Console.ReadLine();
        }
    }
}
