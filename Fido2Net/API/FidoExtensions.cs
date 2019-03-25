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
        HmacSecret = 0x01
    }
}
