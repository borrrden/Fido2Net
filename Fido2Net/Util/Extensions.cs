namespace Fido2Net.Util
{
    internal static class Extensions
    {
        public static void Check(this int err)
        {
            if (err > (int)CtapStatus.Ok) {
                throw new CtapException((CtapStatus)err);
            }

            if(err < (int)FidoStatus.Ok) {
                throw new FidoException((FidoStatus)err);
            }
        }
    }
}
