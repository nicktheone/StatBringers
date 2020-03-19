using System;

namespace StatBringers.Actions
{
    class Exit : IAction        
    {
        public string Description { get; } = "Exit";

        public void Do()
        {
            Console.WriteLine();
            Console.WriteLine("Press any to key to exit");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
