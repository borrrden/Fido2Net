using System;

using Fido2Net;

namespace Retries
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1) {
                throw new InvalidOperationException("Incorrect number of arguments!  Usage:  Retries <device>");
            }

            using (var dev = new FidoDevice()) {
                dev.Open(args[0]);
                Console.WriteLine(dev.RetryCount);
            }
        }
    }
}
