using System;
using System.Runtime.InteropServices;

namespace Fido2Net.Interop
{
    /// <summary>
    /// The signature for a callback for opening a FIDO2 device
    /// </summary>
    /// <param name="id">The ID of the device to open</param>
    public delegate void fido_dev_io_open_t(string id);

    /// <summary>
    /// The signature for a callback for closing a FIDO2 device
    /// </summary>
    /// <param name="device">The device to close</param>
    public unsafe delegate void fido_dev_io_close_t(void* device);

    /// <summary>
    /// The signature of a callback for reading from a FIDO2 device
    /// </summary>
    /// <param name="device">The device to read from</param>
    /// <param name="buffer">The buffer to read into</param>
    /// <param name="size">The size of the passed buffer</param>
    /// <param name="wait">The number of millisecond to wait before giving up (-1 for infinite)</param>
    /// <returns>The number of bytes read, or -1 on error</returns>
    public unsafe delegate int fido_dev_io_read_t(void* device, byte* buffer, UIntPtr size, int wait);

    /// <summary>
    /// The signature of a callback for writing to a FIDO2 device
    /// </summary>
    /// <param name="device">The device to write to</param>
    /// <param name="buffer">The buffer to write</param>
    /// <param name="size">The size of the buffer to write</param>
    /// <returns>The number of bytes written, or -1 on error</returns>
    public unsafe delegate int fido_dev_io_write_t(void* device, byte* buffer, UIntPtr size);

    /// <summary>
    /// FIDO assertion handle
    /// </summary>
    public struct fido_assert_t
    {

    }

    /// <summary>
    /// FIDO credential handle
    /// </summary>
    public struct fido_cred_t
    {

    }

    /// <summary>
    /// FIDO device handle
    /// </summary>
    public struct fido_dev_t
    {

    }

    /// <summary>
    /// FIDO device info handle
    /// </summary>
    public struct fido_dev_info_t
    {

    }

    /// <summary>
    /// FIDO extended device info handle
    /// </summary>
    public struct fido_cbor_info_t
    {

    }

    public enum fido_opt_t 
    {
        /// <summary>
        /// Use authenticator's default
        /// </summary>
        FIDO_OPT_OMIT = 0,

        /// <summary>
        /// Explicitly set option to false
        /// </summary>
        FIDO_OPT_FALSE = 1,

        /// <summary>
        /// Explicitly set option to true
        /// </summary>
        FIDO_OPT_TRUE = 2
    }

    /// <summary>
    /// The I/O handlers used to talk to a device. Its usage is optional. By default, 
    /// libfido2 will use libhidapi to talk to a FIDO device.
    /// </summary>
    public struct fido_dev_io_t
    {
        private IntPtr _open;
        private IntPtr _close;
        private IntPtr _read;
        private IntPtr _write;

        /// <summary>
        /// The callback for opening a device
        /// </summary>
        public fido_dev_io_open_t open
        {
            get => Marshal.GetDelegateForFunctionPointer<fido_dev_io_open_t>(_open);
            set => _open = Marshal.GetFunctionPointerForDelegate(value);
        }

        /// <summary>
        /// The callback for closing a device
        /// </summary>
        public fido_dev_io_close_t close
        {
            get => Marshal.GetDelegateForFunctionPointer<fido_dev_io_close_t>(_open);
            set => _close = Marshal.GetFunctionPointerForDelegate(value);
        }

        /// <summary>
        /// The callback for reading from a device
        /// </summary>
        public fido_dev_io_read_t read
        {
            get => Marshal.GetDelegateForFunctionPointer<fido_dev_io_read_t>(_open);
            set => _read = Marshal.GetFunctionPointerForDelegate(value);
        }

        /// <summary>
        /// The callback for writing to a device
        /// </summary>
        public fido_dev_io_write_t write
        {
            get => Marshal.GetDelegateForFunctionPointer<fido_dev_io_write_t>(_open);
            set => _write = Marshal.GetFunctionPointerForDelegate(value);
        }
    }
    
    public struct fido_log_handler_t
    {

    }

    /// <summary>
    /// P/Invoke methods
    /// </summary>
    public static unsafe partial class Native
    {
        private const string DllName = "fido2";

        /// <summary>
        /// Returns a pointer to a newly allocated, empty fido_assert_t type. 
        /// If memory cannot be allocated, <c>null</c> is returned
        /// </summary>
        /// <returns>A newly allocated, empty fido_assert_t type</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern fido_assert_t *fido_assert_new();

        /// <summary>
        /// Returns a pointer to a newly allocated, empty fido_cred_t type. 
        /// If memory cannot be allocated, <c>null</c> is returned.
        /// </summary>
        /// <returns>A newly allocated, empty fido_cred_t type</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern fido_cred_t *fido_cred_new();

        /// <summary>
        /// Returns a pointer to a newly allocated, empty fido_dev_t type. 
        /// If memory cannot be allocated, <c>null</c> is returned.
        /// </summary>
        /// <returns>A newly allocated, empty fido_dev_t type</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern fido_dev_t *fido_dev_new();

        /// <summary>
        /// Returns a pointer to a newly allocated, empty fido_dev_info_t type. 
        /// If memory cannot be allocated, <c>null</c> is returned.
        /// </summary>
        /// <returns>A newly allocated, empty fido_dev_info_t type</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern fido_dev_info_t *fido_dev_info_new(UIntPtr n);

