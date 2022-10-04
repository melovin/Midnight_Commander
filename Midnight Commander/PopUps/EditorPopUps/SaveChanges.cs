using Midnight_Commander.Windows;
using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight_Commander.PopUps
{
    public class SaveChanges : IPopUp
    {
        public bool Active { get; set; }
        private string Path { get; set; }
        private int Selected = 0;
        public bool Confirmed = false;
        public event Action YouCanSaveMe;
        public SaveChanges(string path)
        {
            this.Active = true;
            this.Path = path;
        }

        public void Draw()
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 - 16, Console.WindowHeight / 2 - 5);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(" ".PadRight(30));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 16, Console.WindowHeight / 2 - 4);
            Console.WriteLine("- Opravdu uložit? -".PadLeft(26).PadRight(30));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 16, Console.WindowHeight / 2 - 3);
            if (this.Path.Length > 30)
                this.Path = this.Path.Substring(0, 30);
            Console.WriteLine(this.Path.PadLeft(20).PadRight(30));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 16, Console.WindowHeight / 2 - 2);

            if (Selected == 0)
            {
                Console.Write("".PadLeft(5));
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("[ ANO ]");
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("".PadRight(7));
            }
            else
            {
                Console.Write("[ ANO ]".PadLeft(12).PadRight(19));
            }
            if (Selected == 1)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("[ NE ]");
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("".PadRight(5));
            }
            else
            {
                Console.Write("[ NE ]".PadRight(11));
            }
            Console.SetCursorPosition(Console.WindowWidth / 2 - 16, Console.WindowHeight / 2 - 1);
            Console.WriteLine(" ".PadRight(30));
        }

        public void HandleKey(ConsoleKeyInfo info)
        {
            switch (info.Key)
            {
                case ConsoleKey.Tab:
                    if (Selected == 0)
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
                    if (Selected == 0)
                        YouCanSaveMe();
                    this.Active = false;
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
    }
}
