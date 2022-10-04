using Midnight_Commander.Windows;
using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight_Commander.Components
{
    public class Files :IComponent
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Size { get; set; }
        public string Date { get; set; }
        public string Parent { get; set; }

        public Files(string name,string fullName, string size, string date, string dir)
        {
            Name = '│' + name;
            FullName = fullName;
            Size = "│  " + size;
            Date = date;
            Parent = dir;
        }
        public void Draw()
        {
            if (this.Size.Length > 9)
                this.Size = this.Size.Substring(0, 9) ;
            while (this.Size.Length != 11)
            {
                this.Size += " ";
            }
            this.Size += "│";

            if (this.Name.Length > 34)
                this.Name = this.Name.Substring(0, 34);

            if (this.Name.Contains(".exe"))
                Console.ForegroundColor = ConsoleColor.Green;
            else if(this.Name.Contains(".txt"))
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            else if (this.Name.Contains(".zip"))
                Console.ForegroundColor = ConsoleColor.Red;

            Console.Write(this.Name.PadRight(34));
            Console.Write(this.Size.PadRight(12));
            while (this.Date.Length != 13)
            {
                this.Date += " ";
            }
            Console.Write(this.Date);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("│");
        }
        public void HandleKey(ConsoleKeyInfo info)
        {
            throw new NotImplementedException();
        }
    }
}
