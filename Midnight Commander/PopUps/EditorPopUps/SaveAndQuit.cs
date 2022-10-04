using Midnight_Commander.Windows;
using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight_Commander.PopUps
{
    public class SaveAndQuit : IPopUp
    {
        public bool Active { get; set; }
        private bool Edited { get; set; }
        private int Selected = 2;
        private string Path { get; set; }
        private string PathOfBrowser { get; set; }
        public event Action YouCanSaveMe;
        public SaveAndQuit(bool edited, string path, string pathOfBrowser)
        {
            this.Active = true;
            this.Edited = edited;
            this.Path = path;
            this.PathOfBrowser = pathOfBrowser;
        }
        public void HandleKey(ConsoleKeyInfo info)
        {
            switch (info.Key)
            {
                case ConsoleKey.Tab:
                    if (Selected < 2)
                    {
                        Selected++;
                        this.Draw();
                    }
                    else
                    {
                        Selected = 0;
                        this.Draw();
                    }
                    return;
                case ConsoleKey.Enter:
                    if (this.Selected == 0)
                    {
                        this.YouCanSaveMe();
                        Application.Window = new BrowsersW();
                        if (BrowsersW.LeftIsCurrent)
                            BrowsersW.LeftBPath = this.PathOfBrowser;
                        else
                            BrowsersW.RightBPath = this.PathOfBrowser;

                    }
                    else if (this.Selected == 1)
                    {
                        Application.Window = new BrowsersW();
                        if (BrowsersW.LeftIsCurrent)
                            BrowsersW.LeftBPath = this.PathOfBrowser;
                        else
                            BrowsersW.RightBPath = this.PathOfBrowser;
                    }
                    BrowsersW.FromEditor = true;
                    this.Active = false;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Clear();
                    Application.Draw();
                    return;
                case ConsoleKey.Escape:
                    this.Active = false;
                    Application.Draw();
                    return;
                default:
                    this.Draw();
                    return;
            }
        }
        public void Draw()
        {

                Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 5);
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine(" ".PadRight(60));
                Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 4);
                Console.WriteLine("- Zavřít soubor -".PadLeft(40).PadRight(60));
                Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 3);
                if (this.Path.Length > 30)
                    this.Path = this.Path.Substring(0, 30);
                Console.Write($"Soubor {this.Path} byl upraven.".PadLeft(42).PadRight(60));
                Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 2);
                Console.WriteLine("Uložení před uzavřením?".PadLeft(28).PadRight(60));
                Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 1);

                if (Selected == 0)
                {
                    Console.Write("".PadLeft(10));
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write("[ ANO ]");
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.Write("".PadRight(11));
                }
                else
                {
                    Console.Write("[ ANO ]".PadLeft(17).PadRight(28));
                }
                if (Selected == 1)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write("[ NE ]");
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.Write("".PadRight(11));
                }
                else
                {
                    Console.Write("[ NE ]".PadRight(17));
                }
                if (Selected == 2)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write("[ Storno ]");
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.Write("".PadRight(5));
                }
                else
                {
                    Console.Write("[ Storno ]".PadRight(15).PadLeft(2));
                }
                Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2);
                Console.WriteLine(" ".PadRight(60));
                Console.BackgroundColor = ConsoleColor.Black;
            
        }        
    }
}
