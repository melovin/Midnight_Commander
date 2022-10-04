using Midnight_Commander.Windows;
using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight_Commander.PopUps
{
    public class Help : IPopUp
    {
        public bool Active{get; set; }

        public Help()
        {
            this.Active = true;
        }
        public void HandleKey(ConsoleKeyInfo info)
        {
            switch (info.Key)
            {
                case ConsoleKey.Escape:
                    this.Active = false;
                    Application.Draw();
                    return;
                case ConsoleKey.Enter:
                    this.Active = false;
                    Application.Draw();
                    return;
                case ConsoleKey.Spacebar:
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
            Console.WriteLine(" ".PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 4);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("- Nápověda -".PadLeft(Console.WindowWidth / 2 - 25).PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 3);
            Console.WriteLine("        ".PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 2);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Právě používáte:    Midnight Commander - správce souborů".PadLeft(59).PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 1);
            Console.WriteLine("        ".PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2);
            Console.Write("Návod k použití: ".PadRight(20).PadLeft(23));
            Console.WriteLine("Přepínání mezi okny - TABULÁTOR".PadRight(37));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 + 1);
            Console.WriteLine("Listování složkami/soubory - ŠIPKY".PadLeft(57).PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 + 2);
            Console.WriteLine("Otevření složky - ENTER".PadLeft(46).PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 + 3);
            Console.WriteLine("Zpátky - /.. (LVL UP)".PadLeft(44).PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 + 4);
            Console.WriteLine("Funkce - FUNKČÍ KLÁVESY (F1,F2...)".PadLeft(57).PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 + 5);
            Console.Write("Autor (remake): ".PadLeft(19).PadRight(23));
            Console.WriteLine("Ruslana Zubareva (2021)".PadLeft(10).PadRight(37));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 + 6);
            Console.WriteLine(" ".PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 + 7);
            Console.Write("".PadLeft(27));
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write("[ OK ]");
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.WriteLine("".PadRight(27));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 + 8);
            Console.WriteLine(" ".PadRight(60));
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
