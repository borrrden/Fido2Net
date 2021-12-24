using Fido2Net.Interop;

namespace Fido2Net.API
{
    internal static class ClassExtensions
    {
        public static fido_opt_t ToFidoOptional(this bool? val)
        {
            if(val == null) {
                return fido_opt_t.FIDO_OPT_OMIT;
            }

            return val == true ? fido_opt_t.FIDO_OPT_TRUE : fido_opt_t.FIDO_OPT_FALSE;
        }
    }
}
