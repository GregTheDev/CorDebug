using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interop.CorDebug
{
    public class corerror
    {
        public const uint CORDBG_E_FIELD_NOT_AVAILABLE = 0x80131306; // todo xxx
        public const uint CORDBG_S_AT_END_OF_STACK = 0x00131324; // 0x80131324;
        public const uint CORDBG_E_CODE_NOT_AVAILABLE = 0x80131309; //todo xxx
        public const uint CORDBG_E_BAD_REFERENCE_VALUE = 0x80131305;
        public const uint CORDBG_E_FIELD_NOT_INSTANCE = 0x8013133c;
        public const uint CORDBG_E_NOT_CLR = 0x80131c44; // returned by OpenVirtualProcess
        public const uint CORDBG_E_UNSUPPORTED_DEBUGGING_MODEL = 0x80131c46; // returned by OpenVirtualProcess
        public const uint CORDBG_E_UNSUPPORTED_FORWARD_COMPAT = 0x80131c47; // returned by OpenVirtualProcess
        public const int CORDBG_E_CORRUPT_OBJECT = -2146231221; // 0x80131C4B, return by ICorDebugProcess5.GetObject
    }
}
