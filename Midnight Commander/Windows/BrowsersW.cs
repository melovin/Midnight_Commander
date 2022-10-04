using Midnight_Commander.Components;
using Midnight_Commander.Windows;
using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight_Commander
{
    public class BrowsersW : Window
    { 
        public Browser LeftBrowser = new Browser(0);
        public Browser RightBrowser = new Browser(Console.WindowWidth/2);
        public static bool LeftIsCurrent = true;
        public static bool LeftInProcess = true;
        public static string LeftBPath = "";
        public static string RightBPath = "";
        public static bool FromEditor = false;

        public override void HandleKey(ConsoleKeyInfo info)
        {
            if (LeftIsCurrent)
                LeftBrowser.HandleKey(info); 
            else
                RightBrowser.HandleKey(info);      
        }
        public override void Draw()
        {
            if(LeftIsCurrent)
            {
                if (FromEditor)
                {
                    LeftBrowser.CurrentPath = LeftBPath;
                    FromEditor = false;
                }
                else
                    LeftBPath = LeftBrowser.CurrentPath;
            }
            else
            {
                if (FromEditor)
                {
                    RightBrowser.CurrentPath = RightBPath;
                    FromEditor = false;
                }
                else
                    RightBPath = RightBrowser.CurrentPath;
            }
            
            LeftBrowser.Draw();
            BrowsersW.LeftInProcess = false;
            
            RightBrowser.Draw();
            BrowsersW.LeftInProcess = true;
            Browser brw = new Browser(0);
            brw.DrawHelpMenu();
        }        
    }
}
