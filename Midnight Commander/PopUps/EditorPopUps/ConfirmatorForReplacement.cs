using Midnight_Commander.Windows;
using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight_Commander.PopUps.EditorPopUps
{
    public class ConfirmatorForReplacement : IPopUp
    {
        public bool Active { get; set; }
        private string ReplaceMe = "";
        private string ReplaceTo = "";
        private int Selected = 0;
        public event Action<string, string> Replace;
        public event Action<string> Skip;
        public event Action<string> ReplaceAll;
        public ConfirmatorForReplacement(string[] replace)
        {
            this.Active = true;
            this.ReplaceMe = replace[0];
            this.ReplaceTo = replace[1];
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
                case ConsoleKey.Enter:
                    if (this.Selected == 0)
                    {
                        this.Replace(ReplaceMe,ReplaceTo);
                        this.Active = false;
                    }
                    if (this.Selected == 1)
                    {
                        this.ReplaceAll(this.ReplaceMe + ">" + this.ReplaceTo);
                        this.Active = false;
                    }
                    if (this.Selected == 2)
                    {
                        this.Skip(ReplaceMe +">"+ReplaceTo);
                        this.Active = false;
                    }
                    else if (this.Selected == 3)
                    {
                        this.Active = false;
                        Application.Draw();
                    }
                    return;
                case ConsoleKey.Escape:
                    this.Active = false;
                    Application.Draw();
                    return;
            }
        }
        public void Draw()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(Console.WindowWidth / 2 - 10, Console.WindowHeight / 2 - 5);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.WriteLine(" ".PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 10, Console.WindowHeight / 2 - 4);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("- Potvrdit nahrazení -".PadLeft(Console.WindowWidth / 2 - 20).PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 10, Console.WindowHeight / 2 - 3);
            Console.WriteLine("        ".PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 10, Console.WindowHeight / 2 - 2);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"   \"{ReplaceMe}\"".PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 10, Console.WindowHeight / 2 - 1);
            Console.WriteLine("Nahradit textem:".PadLeft(19).PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 10, Console.WindowHeight / 2 );
            Console.WriteLine($"   \"{ReplaceTo}\"".PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 10, Console.WindowHeight / 2 + 1);
            Console.WriteLine(" ".PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 10, Console.WindowHeight / 2 + 2);
            if (Selected == 0)
            {
                Console.Write("".PadLeft(2));
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("[< Nahradit >]");
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("".PadRight(2));
            }
            else
                Console.Write("[< Nahradit >]".PadLeft(16).PadRight(18));
            if (Selected == 1)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("[ Všechny ]");
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("".PadRight(2));
            }
            else
                Console.Write("[ Všechny ]".PadRight(13));
            if (Selected == 2)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("[ Přeskočit ]");
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("".PadRight(2));
            }
            else
                Console.Write("[ Přeskočit ]".PadRight(15));
            if (Selected == 3)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("[ Storno ]");
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("".PadRight(4));
            }
            else
                Console.Write("[ Storno ]".PadRight(14));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 10, Console.WindowHeight / 2 + 3);
            Console.WriteLine(" ".PadRight(60));
            Console.BackgroundColor = ConsoleColor.Blue;
        }

    }
}
