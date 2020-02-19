using System;

namespace StatBringers
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var lodestone = new Lodestone();
            //Console.WriteLine(await lodestone.GetCharacterInfo(12213377, ""));
            //Console.WriteLine(await lodestone.GetHeaders(12213377, ""));

            Console.ReadLine();
        }
    }
}
