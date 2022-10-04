using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight_Commander.Windows
{
    public interface IPopUp
    {
        public bool Active { get; set; }
        public void HandleKey(ConsoleKeyInfo info);
        public void Draw();
    }
}
