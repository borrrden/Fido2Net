using System;

using Fido2Net;

namespace SetPin
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2 || args.Length > 3) {
                throw new InvalidOperationException("Incorrect number of arguments!  Usage:  SetPin <pin> [oldpin] <device>");
            }

            if (args.Length == 2) {
                PerformSetPin(args[1], args[0], null);
            } else {
                PerformSetPin(args[2], args[0], args[1]);
            }
        }

        private static void PerformSetPin(string device, string newPin, string oldPin)
        {
            using (var dev = new FidoDevice()) {
                dev.Open(device);
                dev.SetPin(oldPin, newPin);
            }
        }
    }
}
