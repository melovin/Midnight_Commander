using Midnight_Commander.Components;
using Midnight_Commander.Windows;
using System;

namespace Midnight_Commander
{
    class Program
    {
        public static int CH = Console.WindowHeight;
        public static int CW = Console.WindowWidth;
        static void Main(string[] args)
        {
            Application.brws = new BrowsersW();
            Application.Window = Application.brws;            
            Application.Draw();

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo info = Console.ReadKey(true);
                    Application.HandleKey(info);
                }
                else if(CH!=Console.WindowHeight || CW!= Console.WindowWidth)
                {
                    Application.Draw();
                    CH = Console.WindowHeight;
                    CW = Console.WindowWidth;
                }
            }
        }
    }
}
