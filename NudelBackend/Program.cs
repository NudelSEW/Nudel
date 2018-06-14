using System;

namespace Nudel.Backend
{
    /// <summary>
    /// launching the server through the networkListener 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// instantiating a new networkListener object to start the NudelServer and opening the console
        /// </summary>
        /// <param name="args"> String parameter </param>
        static void Main(string[] args)
        {
            NetworkListener networkListener = new NetworkListener();
            networkListener.Start();

            Console.ReadLine();
        }
    }
}
