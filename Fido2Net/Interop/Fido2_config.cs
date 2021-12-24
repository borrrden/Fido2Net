using System;
using System.Runtime.InteropServices;

namespace Fido2Net.Interop
{
    public static unsafe partial class Native
    {
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_dev_enable_entattest(fido_dev_t* device, string pin);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_dev_force_pin_change(fido_dev_t* device, string pin);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_dev_toggle_always_uv(fido_dev_t * device, string pin);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_dev_set_pin_minlen(fido_dev_t* device, UIntPtr len, string pin);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_dev_set_pin_minlen_rpid(fido_dev_t* device, string rpid,
            UIntPtr n, string pin);
    }
}
