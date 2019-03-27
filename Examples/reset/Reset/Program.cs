using System;

using Fido2Net;

namespace Reset
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1) {
                throw new InvalidOperationException("Incorrect number of arguments!  Usage:  Reset <device>");
            }

            using (var dev = new FidoDevice()) {
                dev.Open(args[0]);
                dev.Reset();
            }
        }
    }
}
