namespace Fido2Net
{
    /// <summary>
    /// CBOR Object Signing and Encryption algorithms (RFC 8152)
    /// </summary>
    public enum FidoCose
    {
        /// <summary>
        /// ECDSA w/ SHA-256
        /// </summary>
        ES256 = -7,

        /// <summary>
        /// RSA Signature with SHA-256
        /// </summary>
        RS256 = -257
    }
}
