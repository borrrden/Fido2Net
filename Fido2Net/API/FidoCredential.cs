using Fido2Net.Interop;
using Fido2Net.Util;
using System;


namespace Fido2Net
{
    /// <summary>
    /// A PODO representing a relying party for a <see cref="FidoCredential"/>
    /// </summary>
    public sealed class FidoCredentialRp
    {
        /// <summary>
        /// Gets or sets the name of the relying party
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the ID of the relying party
        /// </summary>
        public string Id { get; set; }
    }

    /// <summary>
    /// A PODO representing a user attached to a <see cref="FidoCredential"/>
    /// </summary>
    public sealed class FidoCredentialUser
    {
        /// <summary>
        /// Gets or sets the ID of the user
        /// </summary>
        public byte[] Id { get; set; }

        /// <summary>
        /// Gets or set the name of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the display name for the user
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets an identifier for the icon for the user
        /// </summary>
        public string Icon { get; set; }
    }

    /// <summary>
    /// A class that holds a FIDO credential object which can be used 
    /// to generate a <see cref="FidoAssertion"/>
    /// </summary>
    public sealed unsafe class FidoCredential : IDisposable
    {
        #region Variables

        private fido_cred_t* _native;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the authenticator data for this credential
        /// </summary>
        /// <exception cref="CtapException">Thrown if an error occurs while setting the auth data</exception>
        public ReadOnlySpan<byte> AuthData
        {
            get
            {
                var len = (int) Native.fido_cred_authdata_len(_native);
                var ptr = Native.fido_cred_authdata_ptr(_native);
                return new ReadOnlySpan<byte>(ptr, len);
            }
            set
            {
                fixed (byte* value_ = value) {
                    Native.fido_cred_set_authdata(_native, value_, (IntPtr) value.Length).Check();
                }
            }
        }

        /// <summary>
        /// <para>WebAuthn §5.1 https://www.w3.org/TR/webauthn-1/#sec-client-data</para>
        /// Gets or sets the hash of the client data object that the assertion is based on.
        /// </summary>
        /// <exception cref="CtapException">Thrown if an error occurs while setting the hash</exception>
        public ReadOnlySpan<byte> ClientDataHash
        {
            get
            {
                var len = (int) Native.fido_cred_clientdata_hash_len(_native);
                var ptr = Native.fido_cred_clientdata_hash_ptr(_native);
                return new ReadOnlySpan<byte>(ptr, len);
            }
            set
            {
                fixed (byte* value_ = value) {
                    Native.fido_cred_set_clientdata_hash(_native, value_, (IntPtr) value.Length).Check();
                }
            }
        }

        /// <summary>
        /// Gets the flags that are set on this credential
        /// </summary>
        public FidoAuthFlags Flags => Native.fido_cred_flags(_native);

        /// <summary>
        /// <para>WebAuthn §6.4.2 https://www.w3.org/TR/webauthn-1/#attestation-formats </para>
        /// Gets or sets the format of the attestation object that was generated for this credential
        /// </summary>
        /// <exception cref="CtapException">Thrown if an error occurs while setting the format</exception>
        public string Format
        {
            get => Native.fido_cred_fmt(_native);
            set => Native.fido_cred_set_fmt(_native, value).Check();
        }

        /// <summary>
        /// Gets the ID that was generated for this credential
        /// </summary>
        public ReadOnlySpan<byte> Id
        {
            get
            {
                var len = (int) Native.fido_cred_id_len(_native);
                var ptr = Native.fido_cred_id_ptr(_native);
                return new ReadOnlySpan<byte>(ptr, len);
            }
        }

        /// <summary>
        /// Gets the public key that was generated for this credential
        /// </summary>
        public ReadOnlySpan<byte> PublicKey
        {
            get
            {
                var len = (int) Native.fido_cred_pubkey_len(_native);
                var ptr = Native.fido_cred_pubkey_ptr(_native);
                return new ReadOnlySpan<byte>(ptr, len);
            }
        }

        /// <summary>
        /// Gets or sets the relying party which requested the creation of this credentials
        /// </summary>
        /// <exception cref="CtapException">Thrown if an error occurs while setting the rp</exception>
        public FidoCredentialRp Rp
        {
            get => new FidoCredentialRp
            {
                Id = Native.fido_cred_rp_id(_native),
                Name = Native.fido_cred_rp_name(_native)
            };
            set => Native.fido_cred_set_rp(_native, value.Id, value.Name).Check();
        }

