namespace Fido2Net
{
    /// <summary>
    /// Extensions for a FIDO2 device
    /// </summary>
    public enum FidoExtensions
    {
        /// <summary>
        /// No extensions
        /// </summary>
        None = 0x00,

        /// <summary>
        /// HMAC Secret extension (CTAP §9.1)  
        /// </summary>
        HmacSecret = 0x01,

        CredentialProtection = 0x02,
        LargeBlobKey = 0x04,
        CredentialBlob = 0x08,
        MinimumPinLength = 0x10
    }
}
