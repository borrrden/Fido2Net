using System;
using System.Runtime.InteropServices;

namespace Fido2Net.Interop
{
    /// <summary>
    /// A custom marshaller to retrieve char* and const char* properties from
    /// unmanaged code without freeing them (assumes the native library will
    /// free them later)
    /// </summary>
    public sealed class ConstStringMarshaler : ICustomMarshaler
    {
        #region Constants

        private static readonly ConstStringMarshaler Instance = new ConstStringMarshaler();

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the global instance of this class as required by <see cref="ICustomMarshaler"/>
        /// </summary>
        /// <param name="cookie">The cookie to use when getting the global instance (ignored)</param>
        /// <returns>The global instance</returns>
        public static ICustomMarshaler GetInstance(string cookie) => Instance;

        #endregion

        #region ICustomMarshaler

        /// <inheritdoc />
        public void CleanUpManagedData(object ManagedObj)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void CleanUpNativeData(IntPtr pNativeData)
        {
            // Do nothing
        }

        /// <inheritdoc />
        public int GetNativeDataSize()
        {
            return IntPtr.Size;
        }

        /// <inheritdoc />
        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            return Marshal.PtrToStringAnsi(pNativeData);
        }

        #endregion
    }
}