        /// <summary>
        /// Gets or sets the signature for this credential
        /// </summary>
        /// <exception cref="CtapException">Thrown if an error occurs while setting the signature</exception>
        public ReadOnlySpan<byte> Signature
        {
            get
            {
                var len = (int) Native.fido_cred_sig_len(_native);
                var ptr = Native.fido_cred_sig_ptr(_native);
                return new ReadOnlySpan<byte>(ptr, len);
            }
            set
            {
                fixed (byte* value_ = value) {
                    Native.fido_cred_set_sig(_native, value_, (IntPtr) value.Length).Check();
                }
            }
        }

        /// <summary>
        /// Gets the attestation certificate for this credential
        /// </summary>
        public ReadOnlySpan<byte> X5C
        {
            get
            {
                var len = (int) Native.fido_cred_x5c_len(_native);
                var ptr = Native.fido_cred_x5c_ptr(_native);
                return new ReadOnlySpan<byte>(ptr, len);
            }
        }

        #endregion

        #region Constructors

        static FidoCredential()
        {
            Init.Call();
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <exception cref="OutOfMemoryException" />
        public FidoCredential()
        {
            _native = Native.fido_cred_new();
            if (_native == null) {
                throw new OutOfMemoryException();
            }
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~FidoCredential()
        {
            ReleaseUnmanagedResources();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// A cast operator for using this object as a native handle
        /// </summary>
        /// <param name="cred">The object to use as a native handle</param>
        public static explicit operator fido_cred_t* (FidoCredential cred)
        {
            return cred._native;
        }

        /// <summary>
        /// Excludes a given credential from being created (useful for finding out if
        /// a credential already exists on the device)
        /// </summary>
        /// <param name="id">The ID of the credential to exclude</param>
        /// <exception cref="CtapException">Thrown if an error occurs while excluding the credential</exception>
        public void Exclude(ReadOnlySpan<byte> id)
        {
            fixed (byte* body_ = id) {
                Native.fido_cred_exclude(_native, body_, (IntPtr) id.Length).Check();
            }
        }

        /// <summary>
        /// Sets the extensions to use when generating the credential
        /// </summary>
        /// <param name="extensions">The extensions to use</param>
        /// <exception cref="CtapException">Thrown if an error occurs while setting the extension</exception>
        public void SetExtensions(FidoExtensions extensions) => Native.fido_cred_set_extensions(_native, extensions).Check();

        /// <summary>
        /// Sets the options to use when generating the credential
        /// </summary>
        /// <param name="residentKey">Requests that key material be stored on the device</param>
        /// <param name="verifyUser">Requests that the credential go through the user verification process after being generated</param>
        /// <exception cref="CtapException">Thrown if an error occurs while setting the options</exception>
        public void SetOptions(bool residentKey, bool verifyUser) => Native.fido_cred_set_options(_native, residentKey, verifyUser).Check();

        /// <summary>
        /// Sets the algorithm to use when signing using this credential
        /// </summary>
        /// <param name="type">The signing / encryption algorithm to use</param>
        /// <exception cref="CtapException">Thrown if an error occurs while setting the type</exception>
        public void SetType(FidoCose type) => Native.fido_cred_set_type(_native, type).Check();

        /// <summary>
        /// Sets the user that is associated with this credential
        /// </summary>
        /// <param name="user">The user to associate with this credential</param>
        /// <exception cref="CtapException">Thrown if an error occurs while setting the user</exception>
        public void SetUser(FidoCredentialUser user)
        {
            fixed (byte* id_ = user.Id) {
                Native.fido_cred_set_user(_native, id_, (IntPtr) user.Id.Length, user.Name,
                    user.DisplayName, user.Icon).Check();
            }
        }

        /// <summary>
        /// Sets the X509 certificate to use for attestation purposes
        /// </summary>
        /// <param name="x509">The X509 certificate</param>
        /// <exception cref="CtapException">Thrown if an error occurs while setting the certificate</exception>
        public void SetX509(ReadOnlySpan<byte> x509)
        {
            fixed (byte* x509_ = x509) {
                Native.fido_cred_set_x509(_native, x509_, (IntPtr) x509.Length).Check();
            }
        }

        /// <summary>
        /// Verifies that the signature of this credential matches its attributes
        /// </summary>
        /// <exception cref="CtapException">Thrown if an error occurs or verification fails</exception>
        public void Verify() => Native.fido_cred_verify(_native).Check();

        #endregion

        #region Private Methods

        private void ReleaseUnmanagedResources()
        {
            var native = _native;
            Native.fido_cred_free(&native);
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
    }
}
