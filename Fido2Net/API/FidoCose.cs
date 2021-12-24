namespace Fido2Net
{
    /// <summary>
    /// CBOR Object Signing and Encryption algorithms (RFC 8152)
    /// </summary>
    public enum FidoCose
    {
        Unspecified = 0,

        /// <summary>
        /// ECDSA w/ SHA-256
        /// </summary>
        ES256 = -7,

        EDDSA = -8,

        ECDH_ES256 = -25,

        /// <summary>
        /// RSA Signature with SHA-256
        /// </summary>
        RS256 = -257,

        RS1 = -65536
    }
}
