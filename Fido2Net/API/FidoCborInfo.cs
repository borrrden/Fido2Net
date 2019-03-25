using Fido2Net.Interop;
using Fido2Net.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fido2Net
{
    /// <summary>
    /// A class that represents the extended details of a FIDO2 authenticator
    /// </summary>
    public sealed unsafe class FidoCborInfo : IDisposable
    {
        #region Variables

        private fido_cbor_info_t* _native;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Authenticator attestation GUID of this device
        /// </summary>
        public Guid AaGuid
        {
            get
            {
                var len = (int) Native.fido_cbor_info_aaguid_len(_native);
                var ptr = Native.fido_cbor_info_aaguid_ptr(_native);
                return new Guid(new ReadOnlySpan<byte>(ptr, len).ToArray());
            }
        }

        /// <summary>
        /// Gets the extensions that this device supports
        /// </summary>
        public string[] Extensions
        {
            get
            {
                var len = (int) Native.fido_cbor_info_extensions_len(_native);
                var ptr = Native.fido_cbor_info_extensions_ptr(_native);
                var retVal = new string[len];
                for (int i = 0; i < len; i++) {
                    retVal[i] = new string((sbyte*) ptr[i]);
                }

                return retVal;
            }
        }

        /// <summary>
        /// Gets the maximum message size for this device
        /// </summary>
        public ulong MaxMessageSize => Native.fido_cbor_info_maxmsgsiz(_native);

        /// <summary>
        /// Gets the options that this device supports
        /// </summary>
        public IReadOnlyDictionary<string, bool> Options
        {
            get
            {
                var len = (int) Native.fido_cbor_info_options_len(_native);
                var namePtr = Native.fido_cbor_info_options_name_ptr(_native);
                var valuePtr = Native.fido_cbor_info_options_value_ptr(_native);
                var retVal = new Dictionary<string, bool>();
                for (int i = 0; i < len; i++) {
                    var k = new string((sbyte*) namePtr[i]);
                    var v = valuePtr[i] != 0;
                    retVal.Add(k, v);
                }

                return retVal;
            }
        }

        /// <summary>
        /// Gets the pin protocols that this device supports
        /// </summary>
        public ReadOnlySpan<byte> PinProtocols
        {
            get
            {
                var len = (int) Native.fido_cbor_info_protocols_len(_native);
                var ptr = Native.fido_cbor_info_protocols_ptr(_native);
                return new ReadOnlySpan<byte>(ptr, len);
            }
        }

        /// <summary>
        /// Gets the versions that this device supports
        /// </summary>
        public string[] Versions
        {
            get
            {
                var len = (int) Native.fido_cbor_info_versions_len(_native);
                var ptr = Native.fido_cbor_info_versions_ptr(_native);
                var retVal = new string[len];
                for (int i = 0; i < len; i++) {
                    retVal[i] = new string((sbyte*) ptr[i]);
                }

                return retVal;
            }
        }

        #endregion

        #region Constructors

        internal FidoCborInfo()
        {
            _native = Native.fido_cbor_info_new();
            if (_native == null) {
                throw new OutOfMemoryException();
            }
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~FidoCborInfo()
        {
            ReleaseUnmanagedResources();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// A cast operator for using a CborInfo object as a native handle
        /// </summary>
        /// <param name="info">The object to use</param>
        public static explicit operator fido_cbor_info_t*(FidoCborInfo info)
        {
            return info._native;
        }

        #endregion

        #region Internal Methods

        internal void ReadCborInfo(FidoDevice device) =>
            Native.fido_dev_get_cbor_info((fido_dev_t*) device, _native).Check();

        #endregion

        #region Private Methods

        private void ReleaseUnmanagedResources()
        {
            var native = _native;
            Native.fido_cbor_info_free(&native);
            _native = null;
        }

        #endregion

        #region IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Overrides

        /// <inheritdoc />
        public override string ToString()
        {
            var opts = Options.Where(x => x.Value).Select(x => x.Key);
            var pinProtocols = new StringBuilder();
            if (PinProtocols.Length > 0) {
                pinProtocols.Append(PinProtocols[0].ToString());
                for (int i = 1; i < PinProtocols.Length; i++) {
                    pinProtocols.AppendFormat(", {0}", PinProtocols[i]);
                }
            }

            return
                $"version strings: {String.Join(", ", Versions)}, extension strings: {String.Join(", ", Extensions)}, aaguid: {AaGuid}, options: {String.Join(", ", opts)}, maxmsgsize: {MaxMessageSize}, pin protocols: {pinProtocols.ToString()}";
        }

        #endregion
    }
}