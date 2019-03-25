using Fido2Net.Interop;
using Fido2Net.Util;

using System;

namespace Fido2Net
{
    /// <summary>
    /// A reference struct representing a statement contained within a <see cref="FidoAssertion"/> object
    /// </summary>
    public unsafe ref struct FidoAssertionStatement
    {
        #region Properties

        /// <summary>
        /// <para>WebAuthn §6.1 https://www.w3.org/TR/webauthn-1/#sec-authenticator-data</para>
        /// Gets the authenticator data for the assertion statement.
        /// </summary>
        public ReadOnlySpan<byte> AuthData { get; }

        /// <summary>
        /// Gets the flags that are set on this assertion statement
        /// </summary>
        public FidoAuthFlags Flags { get; }

        /// <summary>
        /// <para>CTAP §9.1 (link TBD)</para>
        /// Gets the HMAC secret for this assertion statement.
        /// </summary>
        public ReadOnlySpan<byte> HmacSecret { get; }

        /// <summary>
        /// Gets the ID for this assertion statement
        /// </summary>
        public ReadOnlySpan<byte> Id { get; }

        /// <summary>
        /// Gets the signature for this assertion statement
        /// </summary>
        public ReadOnlySpan<byte> Signature { get; }

        /// <summary>
        /// Gets the user display name that is assigned to the credential this
        /// assertion statement is based on
        /// </summary>
        public string UserDisplayName { get; }

        /// <summary>
        /// Gets the user icon that is assigned to the credential this assertion
        /// statement is based on (probably a URL)
        /// </summary>
        public string UserIcon { get; }

        /// <summary>
        /// Gets the binary ID of the user that is assigned to the credential
        /// this assertion statement is based on.
        /// </summary>
        public ReadOnlySpan<byte> UserId { get; }

        /// <summary>
        /// Gets the name of the user that is assigned to the credential this
        /// assertion statement is based on
        /// </summary>
        public string UserName { get; }

        #endregion

        #region Constructors

        internal FidoAssertionStatement(fido_assert_t* native, int index)
        {
            var idx = (IntPtr) index;
            AuthData = new ReadOnlySpan<byte>(
                Native.fido_assert_authdata_ptr(native, idx),
                (int) Native.fido_assert_authdata_len(native, idx));
            Flags = Native.fido_assert_flags(native, idx);
            HmacSecret = new ReadOnlySpan<byte>(
                Native.fido_assert_hmac_secret_ptr(native, idx),
                (int) Native.fido_assert_hmac_secret_len(native, idx));
            Id = new ReadOnlySpan<byte>(
                Native.fido_assert_id_ptr(native, idx),
                (int) Native.fido_assert_id_len(native, idx));
            Signature = new ReadOnlySpan<byte>(
                Native.fido_assert_sig_ptr(native, idx),
                (int) Native.fido_assert_sig_len(native, idx));
            UserDisplayName = Native.fido_assert_user_display_name(native, idx);
            UserIcon = Native.fido_assert_user_icon(native, idx);
            UserName = Native.fido_assert_user_name(native, idx);
            UserId = new ReadOnlySpan<byte>(
                Native.fido_assert_user_id_ptr(native, idx),
                (int) Native.fido_assert_user_id_len(native, idx));
        }

        #endregion
    }

    /// <summary>
    /// Creates an object for holding data about a given assertion.  In FIDO2, an assertion
    /// is proof that the authenticator being used has knowledge of the private key associated
    /// with the public key that the other party is in posession of.  
    /// </summary>
    public sealed unsafe class FidoAssertion : IDisposable
    {
        #region Variables

        private fido_assert_t* _native;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the assertion statement at the given index
        /// </summary>
        /// <param name="index">The index of the assertion statement to retrieve</param>
        /// <returns>The assertion statement object</returns>
        /// <exception cref="IndexOutOfRangeException">The index is not in the range [0, count)</exception>
        public FidoAssertionStatement this[int index]
        {
            get {
                if (index < 0 || index >= Count) {
                    throw new IndexOutOfRangeException();
                }

                return new FidoAssertionStatement(_native, index);
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
                var len = (int) Native.fido_assert_clientdata_hash_len(_native);
                var ptr = Native.fido_assert_clientdata_hash_ptr(_native);
                return new ReadOnlySpan<byte>(ptr, len);
            }
            set
            {
                fixed (byte* value_ = value) {
                    Native.fido_assert_set_clientdata_hash(_native, value_, (IntPtr) value.Length).Check();
                }
            }
        }

        /// <summary>
        /// Gets or sets the number of <see cref="FidoAssertionStatement"/> entries contained
        /// within this assertion.
        /// </summary>
        /// <exception cref="CtapException">Thrown if an error occurs while setting the count</exception>
        public int Count
        {
            get => (int) Native.fido_assert_count(_native);
            set => Native.fido_assert_set_count(_native, (IntPtr) value).Check();
        }

