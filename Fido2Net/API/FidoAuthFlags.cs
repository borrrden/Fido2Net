using System;

namespace Fido2Net
{
    /// <summary>
    /// Flags set on either a <see cref="FidoCredential"/> or a <see cref="FidoAssertionStatement"/>
    /// </summary>
    [Flags]
    public enum FidoAuthFlags : byte
    {
        /// <summary>
        /// The user was present for the credential/assertion generation process
        /// </summary>
        UserPresent = 0x01,

        /// <summary>
        /// The user has been verified via verification process
        /// </summary>
        UserVerified = 0x04,

        /// <summary>
        /// An attested credential object is included with this object
        /// </summary>
        AttestedCredential = 0x40,

        /// <summary>
        /// Extension data is included on this object
        /// </summary>
        ExtensionData = 0x80
    }
}
