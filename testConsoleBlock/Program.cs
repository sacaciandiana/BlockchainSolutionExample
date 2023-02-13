using System;
namespace BlockchainExample {
    internal class Program {
        static void Main(string[] args)
        {
            BlockchainExample.Block block = new BlockchainExample.Block(0, DateTime.Now, "1", "2", 3);
            Console.WriteLine(block.Hash);
            Console.WriteLine(block.ToString());
        }
    
    }
}