        /// <summary>
        /// Returns a pointer to a newly allocated, empty fido_cbor_info_t type. 
        /// If memory cannot be allocated, <c>null</c> is returned.
        /// </summary>
        /// <returns>A newly allocated, empty fido_cbor_info_t type</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern fido_cbor_info_t *fido_cbor_info_new();

        /// <summary>
        /// Releases the memory backing *assert_p, where *assert_p must have been previously allocated by <see cref="fido_assert_new"/>. 
        /// On return, *assert_p is set to <c>null</c>. Either assert_p or *assert_p may be <c>null</c>, in which case fido_assert_free() is a NOP.
        /// </summary>
        /// <param name="assert_p">The object to free</param>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void fido_assert_free(fido_assert_t ** assert_p);

        /// <summary>
        /// Releases the memory backing *ci_p, where *ci_p must have been previously allocated by <see cref="fido_cbor_info_new"/>. 
        /// On return, *ci_p is set to <c>null</c>. Either ci_p or *ci_p may be <c>null</c>, in which case fido_cbor_info_free() is a NOP.
        /// </summary>
        /// <param name="ci_p">The object to free</param>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void fido_cbor_info_free(fido_cbor_info_t ** ci_p);

        /// <summary>
        /// Releases the memory backing *cred_p, where *cred_p must have been previously allocated by <see cref="fido_cred_new"/>. 
        /// On return, *cred_p is set to <c>null</c>. Either cred_p or *cred_p may be <c>null</c>, in which case fido_cred_free() is a NOP.
        /// </summary>
        /// <param name="cred_p">The object to free</param>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void fido_cred_free(fido_cred_t ** cred_p);

        /// <summary>
        /// Forces the given device to use FIDO2 mode
        /// </summary>
        /// <param name="dev">The device to act on</param>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void fido_dev_force_fido2(fido_dev_t *dev);

        /// <summary>
        /// Forces the given device to use U2F mode
        /// </summary>
        /// <param name="dev">The device to act on</param>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void fido_dev_force_u2f(fido_dev_t *dev);

        /// <summary>
        /// Releases the memory backing *dev_p, where *dev_p must have been previously allocated by <see cref="fido_dev_new"/>. 
        /// On return, *dev_p is set to <c>null</c>. Either dev_p or *dev_p may be <c>null</c>, in which case fido_dev_free() is a NOP.
        /// </summary>
        /// <param name="dev_p"></param>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void fido_dev_free(fido_dev_t **dev_p);

        /// <summary>
        /// Releases the memory backing *devlist_p, where *devlist_p must have been previously allocated by <see cref="fido_dev_info_new(UIntPtr)"/>. 
        /// On return, *devlist_p is set to <c>null</c>. Either devlist_p or *devlist_p may be <c>null</c>, in which case fido_dev_info_free() is a NOP.
        /// </summary>
        /// <param name="devlist_p"></param>
        /// <param name="n">The number of entries this object was allocated to hold</param>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void fido_dev_info_free(fido_dev_info_t **devlist_p, UIntPtr n);

        /// <summary>
        /// The fido_init() function initialises the libfido2 library. 
        /// Its invocation must precede that of any other libfido2 function. 
        /// If FIDO_DEBUG is set in flags, then debug output will be emitted by libfido2 on stderr. 
        /// Alternatively, the FIDO_DEBUG environment variable may be set. 
        /// Please note that debug output is conditional on _FIDO_DEBUG being defined when the library was compiled.
        /// </summary>
        /// <param name="flags">The flags to use during initialization</param>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void fido_init(int flags);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void fido_set_log_handler(fido_log_handler_t log_handler);

        /// <summary>
        /// Returns a pointer to the authenticator data of statement idx in assert
        /// </summary>
        /// <param name="assert">The assertion object to act on</param>
        /// <param name="idx">The index to retrieve</param>
        /// <returns>A pointer to the authenticator data of statement idx in assert</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte *fido_assert_authdata_ptr(fido_assert_t *assert, UIntPtr idx);

        /// <summary>
        /// Returns a pointer to the client data hash of assert
        /// </summary>
        /// <param name="assert">The assertion object to act on</param>
        /// <returns>A pointer to the client data hash of assert</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte *fido_assert_clientdata_hash_ptr(fido_assert_t *assert);

        /// <summary>
        /// Returns a pointer to the hmac-secret of statement idx in assert
        /// </summary>
        /// <param name="assert">The assertion object to act on</param>
        /// <param name="idx">The index to retrieve</param>
        /// <returns>A pointer to hmac-secret of statement idx in assert</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte *fido_assert_hmac_secret_ptr(fido_assert_t *assert, UIntPtr idx);

        /// <summary>
        /// Returns a pointer to the ID of statement idx in assert
        /// </summary>
        /// <param name="assert">The assertion object to act on</param>
        /// <param name="idx">The index to retrieve</param>
        /// <returns>A pointer to the ID of statement idx in assert</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte *fido_assert_id_ptr(fido_assert_t *assert, UIntPtr idx);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte* fido_assert_largeblob_key_ptr(fido_assert_t*assert, UIntPtr idx);

