using System;


namespace UnityGameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Game server";

            Server.Start(5, 7050);

            Console.ReadKey();
        }
    }
}
