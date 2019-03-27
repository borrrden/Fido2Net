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
        public CtapException(CtapStatus err) : base($"CTAP response indicated non-success status ({err})")
        {
            Code = err;
        }
    }

    /// <summary>
    /// An exception indicating that there was some problem with the FIDO2 device
    /// </summary>
    public sealed class FidoException : Exception
    {
        /// <summary>
        /// The code returned from the device
        /// </summary>
        public FidoStatus Code { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="code">The code to use</param>
        public FidoException(FidoStatus code) : base($"FIDO2 operation failed ({code})")
        {
            Code = code;
        }
    }
}
