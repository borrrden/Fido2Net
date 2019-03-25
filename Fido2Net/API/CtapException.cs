using System;

namespace Fido2Net
{
    /// <summary>
    /// An exception representing a return status that is non-successful according to the CTAP specification
    /// </summary>
    public sealed class CtapException : Exception
    {
        /// <summary>
        /// The status code that was returned
        /// </summary>
        public CtapStatus Code { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="err">The status code to use</param>
        public CtapException(CtapStatus err) : base($"FIDO2 device returned non-success status ({err})")
        {
            Code = err;
        }
    }

    /// <summary>
    /// An exception indicating that there was some problem with the FIDO2 device
    /// </summary>
    public sealed class AuthenticatorException : Exception
    {
        /// <summary>
        /// The code returned from the device
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="code">The code to use</param>
        public AuthenticatorException(int code) : base($"FIDO2 device failed to perform operation ({code})")
        {
            Code = code;
        }
    }
}
