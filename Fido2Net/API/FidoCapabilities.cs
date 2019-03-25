using System;

namespace Fido2Net
{
    /// <summary>
    /// Flags representing the capabilities of a FIDO2 authenticator
    /// </summary>
    [Flags]
    public enum FidoCapabilities : byte
    {
        /// <summary>
        /// The authenticator has some sort of action that indicates usage 
        /// (e.g. blinking LED, audio notification, etc)
        /// </summary>
        Wink = 0x01,

        /// <summary>
        /// The device has the ability to communicate via the CBOR protocol (RFC 7049)
        /// </summary>
        Cbor = 0x04,

        /// <summary>
        /// The device does NOT have the ability to communicate via the CTAP1 / U2F
        /// protocol 
        /// </summary>
        Nmsg = 0x08
    }
}
