using System;
using System.Collections.Generic;
using System.Text;

namespace Fido2Net
{
    public enum FidoStatus
    {
        Ok = 0,
        TxErr = -1,
        RxErr = -2,
        RxNotCbor = -3,
        RxInvalidCbor = -4,
        InvalidParam = -5,
        InvalidSignature = -6,
        InvalidArgument = -7,
        UserPresenceRequired = -8,
        InternalError = -9
    }
}
