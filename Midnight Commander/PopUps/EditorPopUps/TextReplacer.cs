using Midnight_Commander.Windows;
using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight_Commander.PopUps
{
    public class TextReplacer : IPopUp
    {
        public bool Active { get; set; }
        private string WordToReplace = "";
        private string ReplaceWith = "";
        private int Selected = 0;
        public event Action<string> Replace;
        public TextReplacer()
        {
            this.Active = true;
        }

        public void HandleKey(ConsoleKeyInfo info)
        {
            switch(info.Key)
            {
                case ConsoleKey.Tab:
                    if (this.Selected < 3)
                        this.Selected++;
                    else
                        this.Selected = 0;
                    this.Draw();
                    return;
                case ConsoleKey.Escape:
                    this.Active = false;
                    Application.Draw();
                    return;
                case ConsoleKey.Enter:
                    if(this.Selected == 3)
                    {
                        this.Active = false;
                        Application.Draw();
                    }
                    else if(this.WordToReplace != "")
                    {
                        this.Replace(this.WordToReplace + ">" + this.ReplaceWith);
                        this.Active = false;
                    }
                    return;
                case ConsoleKey.Backspace:
                    if (this.WordToReplace.Length > 0 && Selected == 0)
                    {
                        this.WordToReplace = this.WordToReplace.Substring(0, this.WordToReplace.Length - 1);
                        Console.SetCursorPosition(Console.WindowWidth / 2 - 27 + this.WordToReplace.Length, Console.WindowHeight / 2 - 1);
                        this.Draw();
                    }
                    else if (this.ReplaceWith.Length > 0 && Selected == 1)
                    {
                        this.ReplaceWith = this.ReplaceWith.Substring(0, this.ReplaceWith.Length - 1);
                        Console.SetCursorPosition(Console.WindowWidth / 2 - 27 + this.ReplaceWith.Length, Console.WindowHeight / 2 - 1);
                        this.Draw();
                    }

                    return;
                default:
                    if (this.Selected == 0)
                        this.WordToReplace += info.KeyChar.ToString();
                    else if (this.Selected == 1)
                        this.ReplaceWith += info.KeyChar.ToString();
                    this.Draw();
                    return;
            }
        }
        public void Draw()
        {
            Console.CursorVisible = true;
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 5);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.WriteLine(" ".PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 4);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("- Nahrazení -".PadLeft(Console.WindowWidth / 2 - 23).PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 3);
            Console.WriteLine("        ".PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 2);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Zadejte text k nahrazení: ".PadLeft(29).PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 1);
            Console.Write(">".PadLeft(3));
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(this.WordToReplace);
            Console.Write("".PadRight(54 - this.WordToReplace.Length));
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write("<".PadRight(3));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2);
            Console.Write("".PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 + 1);
            Console.Write("".PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 + 2);
            Console.WriteLine("Zadejte nahrazující text: ".PadLeft(29).PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 +3);
            Console.Write(">".PadLeft(3));
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(this.ReplaceWith);
            Console.Write("".PadRight(54 - this.ReplaceWith.Length));
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write("<".PadRight(3));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 + 4);
            Console.Write("".PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 + 5);
            if (Selected == 2)
            {
                Console.CursorVisible = false;
                Console.Write("".PadLeft(20));
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("[< OK >]");
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("".PadRight(7));
            }
            else
                Console.Write("[< OK >]".PadLeft(28).PadRight(35));
            if (Selected == 3)
            {
                Console.CursorVisible = false;
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("[ Storno ]");
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("".PadRight(15));
            }
            else
                Console.Write("[ Storno ]".PadRight(25));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 + 6);
            Console.WriteLine(" ".PadRight(60));
            if (Selected == 0)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - 27 + this.WordToReplace.Length, Console.WindowHeight / 2 - 1);
                Console.BackgroundColor = ConsoleColor.Blue;
                return;
            }
            if (Selected == 1)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - 27 + this.ReplaceWith.Length, Console.WindowHeight / 2 + 3);
                Console.BackgroundColor = ConsoleColor.Blue;
                return;
            }
        }
    }
}
