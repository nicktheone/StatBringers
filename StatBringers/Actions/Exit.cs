using System;
using System.Collections.Generic;
using System.Text;

namespace StatBringers.Actions
{
    class Exit : IAction        
    {
        public string Description { get; } = "Exit";

        public void Do()
        {
            Environment.Exit(0);
        }
    }
}
