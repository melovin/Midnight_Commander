using Midnight_Commander.Windows;
using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight_Commander
{
    public class Application
    {
        public static Window Window { get; set; }
        public static BrowsersW brws { get; set; }
        public static void HandleKey(ConsoleKeyInfo info)
        {
            Application.Window.HandleKey(info);
        }
        public static void Draw()
        {
            Application.Window.Draw();
        }
    }
}
