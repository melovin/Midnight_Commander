using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Midnight_Commander.Windows
{
    public class Deleter :IPopUp
    {
        private int Selected = 0;
        private string DeletePath { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }
        public static bool Confirmed = false;
        public Deleter(string path, string name)
        {
            DeletePath = path;
            this.Name = name;
            this.Active = true;
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
                    {
                        Confirmed = true; //rozhodnuto o mazani
                        this.Delete();
                        this.Active = false;
                        Console.ForegroundColor = ConsoleColor.White;
                        Application.Draw();
                    }
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
            Console.SetCursorPosition(Console.WindowWidth / 2 - 16, Console.WindowHeight / 2 - 5);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(" ".PadRight(30));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 16, Console.WindowHeight / 2 - 4);
            Console.WriteLine("- Opravdu odstranit? -".PadLeft(26).PadRight(30));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 16, Console.WindowHeight / 2 - 3);
            if (this.Name.Length > 30)
                this.Name = this.Name.Substring(0, 30);
            Console.WriteLine(this.Name.PadLeft(17).PadRight(30));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 16, Console.WindowHeight / 2 - 2);

            if (Selected == 0)
            {
                Console.Write("".PadLeft(5));
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("[ ANO ]");
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write("".PadRight(7));
            }
            else
            {
                Console.Write("[ ANO ]".PadLeft(12).PadRight(19));
            }
            if (Selected == 1)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("[ NE ]");
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write("".PadRight(5));
            }
            else
            {
                Console.Write("[ NE ]".PadRight(11));
            }
            Console.SetCursorPosition(Console.WindowWidth / 2 - 16, Console.WindowHeight / 2 -1);
            Console.WriteLine(" ".PadRight(30));
        }
        private void Delete()
        {
            try
            {
                Directory.Delete(DeletePath, true);
            }
            catch
            {
                File.Delete(DeletePath);
            }
        }
    }
}
