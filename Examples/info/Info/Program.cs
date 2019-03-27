using System;

using Fido2Net;

namespace Info
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1) {
                throw new InvalidOperationException("Incorrect number of arguments!  Usage:  Info <device>");
            }

            using (var dev = new FidoDevice()) {
                dev.Open(args[0]);
                if (!dev.IsFido2) {
                    return;
                }

                using (var ci = dev.GetCborInfo()) {
                    Console.WriteLine(dev);
                    Console.WriteLine(ci);
                }
            }
        }
    }
}
