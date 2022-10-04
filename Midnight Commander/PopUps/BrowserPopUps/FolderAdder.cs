using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Midnight_Commander.Windows
{
    public class FolderAdder :IPopUp
    {
        private int Selected = 0;
        public event Action<string> FolderName;
        private string folderName = "";

        public bool Active { get ; set; }
        public FolderAdder()
        {
            this.Active = true;
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
                    else if(Selected == 2)
                    {
                        Selected = 0;
                        this.Draw();
                    }
                    return;
                case ConsoleKey.Enter:
                    if (Selected == 2)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        this.Active = false;
                        Application.Draw();
                        break;
                    }
                    else if ((Selected == 1 || Selected == 0) && this.folderName.Length != 0)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        this.FolderName(this.folderName);
                        this.Active = false;
                    }
                    else
                        this.Draw();
                    return;
                case ConsoleKey.Escape:
                    this.Active = false;
                    Application.Draw();
                    return;
                case ConsoleKey.Backspace:
                    if (this.folderName.Length > 0 && Selected==0)
                    {
                        this.folderName = this.folderName.Substring(0, this.folderName.Length - 1);
                        Console.SetCursorPosition(Console.WindowWidth / 2 - 27 + this.folderName.Length, Console.WindowHeight / 2 - 1);
                        this.Draw();
                    }
                    return;
                default:
                    if (this.folderName.Length <= 52 && Selected==0)
                        this.folderName += info.KeyChar.ToString();
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
            Console.WriteLine("- Vytvořit novou složku -".PadLeft(Console.WindowWidth/2-17).PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 3);
            Console.WriteLine("        ".PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 2);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Zadejte název složky: ".PadLeft(25).PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 - 1);
            Console.Write(">".PadLeft(3));
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(this.folderName);
            Console.Write("".PadRight(54-this.folderName.Length));
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write("<".PadRight(3));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 );
            Console.Write("".PadRight(60));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2+1);
            if(Selected == 1)
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
            if (Selected == 2)
            {
                Console.CursorVisible = false;
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("[ Storno ]");
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("".PadRight(15));
            }
            else
                Console.Write("[ Storno ]".PadRight(25));
            Console.SetCursorPosition(Console.WindowWidth / 2 - 30, Console.WindowHeight / 2 + 2);
            Console.WriteLine(" ".PadRight(60));            
            if (Selected==0)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - 27+this.folderName.Length, Console.WindowHeight / 2 - 1);
                Console.BackgroundColor = ConsoleColor.Blue;
                return;
            }            
        }
    }
}
