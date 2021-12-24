using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Fido2Net.Interop
{
    public struct fido_credman_metadata_t
    {

    }

    public struct fido_credman_rk_t
    {

    }

    public struct fido_credman_rp_t
    {

    }


    public static unsafe partial class Native
    {
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstStringMarshaler))]
        public static extern string fido_credman_rp_id(fido_credman_rp_t* rp, UIntPtr index);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstStringMarshaler))]
        public static extern string fido_credman_rp_name(fido_credman_rp_t* rp, UIntPtr index);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern fido_cred_t* fido_credman_rk(fido_credman_rk_t* rk, UIntPtr index);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstStringMarshaler))]
        public static extern string fido_credman_rp_id_hash_ptr(fido_credman_rp_t* rp, UIntPtr index);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern fido_credman_metadata_t* fido_credman_metadata_new();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern fido_credman_rk_t* fido_credman_rk_new();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern fido_credman_rp_t* fido_credman_rp_new();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_credman_del_dev_rk(fido_dev_t* device, string cred_id,
            UIntPtr cred_id_len, string pin);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_credman_get_dev_metadata(fido_dev_t* device, fido_credman_metadata_t* metadata,
            string pin);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_credman_get_dev_rk(fido_dev_t* device, fido_credman_rk_t* rk,
            string pin);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_credman_get_dev_rp(fido_dev_t* device, fido_credman_rp_t* rp,
            string pin);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_credman_set_dev_rk(fido_dev_t* device, fido_cred_t* cred,
            string pin);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr fido_credman_rk_count(fido_credman_rk_t* rk);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr fido_credman_rp_count(fido_credman_rp_t* rp);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr fido_credman_rp_id_hash_len(fido_credman_rp_t* rp, UIntPtr index);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong fido_credman_rk_existing(fido_credman_metadata_t* metadata);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong fido_credman_rk_remaining(fido_credman_metadata_t* metadata);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void fido_credman_metadata_free(fido_credman_metadata_t* metadata);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void fido_credman_rk_free(fido_credman_rk_t* rk);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void fido_credman_rp_free(fido_credman_rp_t* rp);
    }
}
