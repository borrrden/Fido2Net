using Fido2Net.Interop;
using Fido2Net.Util;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fido2Net
{
    /// <summary>
    /// A class representing external info about a particular FIDO capable
    /// device
    /// </summary>
    public sealed class FidoDeviceInfo
    {
        #region Properties

        /// <summary>
        /// Gets the manufacturer of the device
        /// </summary>
        public string Manufacturer { get; }

        /// <summary>
        /// Gets the path of the device (for use in <see cref="FidoDevice.Open(string)"/>)
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Gets the product ID of the device
        /// </summary>
        public short Product { get; }

        /// <summary>
        /// Gets a string representation of the product ID
        /// </summary>
        public string ProductString { get; }

        /// <summary>
        /// Gets the vendor ID of the device
        /// </summary>
        public short Vendor { get; }

        #endregion

        #region Constructors

        internal FidoDeviceInfo(string path, short vendor, short product, string manufacturer, string productString)
        {
            Path = path;
            Vendor = vendor;
            Product = product;
            Manufacturer = manufacturer;
            ProductString = productString;
        }

        #endregion

        #region Overrides

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Path}: vendor={Vendor:X4}, product={Product:X4} ({Manufacturer} {ProductString})";
        }

        #endregion
    }

    internal sealed unsafe class FidoDeviceInfoEnumerator : IEnumerator<FidoDeviceInfo>
    {
        #region Variables

        private readonly int _count;
        private readonly fido_dev_info_t* _native;
        private int _index = -1;

        #endregion

        #region Properties

        public FidoDeviceInfo Current
        {
            get
            {
                var di = Native.fido_dev_info_ptr(_native, (UIntPtr) _index);
                return new FidoDeviceInfo(
                    Native.fido_dev_info_path(di),
                    Native.fido_dev_info_vendor(di),
                    Native.fido_dev_info_product(di),
                    Native.fido_dev_info_manufacturer_string(di),
                    Native.fido_dev_info_product_string(di)
                );
            }
        }
            

        object IEnumerator.Current => Current;

        #endregion

        #region Constructors

        public FidoDeviceInfoEnumerator(fido_dev_info_t* native, int count)
        {
            _native = native;
            _count = count;
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            
        }

        #endregion

        #region IEnumerator

        public bool MoveNext()
        {
            return ++_index < _count;
        }

        public void Reset()
        {
            _index = -1;
        }

        #endregion
    }

    /// <summary>
    /// A class that can enumerate FIDO capable devices that are attached to and/or paired
    /// with the local machine
    /// </summary>
    public sealed unsafe class FidoDeviceInfoList : IEnumerable<FidoDeviceInfo>, IDisposable
    {
        #region Variables

        private UIntPtr _capacity;
        private int _count;
        private fido_dev_info_t* _native;

        #endregion

        #region Constructors

        static FidoDeviceInfoList()
        {
            Init.Call();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="capacity">The maximum number of devices to search for</param>
        /// <exception cref="OutOfMemoryException" />
        /// <exception cref="CtapException">Thrown if an error occurs while enumerating the devices</exception>
        public FidoDeviceInfoList(int capacity)
        {
            _capacity = (UIntPtr) capacity;
            _native = Native.fido_dev_info_new(_capacity);
            if (_native == null) {
                throw new OutOfMemoryException();
            }

            var olen = UIntPtr.Zero;
            Native.fido_dev_info_manifest(_native, _capacity, &olen).Check();
            _count = (int) olen;
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~FidoDeviceInfoList()
        {
            ReleaseUnmanagedResources();
        }

        #endregion

        #region Private Methods

        private void ReleaseUnmanagedResources()
        {
            if (_native == null) {
                return;
            }

            var native = _native;
            Native.fido_dev_info_free(&native, _capacity);
            _native = null;
        }

        #endregion

        #region IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IEnumerable

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IEnumerable<FidoDeviceInfo>

        /// <inheritdoc />
        public IEnumerator<FidoDeviceInfo> GetEnumerator()
        {
            return new FidoDeviceInfoEnumerator(_native, _count);
        }

        #endregion
    }
}