        /// <summary>
        /// Returns a pointer to the signature of statement idx in assert
        /// </summary>
        /// <param name="assert">The assertion object to act on</param>
        /// <param name="idx">The index to retrieve</param>
        /// <returns>A pointer to the signatureof statement idx in assert</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte *fido_assert_sig_ptr(fido_assert_t *assert, UIntPtr idx);

        /// <summary>
        /// Returns a pointer to the user ID of statement idx in assert
        /// </summary>
        /// <param name="assert">The assertion object to act on</param>
        /// <param name="idx">The index to retrieve</param>
        /// <returns>A pointer to the user ID of statement idx in assert</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte *fido_assert_user_id_ptr(fido_assert_t *assert, UIntPtr idx);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte* fido_assert_blob_ptr(fido_assert_t* assert, UIntPtr idx);

        /// <summary>
        /// Returns a pointer to the extensions of ci
        /// </summary>
        /// <param name="ci">The object to act on</param>
        /// <returns>A pointer to the extensions of ci</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte **fido_cbor_info_extensions_ptr(fido_cbor_info_t *ci);

        /// <summary>
        /// Returns a pointer to the options dictionary names in ci
        /// </summary>
        /// <param name="ci">The object to act on</param>
        /// <returns>A pointer to the options dictionary names of ci</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte **fido_cbor_info_options_name_ptr(fido_cbor_info_t *ci);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte** fido_cbor_info_transports_ptr(fido_cbor_info_t* ci);

        /// <summary>
        /// Returns a pointer to the versions of ci
        /// </summary>
        /// <param name="ci">The object to act on</param>
        /// <returns>A pointer to the versions of ci</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte **fido_cbor_info_versions_ptr(fido_cbor_info_t *ci);

        /// <summary>
        /// Returns a pointer to the options dictionary values of ci
        /// </summary>
        /// <param name="ci">The object to act on</param>
        /// <returns>A pointer to the options dictionary values of ci</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte /* bool */ *fido_cbor_info_options_value_ptr(fido_cbor_info_t *ci);

        /// <summary>
        /// Returns a pointer to the relying party of assert
        /// </summary>
        /// <param name="assert">The object to act on</param>
        /// <returns>A pointer to the relying party of assert</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstStringMarshaler))]
		public static extern string fido_assert_rp_id(fido_assert_t *assert);

        /// <summary>
        /// Returns a pointer to the relying party of statement idx in assert
        /// </summary>
        /// <param name="assert">The object to act on</param>
        /// <param name="idx">The index to retrieve</param>
        /// <returns>A pointer to the relying party of assert</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstStringMarshaler))]
		public static extern string fido_assert_user_display_name(fido_assert_t *assert, UIntPtr idx);

        /// <summary>
        /// Returns a pointer to the user icon of statement idx in assert
        /// </summary>
        /// <param name="assert">The object to act on</param>
        /// <param name="idx">The index to retrieve</param>
        /// <returns>A pointer to the user icon of assert</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstStringMarshaler))]
		public static extern string fido_assert_user_icon(fido_assert_t *assert, UIntPtr idx);

