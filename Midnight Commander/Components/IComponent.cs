using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight_Commander.Components
{
    public interface IComponent
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Parent { get; set; }
        public void HandleKey(ConsoleKeyInfo info);

        public void Draw();
    }
}
