using System;

using Fido2Net;

namespace Manifest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var devlist = new FidoDeviceInfoList(64)) {
                foreach (var di in devlist) {
                    Console.WriteLine(di);
                }
            }
        }
    }
}
