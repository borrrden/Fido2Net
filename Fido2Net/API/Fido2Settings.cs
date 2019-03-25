using System;

namespace Fido2Net
{

    /// <summary>
    /// Flags representing global FIDO library behavior
    /// </summary>
    [Flags]
    public enum FidoFlags
    {
        /// <summary>
        /// No special behavior
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Enable additional debug output from libfido2
        /// </summary>
        Debug = 0x01
    }

    /// <summary>
    /// The global settings for Fido2Net
    /// </summary>
    public static class Fido2Settings
    {
        /// <summary>
        /// Gets or sets the flags to use when initializing libfido2
        /// behavior.  Must be set before use of any objects in the
        /// Fido2Net namespace.
        /// </summary>
        public static FidoFlags Flags;
    }
}