        /// <summary>
        /// Returns a pointer to the user name of statement idx in assert
        /// </summary>
        /// <param name="assert">The object to act on</param>
        /// <param name="idx">The index to retrieve</param>
        /// <returns>A pointer to the user name of assert</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstStringMarshaler))]
		public static extern string fido_assert_user_name(fido_assert_t *assert, UIntPtr idx);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstStringMarshaler))]
        public static extern string fido_cbor_info_algorithm_type(fido_cbor_info_t* ci, UIntPtr index);

        /// <summary>
        /// Returns a pointer to the format of cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <returns>A pointer to the format of cred</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstStringMarshaler))]
		public static extern string fido_cred_fmt(fido_cred_t *cred);

        /// <summary>
        /// Returns a pointer to the relying party ID of cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <returns>A pointer to the relying party ID of cred</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstStringMarshaler))]
		public static extern string fido_cred_rp_id(fido_cred_t *cred);

        /// <summary>
        /// Returns a pointer to the relying party name of cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <returns>A pointer to the relying part name of cred</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstStringMarshaler))]
		public static extern string fido_cred_rp_name(fido_cred_t *cred);

        /// <summary>
        /// Returns a pointer to the manufacturer string of di
        /// </summary>
        /// <param name="di">The object to act on</param>
        /// <returns>A pointer to the manufacturer string of di</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstStringMarshaler))]
		public static extern string fido_dev_info_manufacturer_string(fido_dev_info_t *di);

        /// <summary>
        /// Returns a pointer to the path of di
        /// </summary>
        /// <param name="di">The object to act on</param>
        /// <returns>A pointer to the path of di</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstStringMarshaler))]
		public static extern string fido_dev_info_path(fido_dev_info_t *di);

        /// <summary>
        /// Returns a pointer to the product string of di
        /// </summary>
        /// <param name="di">The object to act on</param>
        /// <returns>A pointer to the product string of di</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstStringMarshaler))]
		public static extern string fido_dev_info_product_string(fido_dev_info_t *di);

        /// <summary>
        /// Returns a pointer to the idx entry of di
        /// </summary>
        /// <param name="di">The object to act on</param>
        /// <param name="idx">The index of the object to retrieve</param>
        /// <returns>A pointer to the idx entry of di</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern fido_dev_info_t *fido_dev_info_ptr(fido_dev_info_t *di, UIntPtr idx);

        /// <summary>
        /// Returns a pointer to the protocols of ci
        /// </summary>
        /// <param name="ci">The object to act on</param>
        /// <returns>A pointer to the protocols of ci</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte *fido_cbor_info_protocols_ptr(fido_cbor_info_t *ci);

        /// <summary>
        /// Returns a pointer to the AAGUID of ci
        /// </summary>
        /// <param name="ci">The object to act on</param>
        /// <returns>A pointer to the AAGUID of ci</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte *fido_cbor_info_aaguid_ptr(fido_cbor_info_t *ci);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte* fido_cred_aaguid_ptr(fido_cred_t* cred);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte *fido_cred_attstmt_ptr(fido_cred_t* cred);

        /// <summary>
        /// Returns a pointer to the authenticator data of cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <returns>A pointer to the authenticator data of cred</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte *fido_cred_authdata_ptr(fido_cred_t *cred);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte* fido_cred_authdata_raw_ptr(fido_cred_t* cred);

        /// <summary>
        /// Returns a pointer to the client data hash of cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <returns>A pointer to the client data hash of cred</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte *fido_cred_clientdata_hash_ptr(fido_cred_t *cred);

        /// <summary>
        /// Returns a pointer to the ID of cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <returns>A pointer to the ID of cred</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte *fido_cred_id_ptr(fido_cred_t *cred);

        /// <summary>
        /// Returns a pointer to the public key of cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <returns>A pointer to the public key of cred</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte *fido_cred_pubkey_ptr(fido_cred_t *cred);

        /// <summary>
        /// Returns a pointer to the signature of cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <returns>A pointer to the signature of cred</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte *fido_cred_sig_ptr(fido_cred_t *cred);

        /// <summary>
        /// Returns a pointer to the attestation certificate of cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <returns>A pointer to the attestation of cred</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte *fido_cred_x5c_ptr(fido_cred_t *cred);

        /// <summary>
        /// Adds ptr to the list of credentials allowed in assert, where ptr points to a credential ID of len bytes. 
        /// A copy of ptr is made, and no references to the passed pointer are kept. 
        /// If this call fails, the existing list of allowed credentials is preserved.
        /// </summary>
        /// <param name="assert">The object to act on</param>
        /// <param name="ptr">A pointer to the ID of the credential to allow</param>
        /// <param name="len">The length of the data inside of <paramref name="ptr"/></param>
        /// <returns></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_assert_allow_cred(fido_assert_t* assert, byte* ptr, UIntPtr len);

        /// <summary>
        /// Set the authenticator data of statement idx in assert
        /// </summary>
        /// <param name="assert">The assertion object to act on</param>
        /// <param name="idx">The index to set</param>
        /// <param name="ptr">The authenticator data to set</param>
        /// <param name="len">The length of the data in <paramref name="ptr"/></param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_assert_set_authdata(fido_assert_t *assert, UIntPtr idx, byte *ptr,
    UIntPtr len);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_assert_set_authdata_raw(fido_assert_t* assert, UIntPtr idx, byte* ptr,
    UIntPtr len);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_cred_set_blob(fido_cred_t* cred, byte* blob, UIntPtr len);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_assert_set_clientdata(fido_assert_t* assert, byte* data,
            UIntPtr len);

        /// <summary>
        /// Set the client data hash of assert
        /// </summary>
        /// <param name="assert">The assertion object to act on</param>
        /// <param name="ptr">The client data hash to set</param>
        /// <param name="len">The length of the data in <paramref name="ptr"/></param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_assert_set_clientdata_hash(fido_assert_t *assert, byte *ptr,
    UIntPtr len);

        /// <summary>
        /// Sets the number of assertion statements contained in assert
        /// </summary>
        /// <param name="assert">The assertion object to act on</param>
        /// <param name="n">The new number of assertion statements</param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_assert_set_count(fido_assert_t *assert, UIntPtr n);

        /// <summary>
        /// Sets the extensions of assert
        /// </summary>
        /// <param name="assert">The assertion object to act on</param>
        /// <param name="ext">The extensions to set</param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_assert_set_extensions(fido_assert_t *assert, FidoExtensions ext);

        /// <summary>
        /// Sets the hmac salt of assert
        /// </summary>
        /// <param name="assert">The assertion object to act on</param>
        /// <param name="salt">A pointer to the hmac salt to set</param>
        /// <param name="salt_len">The length of the data in <paramref name="salt"/></param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_assert_set_hmac_salt(fido_assert_t *assert, byte *salt, UIntPtr salt_len);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_assert_set_hmac_secret(fido_assert_t* assert, UIntPtr idx,
            byte* secret, UIntPtr secret_len);

        /// <summary>
        /// Sets the options of assert
        /// </summary>
        /// <param name="assert">The assertion object to act on</param>
        /// <param name="up">Whether or not to require user presence for the generation process</param>
        /// <param name="uv">Whether or not to require user verification for the generation process</param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_assert_set_options(fido_assert_t *assert, [MarshalAs(UnmanagedType.U1)]bool up, 
            [MarshalAs(UnmanagedType.U1)]bool uv);

        /// <summary>
        /// Sets the relying party of assert
        /// </summary>
        /// <param name="assert">The assertion object to act on</param>
        /// <param name="id">The ID of the the relying party</param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_assert_set_rp(fido_assert_t *assert, string id);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_assert_set_up(fido_assert_t* assert, fido_opt_t val);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_assert_set_uv(fido_assert_t* assert, fido_opt_t val);

        /// <summary>
        /// Set the client data hash of statement idx in assert
        /// </summary>
        /// <param name="assert">The assertion object to act on</param>
        /// <param name="idx">The index to set</param>
        /// <param name="ptr">The signature to set</param>
        /// <param name="len">The length of the data in <paramref name="ptr"/></param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_assert_set_sig(fido_assert_t *assert, UIntPtr idx, byte *ptr, UIntPtr len);

        /// <summary>
        /// Verifies whether the client data hash, relying party ID, user presence and user verification attributes 
        /// of assert have been attested by the holder of the private counterpart of the public key pk of COSE type cose_alg, 
        /// where cose_alg is either <see cref="FidoCose.ES256"/> or <see cref="FidoCose.RS256"/> and pk points to a 
        /// es256_pk_t or rs256_pk_t type accordingly.
        /// </summary>
        /// <param name="assert">The assertion object to act on</param>
        /// <param name="idx">The index to set</param>
        /// <param name="cose_alg">The algorithm to use during verification</param>
        /// <param name="pk">The public key to use during verification</param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_assert_verify(fido_assert_t *assert, UIntPtr idx, FidoCose cose_alg, void *pk);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_cbor_info_algorithm_cose(fido_cbor_info_t* ci, UIntPtr index);

        /// <summary>
        /// Adds ptr to the list of credentials excluded by cred, where ptr points to a credential ID of len bytes. 
        /// A copy of ptr is made, and no references to the passed pointer are kept. 
        /// If this function fails, the existing list of excluded credentials is preserved.
        /// </summary>
        /// <param name="cred">The credential object to act on</param>
        /// <param name="id_ptr">A pointer to the id of the credential to exclude</param>
        /// <param name="id_len">The length of the data in <paramref name="id_ptr"/></param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_cred_exclude(fido_cred_t *cred, byte *id_ptr, UIntPtr id_len);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_cred_prot(fido_cred_t* cred);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_cred_set_attstmt(fido_cred_t* cred, byte* ptr, UIntPtr len);

        /// <summary>
        /// Sets the authenticator data of cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <param name="ptr">A pointer to the authenticator data</param>
        /// <param name="len">The length of the data in <paramref name="ptr"/></param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_cred_set_authdata(fido_cred_t *cred, byte *ptr, UIntPtr len);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_cred_set_authdata_raw(fido_cred_t* cred, byte* ptr, UIntPtr len);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_cred_set_clientdata(fido_cred_t* cred, byte* ptr, UIntPtr len);

        /// <summary>
        /// Sets the client data hash of cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <param name="hash">A pointer to the client data hash</param>
        /// <param name="hash_len">The length of the data in <paramref name="hash"/></param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_cred_set_clientdata_hash(fido_cred_t *cred, byte *hash, UIntPtr hash_len);

        /// <summary>
        /// Sets the extensions of cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <param name="ext">The extensions to set</param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_cred_set_extensions(fido_cred_t *cred, FidoExtensions ext);

        /// <summary>
        /// Sets the format of cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <param name="fmt">The format to set</param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_cred_set_fmt(fido_cred_t *cred, string fmt);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_cred_set_id(fido_cred_t* cred, byte* ptr, UIntPtr len);

        /// <summary>
        /// Sets the options of cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <param name="rk">Whether or not to attempt to store key material onto the device</param>
        /// <param name="uv">Whether or not to request user verification during the generation process</param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_cred_set_options(fido_cred_t *cred, [MarshalAs(UnmanagedType.U1)]bool rk, 
            [MarshalAs(UnmanagedType.U1)]bool uv);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_cred_set_pin_minlen(fido_cred_t* cred, UIntPtr len);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_cred_set_prot(fido_cred_t* cred, int prot);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_cred_set_rk(fido_cred_t* cred, fido_opt_t val);

        /// <summary>
        /// Sets the relying party name of cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <param name="id">The relying party ID to set</param>
        /// <param name="name">The relying party name to set</param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_cred_set_rp(fido_cred_t *cred, string id, string name);

        /// <summary>
        /// Sets the signature of cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <param name="ptr">A pointer to the signature to set</param>
        /// <param name="len">The length of the data in <paramref name="ptr"/></param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_cred_set_sig(fido_cred_t *cred, byte *ptr, UIntPtr len);

        /// <summary>
        /// Sets the algorithm type of cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <param name="cose_alg">The algorithm type to set</param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_cred_set_type(fido_cred_t *cred, FidoCose cose_alg);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_cred_set_uv(fido_cred_t* cred, fido_opt_t val);

        /// <summary>
        /// Sets the user data of cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <param name="user_id">A pointer to the user ID data</param>
        /// <param name="user_id_len">The length of the data in <paramref name="user_id"/></param>
        /// <param name="name">The user name</param>
        /// <param name="display_name">The user display name</param>
        /// <param name="icon">The user icon identifier (e.g. url)</param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_cred_set_user(fido_cred_t *cred, byte *user_id, UIntPtr user_id_len,
    string name, string display_name, string icon);

        /// <summary>
        /// Sets the attestation certificate of cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <param name="ptr">A pointer to the attestation certificate to set</param>
        /// <param name="len">The length of the data in <paramref name="ptr"/></param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_cred_set_x509(fido_cred_t *cred, byte *ptr, UIntPtr len);

        /// <summary>
        /// Verifies whether the client data hash, relying party ID, credential ID, type, and resident key and user verification attributes 
        /// of cred have been attested by the holder of the private counterpart of the public key contained in the credential's 
        /// x509 certificate.
        /// </summary>
        /// <param name="cred">The credential to act on</param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_cred_verify(fido_cred_t *cred);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_dev_cancel(fido_dev_t* device);

        /// <summary>
        /// Closes the device represented by dev. If dev is already closed, this is a NOP.
        /// </summary>
        /// <param name="dev">The device to close</param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_dev_close(fido_dev_t *dev);

        /// <summary>
        /// <para>Asks the FIDO device represented by dev for an assertion according to the following parameters defined in assert:
        /// relying party ID;</para>
        /// client data hash;
        /// list of allowed credential IDs;
        /// user presence and user verification attributes.
        /// <para>See fido_assert_set(3) for information on how these values are set.</para>
        /// <para>If a PIN is not needed to authenticate the request against dev, then pin may be NULL.  Otherwise pin must point to a NUL-terminated UTF-8 string.</para>
        /// <para>After a successful call, the <see cref="fido_assert_count(fido_assert_t*)"/>, <see cref="fido_assert_user_display_name(fido_assert_t*, UIntPtr)"/>, 
        /// <see cref="fido_assert_user_icon(fido_assert_t*, UIntPtr)"/>, <see cref="fido_assert_user_name(fido_assert_t*, UIntPtr)"/>, 
        /// <see cref="fido_assert_authdata_ptr(fido_assert_t*, UIntPtr)"/>, <see cref="fido_assert_user_id_ptr(fido_assert_t*, UIntPtr)"/>, and <see cref="fido_assert_sig_ptr(fido_assert_t*, UIntPtr)"/> 
        /// functions may be invoked on assert to retrieve the various attributes of the generated assertion.</para>
        /// <para>Please note that fido_dev_get_assert() is synchronous and will block if necessary.</para>
        /// </summary>
        /// <param name="dev">The device to use for generation</param>
        /// <param name="assert">The assert to use for generation</param>
        /// <param name="pin">The pin of the device</param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_dev_get_assert(fido_dev_t *dev, fido_assert_t *assert, string pin);

        /// <summary>
        /// Gets the extended information of the device
        /// </summary>
        /// <param name="dev">The device to get the info from</param>
        /// <param name="ci">A pointer to the result.  It will be populated on success</param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_dev_get_cbor_info(fido_dev_t *dev, fido_cbor_info_t *ci);

        /// <summary>
        /// Fills retries with the number of PIN retries left in dev before lock-out, where retries is an addressable pointer.
        /// </summary>
        /// <param name="dev">The device to act on</param>
        /// <param name="retries">A pointer to the location to store the result</param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_dev_get_retry_count(fido_dev_t *dev, int *retries);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_dev_get_uv_retry_count(fido_dev_t* device, int* retries);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_dev_get_touch_begin(fido_dev_t* device);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_dev_get_touch_status(fido_dev_t* device);

        /// <summary>
        /// Fills devlist with up to ilen FIDO devices found by the underlying operating system. 
        /// Currently only USB HID devices are supported. 
        /// The number of discovered devices is returned in olen, where olen is an addressable pointer.
        /// </summary>
        /// <param name="devlist">The devlist pointer to store the result in</param>
        /// <param name="ilen">The number of entries that the list can hold</param>
        /// <param name="olen">A pointer to where the number of entries that were written will be stored</param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_dev_info_manifest(fido_dev_info_t *devlist, UIntPtr ilen, UIntPtr *olen);

        /// <summary>
        /// <para>Asks the FIDO device represented by dev to generate a new credential according to the following parameters defined in cred:</para>
        /// type;
        /// client data hash;
        /// relying party;
        /// user attributes;
        /// list of excluded credential IDs;
        /// resident key and user verification attributes.
        /// <para>If a PIN is not needed to authenticate the request against dev, then pin may be NULL.Otherwise pin must point to a NUL-terminated UTF-8 string.</para>
        /// <para>After a successful call, the <see cref="fido_cred_authdata_ptr(fido_cred_t*)"/>, <see cref="fido_cred_pubkey_ptr(fido_cred_t*)"/>,
        /// <see cref="fido_cred_x5c_ptr(fido_cred_t*)"/>, and <see cref="fido_cred_sig_ptr(fido_cred_t*)"/> functions may be 
        /// invoked on cred to retrieve the various parts of the generated credential.</para>
        /// <para>Please note that this call is synchronous and will block if necessary.</para>
        /// </summary>
        /// <param name="dev">The device to act on</param>
        /// <param name="cred">The credential to ac on</param>
        /// <param name="pin">The pin of the device</param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_dev_make_cred(fido_dev_t *dev, fido_cred_t *cred, string pin);

        /// <summary>
        /// Opens the device pointed to by path, where dev is a freshly allocated or otherwise closed fido_dev_t.
        /// </summary>
        /// <param name="dev">The device handle to store the result</param>
        /// <param name="path">The unique path to the device</param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_dev_open(fido_dev_t *dev, string path);

        /// <summary>
        /// Performs a reset on dev, resetting the device's PIN and erasing credentials stored on the device.
        /// </summary>
        /// <param name="dev">The device to act on</param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_dev_reset(fido_dev_t *dev);

        /// <summary>
        /// Defines the I/O handlers used to talk to dev. Its usage is optional. 
        /// By default, libfido2 will use libhidapi to talk to a FIDO device.
        /// </summary>
        /// <param name="dev">The device to act on</param>
        /// <param name="io">The I/O interface to use when interacting with the device</param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_dev_set_io_functions(fido_dev_t *dev, fido_dev_io_t *io);

        /// <summary>
        /// Sets the PIN of device dev to pin, where pin is a NUL-terminated UTF-8 string. 
        /// If oldpin is not <c>null</c>, the device's PIN is changed from oldpin to pin, 
        /// where pin and oldpin are NUL-terminated UTF-8 strings.
        /// </summary>
        /// <param name="dev">The device to act on</param>
        /// <param name="pin">The pin to set</param>
        /// <param name="oldpin">The existing pin</param>
        /// <returns><see cref="CtapStatus.Ok"/> on success, anything else on failure</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int fido_dev_set_pin(fido_dev_t *dev, string pin, string oldpin);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_dev_set_timeout(fido_dev_t* device, int timeout);

        /// <summary>
        /// Returns the length of the authenticator data of statement idx in assert
        /// </summary>
        /// <param name="assert">The assertion object to act on</param>
        /// <param name="idx">The index to retrieve</param>
        /// <returns>The length of the authenticator data of statement idx in assert</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr fido_assert_authdata_len(fido_assert_t *assert, UIntPtr idx);

        /// <summary>
        /// Returns the length of the client data hash of assert
        /// </summary>
        /// <param name="assert">The assertion object to act on</param>
        /// <returns>The length of the client data hash of statement idx in assert</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr fido_assert_clientdata_hash_len(fido_assert_t *assert);

        /// <summary>
        /// Gets the number of statements in this assertion
        /// </summary>
        /// <param name="assert">The assert to act on</param>
        /// <returns>The number of statements in this assertion</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr fido_assert_count(fido_assert_t *assert);

        /// <summary>
        /// Returns the length of the hmac-secret of statement idx in assert
        /// </summary>
        /// <param name="assert">The assertion object to act on</param>
        /// <param name="idx">The index to retrieve</param>
        /// <returns>The length of the hmac-secret of statement idx in assert</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr fido_assert_hmac_secret_len(fido_assert_t *assert, UIntPtr idx);

        /// <summary>
        /// Returns the length of the ID of statement idx in assert
        /// </summary>
        /// <param name="assert">The assertion object to act on</param>
        /// <param name="idx">The index to retrieve</param>
        /// <returns>The length of the ID of statement idx in assert</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr fido_assert_id_len(fido_assert_t *assert, UIntPtr idx);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr fido_assert_largeblob_key_len(fido_assert_t* assert, UIntPtr idx);

        /// <summary>
        /// Returns the length of the signature of statement idx in assert
        /// </summary>
        /// <param name="assert">The assertion object to act on</param>
        /// <param name="idx">The index to retrieve</param>
        /// <returns>The length of the signature of statement idx in assert</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr fido_assert_sig_len(fido_assert_t *assert, UIntPtr idx);

        /// <summary>
        /// Returns the length of the user ID of statement idx in assert
        /// </summary>
        /// <param name="assert">The assertion object to act on</param>
        /// <param name="idx">The index to retrieve</param>
        /// <returns>The length of the user ID of statement idx in assert</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr fido_assert_user_id_len(fido_assert_t *assert, UIntPtr idx);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr fido_assert_blob_len(fido_assert_t* assert, UIntPtr idx);

        /// <summary>
        /// Returns the length of the AAGUID in ci
        /// </summary>
        /// <param name="ci">The object to act on</param>
        /// <returns>The length of the AAGUID in ci</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr fido_cbor_info_aaguid_len(fido_cbor_info_t *ci);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr fido_cbor_info_algorithm_count(fido_cbor_info_t *ci);

        /// <summary>
        /// Returns the length of the extensions in ci
        /// </summary>
        /// <param name="ci">The object to act on</param>
        /// <returns>The length of the extensions in ci</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr fido_cbor_info_extensions_len(fido_cbor_info_t *ci);

        /// <summary>
        /// Returns the length of the options in ci
        /// </summary>
        /// <param name="ci">The object to act on</param>
        /// <returns>The length of the options in ci</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr fido_cbor_info_options_len(fido_cbor_info_t *ci);

        /// <summary>
        /// Returns the length of the protocols in ci
        /// </summary>
        /// <param name="ci">The object to act on</param>
        /// <returns>The length of the protocols in ci</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr fido_cbor_info_protocols_len(fido_cbor_info_t *ci);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr fido_cbor_info_transports_len(fido_cbor_info_t* ci);

        /// <summary>
        /// Returns the length of the versions in ci
        /// </summary>
        /// <param name="ci">The object to act on</param>
        /// <returns>The length of the versions in ci</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr fido_cbor_info_versions_len(fido_cbor_info_t *ci);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr fido_cred_aaguid_len(fido_cred_t* cred);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr fido_cred_attstmt_len(fido_cred_t *cred);

        /// <summary>
        /// Returns the length of the authenticator data in cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <returns>The length of the authenticator data in cred</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr fido_cred_authdata_len(fido_cred_t *cred);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr fido_cred_authdata_raw_len(fido_cred_t* cred);

        /// <summary>
        /// Returns the length of the client data hash in cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <returns>The length of the client data hash in cred</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr fido_cred_clientdata_hash_len(fido_cred_t *cred);

        /// <summary>
        /// Returns the length of the ID in cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <returns>The length of the ID in cred</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr fido_cred_id_len(fido_cred_t *cred);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr fido_cred_pin_minlen(fido_cred_t* cred);

        /// <summary>
        /// Returns the length of the public key in cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <returns>The length of the public key in cred</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr fido_cred_pubkey_len(fido_cred_t *cred);

        /// <summary>
        /// Returns the length of the signature in cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <returns>The length of the signature in cred</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr fido_cred_sig_len(fido_cred_t *cred);

        /// <summary>
        /// Returns the length of the attestation certificate in cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <returns>The length of the attestation certificate in cred</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr fido_cred_x5c_len(fido_cred_t *cred);

        /// <summary>
        /// Gets the flags that are set on statement idx in assert
        /// </summary>
        /// <param name="assert">The object to act on</param>
        /// <param name="idx">The index of the statement to read</param>
        /// <returns>The flags that are set on statement idx in assert</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern FidoAuthFlags fido_assert_flags(fido_assert_t *assert, UIntPtr idx);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint fido_assert_sigcount(fido_assert_t* assert, UIntPtr index);

        /// <summary>
        /// Returns the flags that are set on cred
        /// </summary>
        /// <param name="cred">The object to act on</param>
        /// <returns>The flags that are set on cred</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern FidoAuthFlags fido_cred_flags(fido_cred_t *cred);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint fido_cred_sigcount(fido_cred_t* cred);

        /// <summary>
        /// Returns the protocol of the device
        /// </summary>
        /// <param name="dev">The object to act on</param>
        /// <returns>The protocol of the device</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte fido_dev_protocol(fido_dev_t *dev);

        /// <summary>
        /// Returns the major version of the device
        /// </summary>
        /// <param name="dev">The object to act on</param>
        /// <returns>The major version of the device</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte  fido_dev_major(fido_dev_t *dev);

        /// <summary>
        /// Returns the minor version of the device
        /// </summary>
        /// <param name="dev">The object to act on</param>
        /// <returns>The minor version of the device</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte  fido_dev_minor(fido_dev_t *dev);

        /// <summary>
        /// Returns the build version of the device
        /// </summary>
        /// <param name="dev">The object to act on</param>
        /// <returns>The build version of the device</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte  fido_dev_build(fido_dev_t *dev);

        /// <summary>
        /// Returns the capatbilities of the device
        /// </summary>
        /// <param name="dev">The object to act on</param>
        /// <returns>The capabilities of the device</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern FidoCapabilities fido_dev_flags(fido_dev_t *dev);

        /// <summary>
        /// Returns the vendor of the device
        /// </summary>
        /// <param name="di">The object to act on</param>
        /// <returns>The vendor of the device</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern short  fido_dev_info_vendor(fido_dev_info_t *di);

        /// <summary>
        /// Returns the product of the device
        /// </summary>
        /// <param name="di">The object to act on</param>
        /// <returns>The product of the device</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern short  fido_dev_info_product(fido_dev_info_t *di);

        /// <summary>
        /// Returns the max message size of the device
        /// </summary>
        /// <param name="ci">The object to act on</param>
        /// <returns>The max message size of the device</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong fido_cbor_info_maxmsgsiz(fido_cbor_info_t *ci);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong fido_cbor_info_maxcredbloblen(fido_cbor_info_t* ci);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong fido_cbor_info_maxcredcntlist(fido_cbor_info_t* ci);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong fido_cbor_info_maxcredidlen(fido_cbor_info_t* ci);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong fido_cbor_info_fwversion(fido_cbor_info_t* ci);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool fido_dev_has_pin(fido_dev_t* dev);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool fido_dev_has_uv(fido_dev_t* device);

        /// <summary>
        /// Returns if device is capable of FIDO2
        /// </summary>
        /// <param name="dev">The object to act on</param>
        /// <returns><c>true</c> if capable, otherwise <c>false</c></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
		public static extern bool fido_dev_is_fido2(fido_dev_t *dev);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool fido_dev_is_winhello(fido_dev_t* dev);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool fido_dev_supports_pin(fido_dev_t* device);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool fido_dev_supports_cred_prot(fido_dev_t* device);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool fido_dev_supports_credman(fido_dev_t* device);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool fido_dev_supports_uv(fido_dev_t* device);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_dev_largeblob_get(fido_dev_t* device, byte* key_ptr,
            UIntPtr key_len, byte** blob_ptr, UIntPtr* blob_len);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_dev_largeblob_get_array(fido_dev_t* device, byte** cbor_ptr,
            UIntPtr* cbor_len);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_dev_largeblob_remove(fido_dev_t* device, byte* key_ptr,
            UIntPtr key_len, string pin);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_dev_largeblob_set(fido_dev_t* device, byte* key_ptr,
            UIntPtr key_len, byte* blob_ptr, UIntPtr blob_len);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fido_dev_largeblob_set_array(fido_dev_t* device, byte* cbor_ptr,
            UIntPtr cbor_len, string pin);
    }
}
