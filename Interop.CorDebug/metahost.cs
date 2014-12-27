using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Interop.CorDebug
{
    /// <summary>
    /// Includes the ICLRDebuggingLibraryProvider::ProvideLibrary Method method, which gets a library provider callback interface that allows common language runtime version-specific debugging libraries to be located and loaded on demand.
    /// </summary>
    /// <see cref="http://msdn.microsoft.com/en-us/library/dd537635%28v=vs.110%29.aspx"/>
    [ComImport, Guid("3151C08D-4D09-4f9b-8838-2880BF18FE51"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICLRDebuggingLibraryProvider
    {
        /// <summary>
        /// Gets a library provider callback interface that allows common language runtime (CLR) version-specific debugging libraries to be located and loaded on demand.
        /// </summary>
        /// <see cref="http://msdn.microsoft.com/en-us/library/dd537638%28v=vs.110%29.aspx"/>
        [PreserveSig]
        uint ProvideLibrary([MarshalAs(UnmanagedType.LPWStr)] string fileName, uint timestamp, uint sizeOfImage, out IntPtr hModule);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct CLR_DEBUGGING_VERSION
    {
        public ushort wStructVersion;
        public ushort wMajor;
        public ushort wMinor;
        public ushort wBuild;
        public ushort wRevision;
    }

    public enum CLR_DEBUGGING_PROCESS_FLAGS
    {
        CLR_DEBUGGING_MANAGED_EVENT_PENDING = 1
    }

    [ComImport, Guid("D28F3C5A-9634-4206-A509-477552EEFB10"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICLRDebugging
    {
        [PreserveSig]
        uint OpenVirtualProcess(UInt64 moduleBaseAddress, [MarshalAs(UnmanagedType.IUnknown)] object dataTarget, [MarshalAs(UnmanagedType.Interface)] ICLRDebuggingLibraryProvider libraryProvider, ref CLR_DEBUGGING_VERSION maxDebuggerSupportedVersion, ref Guid riidProcess, [MarshalAs(UnmanagedType.IUnknown)] out object process, ref CLR_DEBUGGING_VERSION version, out CLR_DEBUGGING_PROCESS_FLAGS flags);
        [PreserveSig]
        uint CanUnloadNow(IntPtr moduleHandle);
    }

    public class MetaHost
    {
        public static Guid CLSID_CLRDebugging = new Guid(0xbacc578d, 0xfbdd, 0x48a4, 0x96, 0x9f, 0x2, 0xd9, 0x32, 0xb7, 0x46, 0x34);
        public static Guid IID_ICLRDebugging = new Guid(0xd28f3c5a, 0x9634, 0x4206, 0xa5, 0x9, 0x47, 0x75, 0x52, 0xee, 0xfb, 0x10);

        public static int CLRCreateInstance(ref Guid clsid, ref Guid riid, ref IntPtr result)
        {
            return NativeMethods.CLRCreateInstance(ref clsid, ref riid, ref result);
        }
    }

    internal class NativeMethods
    {
        [DllImport("mscoree.dll", SetLastError = true)]
        public static extern int CLRCreateInstance(ref Guid clsid, ref Guid riid, ref IntPtr result);
    }
}
