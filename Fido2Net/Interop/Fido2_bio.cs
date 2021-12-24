using System;
using System.Runtime.InteropServices;

namespace Fido2Net.Interop
{
    public struct fido_bio_template_t
    {

    }

    public struct fido_bio_template_array_t
    {

    }

    public struct fido_bio_enroll_t
    {

    }

    public struct fido_bio_info_t
    {

    }

    public static unsafe partial class Native
    {
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstStringMarshaler))]
        public static extern string fido_bio_template_name(fido_bio_template_t* ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern fido_bio_template_t* fido_bio_template(fido_bio_template_array_t* ptr, UIntPtr size);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte* fido_bio_template_id_ptr(fido_bio_template_t* ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern fido_bio_enroll_t* fido_bio_enroll_new();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern fido_bio_info_t* fido_bio_info_new();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern fido_bio_template_array_t* fido_bio_template_array_new();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern fido_bio_template_t* fido_bio_template_new();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_bio_dev_enroll_begin(fido_dev_t* device, fido_bio_template_t* template,
            fido_bio_enroll_t* enroll, uint timeout_ms, string pin);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_bio_dev_enroll_cancel(fido_dev_t* device);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_bio_dev_enroll_continue(fido_dev_t* device, fido_bio_template_t* template,
            fido_bio_enroll_t* enroll, uint timeout_ms);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_bio_dev_enroll_remove(fido_dev_t* device, fido_bio_template_t* template,
            string pin);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_bio_dev_get_info(fido_dev_t* device, fido_bio_info_t* info);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_bio_dev_get_template_array(fido_dev_t* device, fido_bio_template_array_t* array,
            string pin);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_bio_dev_set_template_name(fido_dev_t* device, fido_bio_template_t* template,
            string pin);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_bio_template_set_id(fido_bio_template_t* template, string ptr, UIntPtr len);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_bio_template_set_name(fido_bio_template_t* template, string name);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr fido_bio_template_array_count(fido_bio_template_array_t* array);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr fido_bio_template_id_len(fido_bio_template_t* template);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte fido_bio_enroll_last_status(fido_bio_enroll_t* enroll);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte fido_bio_enroll_remaining_samples(fido_bio_enroll_t* enroll);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte fido_bio_enroll_max_samples(fido_bio_enroll_t* enroll);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte fido_bio_info_type(fido_bio_info_t* info);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void fido_bio_enroll_free(fido_bio_enroll_t* enroll);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void fido_bio_info_free(fido_bio_info_t* info);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void fido_bio_template_array_free(fido_bio_template_array_t* array);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void fido_bio_template_free(fido_bio_template_t* template);
    }
}
