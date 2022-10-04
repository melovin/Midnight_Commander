using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Midnight_Commander.Windows
{
    public class Mover :IPopUp
    {
        private int Selected = 0;
        private string To { get; set; }
        public bool Active { get; set; }
        private string Name { get; set; }
        private string Source { get; set; }
        public Mover(string from, string name)
        {
            this.Active = true;
            this.Source = from;
            this.Name = name;
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
                    else if (Selected == 2)
                    {
                        Selected = 0;
                        this.Draw();
                    }
                    return;
                case ConsoleKey.Backspace:
                    if (this.To.Length > 0)
                    {
                        this.To = this.To.Remove(this.To.Length - 1);
                        Console.SetCursorPosition(Console.WindowWidth / 2 - 27 + this.To.Length, Console.WindowHeight / 2 - 1);
                        this.Draw();
                    }
                    return;
                case ConsoleKey.Enter:
                    if (Selected == 2)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Application.Draw();
                        break;
                    }
                    else if ((Selected == 0 || Selected == 1) && this.To.Length != 0)
                    {
                        this.Move(this.Source, this.To);
                        this.Active = false;
                        Application.Draw();
                    }
                    else
                        this.Draw();
                    return;
                case ConsoleKey.Escape:
                    this.Active = false;
                    Application.Draw();
                    return;
                default:
                    if (this.To.Length <= 52)
                    {
                        this.To += info.KeyChar;
                        Console.Write(info.KeyChar);
                    }
                    return;
            }
        }
        public void SetPath(string path)
        {
            string tempPath = "";
            if (this.Name.Contains('.'))
                tempPath = this.Name.Substring(1);
            else
                tempPath = this.Name.Substring(2);

            this.To = path + @"\" + tempPath;
        }
        public void Draw()
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 5);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.WriteLine(" ".PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 4);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("- Přesunout -".PadLeft(Console.WindowWidth / 2 - 23).PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 3);
            Console.WriteLine("        ".PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 2);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Cíl:".PadLeft(6));
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(this.To);
            Console.Write("".PadRight(52 - this.To.Length));
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write("<".PadRight(2));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2-1);
            Console.Write("".PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 );
            if (Selected == 1)
            {
                Console.CursorVisible = false;
                Console.Write("".PadLeft(20));
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("[< OK >]");
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("".PadRight(7));
            }
            else
            {
                Console.Write("[< OK >]".PadLeft(28).PadRight(35));
            }
            if (Selected == 2)
            {
                Console.CursorVisible = false;
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("[ Storno ]");
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("".PadRight(15));
            }
            else
            {
                Console.Write("[ Storno ]".PadRight(25));
            }
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 + 1);
            Console.WriteLine(" ".PadRight(60));
            if (Selected == 0)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - 24 + this.To.Length, Console.WindowHeight / 2 - 2);
                Console.CursorVisible = true;
                Console.CursorSize = 2;
                Console.BackgroundColor = ConsoleColor.Blue;
                return;
            }
        }
        public void Move(string source, string to)
        {
            this.To = to;
            this.Draw();
                try
                {
                    try
                    {
                        File.Copy(source, this.To );
                        File.Delete(source);
                    }
                    catch
                    {
                        this.RecursiveCopying(source, this.To );
                        Directory.Delete(source,true);
                    }
                }
                catch
                {
                    this.DrawError();
                }
        }
        public void RecursiveCopying(string src, string to)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(src);
            DirectoryInfo[] dirarray = dirInfo.GetDirectories();
            FileInfo[] filArray = dirInfo.GetFiles();
            string tempPath = "";
            foreach (DirectoryInfo item in dirarray)
            {
                tempPath = to + @"\" + item.Name;
                if (dirInfo.GetDirectories() != null)
                {
                    Directory.CreateDirectory(tempPath);
                    try
                    {
                        foreach (FileInfo item1 in filArray)
                        {
                            File.Copy(item1.FullName, to + @"\" + item1.Name);
                        }
                    }
                    catch { }
                    this.RecursiveCopying(item.FullName, to + @"\" + item.Name);
                }
            }
        }
        private void DrawError()
        {
            Application.Draw();
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 5);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("".PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 4);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("- Jejda! Chybička se vloudila.. -".PadLeft(Console.WindowWidth / 2 - 12).PadRight(60));
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 3);
            Console.WriteLine("".PadRight(60));
            ConsoleKeyInfo info = Console.ReadKey();
            Application.Draw();
        }
    }
}
