using Midnight_Commander.Windows;
using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight_Commander.Components
{
    public class Folder :IComponent
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Size = "│          │";
        public string Date { get; set; }
        public string Parent { get; set; }
        public Folder(string name,string fullName,string date,string dir)
        {
            Name = "│/" + name;
            FullName = fullName;
            Date = date ;
            Parent = dir;
        }

        public void HandleKey(ConsoleKeyInfo info)
        {
            throw new NotImplementedException();
        }

        public void Draw()
        {
            while (this.Date.Length != 13)
            {
                this.Date += " ";
            }
            if (this.Name.Length > 34)
                this.Name = this.Name.Substring(0, 34);
            Console.Write(this.Name.PadRight(34));
            Console.Write(this.Size.PadRight(12));
            Console.Write(this.Date);
            Console.WriteLine("│");
        }
    }
}
