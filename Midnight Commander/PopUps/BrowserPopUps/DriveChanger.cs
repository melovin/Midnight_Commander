using Midnight_Commander.Services;
using Midnight_Commander.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Midnight_Commander
{
    public class DriveChanger :IPopUp
    {
        private int selected = 0;

        public bool Active { get; set; }

        public event Action<string> ChangeDrive;
        public DriveChanger()
        {
            this.Active = true;
        }
        
        public void HandleKey(ConsoleKeyInfo info)
        {        
                switch (info.Key)
                {
                    case ConsoleKey.Tab:
                        if (selected < DriveService.Drives().Length - 1)
                        {
                            selected++;
                            this.Draw();
                        }
                        else if (selected == DriveService.Drives().Length - 1)
                        {
                            selected = 0;
                            this.Draw();
                        }
                        return;
                    case ConsoleKey.Enter: 
                         this.PathName();
                         this.Active = false;
                        return;        
                    case ConsoleKey.Escape:
                        this.Active = false;
                        Application.Draw();
                        return;
                default:
                    this.Active = false;
                    Application.Draw();
                    return;
            }            
        }
        public void PathName() //uklada jmeno vybraneho disku do action
        {
            int tempS = 0;
            foreach (DriveInfo item in DriveService.Drives())
            {
                if (tempS == selected)
                {
                    this.ChangeDrive(item.Name);
                }
                tempS++;
            }
        }
        public void Draw()
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 -16, Console.WindowHeight / 2-5);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(" ".PadRight(30));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 16, Console.WindowHeight / 2 - 4);
            Console.WriteLine("       - Change drive -".PadRight(30));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 16, Console.WindowHeight / 2 - 3);
            Console.WriteLine("        ".PadRight(30));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 16, Console.WindowHeight / 2 - 2);
            int l = 1;
            int lvl = 1;
            int lvl2 = -1;
            int s = 0;
            foreach (DriveInfo item in DriveService.Drives())
            {
                if(selected==s)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    if (l % 3 == 0) 
                    {
                        Console.Write("   [" + item.Name + "]".PadRight(3));
                        Console.BackgroundColor = ConsoleColor.Gray;
                        if (l == DriveService.Drives().Length)
                        {
                            Console.SetCursorPosition(Console.WindowWidth / 2 - 16, Console.WindowHeight / 2 - lvl);
                            Console.WriteLine("        ".PadRight(30));
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.SetCursorPosition(Console.WindowWidth / 2 - 16, Console.WindowHeight / 2 - lvl);
                        }
                        lvl2++;
                        lvl--;
                    }
                    else if (DriveService.Drives().Length % 3 != 0 && l == DriveService.Drives().Length)
                    {
                        double onNewLineDouble = DriveService.Drives().Length / (float)3;
                        string[] onNewLine = onNewLineDouble.ToString().Split(',');
                        if (onNewLine[1].StartsWith('3'))
                        {
                            Console.Write("   [" + item.Name + "]".PadRight(23));
                            Console.BackgroundColor = ConsoleColor.Gray;
                        }
                        else if (onNewLine[1].StartsWith('6'))
                        {
                            Console.Write("   [" + item.Name + "]".PadRight(13));
                            Console.BackgroundColor = ConsoleColor.Gray;
                        }
                        Console.SetCursorPosition(Console.WindowWidth / 2 - 16, Console.WindowHeight / 2 + lvl2);
                        Console.WriteLine("        ".PadRight(30));
                        lvl2++;
                    }
                    else
                    {
                        Console.Write("   [" + item.Name + "]".PadRight(3));
                        Console.BackgroundColor = ConsoleColor.Gray;
                    }                       
                    l++;
                    s++;
                }
                else
                {
                    if (l % 3 == 0)//zalomi po kazdem 3. disku
                    {
                        Console.Write("   [" + item.Name + "]".PadRight(3));
                        if (l == DriveService.Drives().Length)
                        {
                            Console.SetCursorPosition(Console.WindowWidth / 2 - 16, Console.WindowHeight / 2 - lvl);
                            Console.WriteLine("        ".PadRight(30));
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.SetCursorPosition(Console.WindowWidth / 2 - 16, Console.WindowHeight / 2 - lvl);
                        }
                        lvl2++;
                        lvl--;
                    }
                    else if (DriveService.Drives().Length % 3 != 0 && l == DriveService.Drives().Length)
                    {
                        double onNewLineDouble = DriveService.Drives().Length / (float)3;
                        string[] onNewLine = onNewLineDouble.ToString().Split(',');
                        if (onNewLine[1].StartsWith('3'))
                        {
                            Console.Write("   [" + item.Name + "]".PadRight(23));
                        }
                        else if (onNewLine[1].StartsWith('6'))
                        {
                            Console.Write("   [" + item.Name + "]".PadRight(13));
                        }
                        Console.SetCursorPosition(Console.WindowWidth / 2 - 16, Console.WindowHeight / 2 + lvl2);
                        Console.WriteLine("        ".PadRight(30));
                        lvl2++;
                    }
                    else
                        Console.Write("   [" + item.Name + "]".PadRight(3));
                    l++;
                    s++;
                }
                
            }
        }
    }
}