        /// <summary>
        /// Gets or sets the relying party that requested this assertion
        /// </summary>
        /// <exception cref="CtapException">Thrown if an error occurs while setting the relying party</exception>
        public string Rp
        {
            get => Native.fido_assert_rp_id(_native);
            set => Native.fido_assert_set_rp(_native, value).Check();
        }

        #endregion

        #region Constructors

        static FidoAssertion()
        {
            Init.Call();
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <exception cref="OutOfMemoryException" />
        public FidoAssertion()
        {
            _native = Native.fido_assert_new();
            if (_native == null) {
                throw new OutOfMemoryException();
            }
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~FidoAssertion()
        {
            ReleaseUnmanagedResources();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Cast operator for using this object as a native handle
        /// </summary>
        /// <param name="assert">The object to use</param>
        public static explicit operator fido_assert_t*(FidoAssertion assert)
        {
            return assert._native;
        }

        /// <summary>
        /// Adds an allowed credential to this assertion.  If used, only credential objects
        /// with the IDs added via this method will be considered when making an assertion.
        /// </summary>
        /// <param name="credentialId">The ID of the credential to add to the whitelist</param>
        /// <exception cref="CtapException">Thrown if an error occurs while adding the credential</exception>
        public void AllowCredential(ReadOnlySpan<byte> credentialId)
        {
            fixed (byte* cred = credentialId) {
                Native.fido_assert_allow_cred(_native, cred, (IntPtr) credentialId.Length).Check();
            }
        }

        /// <summary>
        /// Sets the authenticator data for a given statement in this assertion
        /// </summary>
        /// <param name="authData">The data to set</param>
        /// <param name="index">The index of the assertion statement to set the data for</param>
        /// <exception cref="CtapException">Thrown if an error occurs while setting the auth data</exception>
        public void SetAuthData(ReadOnlySpan<byte> authData, int index)
        {
            fixed (byte* authData_ = authData) {
                Native.fido_assert_set_authdata(_native, (IntPtr) index, authData_, (IntPtr) authData.Length).Check();
            }
        }

        /// <summary>
        /// Sets the extensions to use while creating this assertion
        /// </summary>
        /// <param name="extensions">The extension to use</param>
        /// <exception cref="CtapException">Thrown if an error occurs while setting the extensions</exception>
        public void SetExtensions(FidoExtensions extensions) =>
            Native.fido_assert_set_extensions(_native, extensions).Check();

        /// <summary>
        /// Sets the HMAC salt on a given assertion statement within this assertion
        /// </summary>
        /// <param name="salt">The salt to set</param>
        /// <param name="index">The index of the assertion statement that will receive the salt</param>
        /// <exception cref="CtapException">Thrown if an error occurs while setting the salt</exception>
        public void SetHmacSalt(ReadOnlySpan<byte> salt, int index)
        {
            fixed (byte* salt_ = salt) {
                Native.fido_assert_set_hmac_salt(_native, salt_, (IntPtr) salt.Length).Check();
            }
        }

        /// <summary>
        /// Sets the options that will be used when the assertion is generated
        /// </summary>
        /// <param name="userPresent">Whether or not the user is required to be present (i.e. interact with the authenticator)</param>
        /// <param name="userVerification">Whether or not a user verification process must be used in addition to the assertion generation</param>
        /// <exception cref="CtapException">Thrown if an error occurs while setting the options</exception>
        public void SetOptions(bool userPresent, bool userVerification) => Native.fido_assert_set_options(_native, userPresent, userVerification).Check();

        /// <summary>
        /// Sets the signature for the assertion statement at the given index
        /// </summary>
        /// <param name="signature">The signature bytes to set</param>
        /// <param name="index">The index of the assertion statement to set the signature on</param>
        /// <exception cref="CtapException">Thrown if an error occurs while setting the signature</exception>
        public void SetSignature(ReadOnlySpan<byte> signature, int index)
        {
            fixed (byte* signature_ = signature) {
                Native.fido_assert_set_sig(_native, (IntPtr) index, signature_, (IntPtr) signature.Length).Check();
            }
        }

        /// <summary>
        /// Verifies that the signature of the assertion statement at the given index matches
        /// the parameters of the assertion
        /// </summary>
        /// <param name="index">The index of the assertion statement to check</param>
        /// <param name="algorithm">The algorithm used in the public key provided</param>
        /// <param name="pk">The public key to use for verification</param>
        /// <exception cref="CtapException">Thrown if an error occurs or verifications fails</exception>
        public void Verify(int index, FidoCose algorithm, ReadOnlySpan<byte> pk)
        {
            fixed(void* pk_ = pk) { 
                Native.fido_assert_verify(_native, (IntPtr) index, algorithm, pk_).Check();
            }
        }

        #endregion

        #region Private Methods

        private void ReleaseUnmanagedResources()
        {
            var native = _native;
            Native.fido_assert_free(&native);
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