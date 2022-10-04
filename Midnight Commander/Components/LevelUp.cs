using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight_Commander.Components
{
    public class LevelUp :IComponent
    {
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Parent { get ; set; }

        public LevelUp()
        {
            Name = "│/..";
            FullName = "│   lvlUp  │";
        }
        public void Draw()
        {
            Console.Write(this.Name.PadRight(34));
            Console.Write(this.FullName.PadRight(25));
            Console.WriteLine('│');
        }

        public void HandleKey(ConsoleKeyInfo info)
        {
            throw new NotImplementedException();
        }
    }
}
