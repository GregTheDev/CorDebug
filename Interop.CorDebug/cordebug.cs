using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Interop.CorDebug
{
    public enum CorDebugPlatform
    {
        CORDB_PLATFORM_WINDOWS_X86,       // Windows on Intel x86
        CORDB_PLATFORM_WINDOWS_AMD64,     // Windows x64 (Amd64, Intel EM64T)
        CORDB_PLATFORM_WINDOWS_IA64,      // Windows on Intel IA-64
        CORDB_PLATFORM_MAC_PPC,           // MacOS on PowerPC
        CORDB_PLATFORM_MAC_X86,           // MacOS on Intel x86
        CORDB_PLATFORM_WINDOWS_ARM,       // Windows on ARM
        CORDB_PLATFORM_MAC_AMD64          // MacOS on Intel x64 
    }

    public enum CorDebugThreadState
    {
        THREAD_RUN,
        THREAD_SUSPEND
    }

    public enum CorDebugUserState
    {
        USER_BACKGROUND = 4,
        USER_STOP_REQUESTED = 1,
        USER_STOPPED = 0x10,
        USER_SUSPEND_REQUESTED = 2,
        USER_SUSPENDED = 0x40,
        USER_UNSAFE_POINT = 0x80,
        USER_UNSTARTED = 8,
        USER_WAIT_SLEEP_JOIN = 0x20
    }

    public enum CorDebugSetContextFlag
    {
        SET_CONTEXT_FLAG_ACTIVE_FRAME = 1,
        SET_CONTEXT_FLAG_UNWIND_FRAME
    }

    public enum CorDebugGCType
    {
        CorDebugWorkstationGC,
        CorDebugServerGC
    }

    public enum CorDebugGenerationTypes
    {
        CorDebug_Gen0 = 0,
        CorDebug_Gen1 = 1,
        CorDebug_Gen2 = 2,
        CorDebug_LOH = 3,
    }

    public enum CorGCReferenceType : uint
    {
        CorHandleStrong = 1<<0,
        CorHandleStrongPinning = 1<<1,
        CorHandleWeakShort = 1<<2,
        CorHandleWeakLong = 1<<3,
        CorHandleWeakRefCount = 1<<4,
        CorHandleStrongRefCount = 1<<5,
        CorHandleStrongDependent = 1<<6,
        CorHandleStrongAsyncPinned = 1<<7,
        CorHandleStrongSizedByref = 1<<8,

        CorReferenceStack = 0x80000001,
        CorReferenceFinalizer = 80000002,

        // Used for EnumHandles
        CorHandleStrongOnly = 0x1E3,
        CorHandleWeakOnly = 0x1C,
        CorHandleAll = 0x7FFFFFFF
    };

    public enum CorDebugNGENPolicy
    {
        DISABLE_LOCAL_NIC = 1 // indicates that the native image cache for a modern application should be ignored
    };

    [ComImport, Guid("FE06DC28-49FB-4636-A4A3-E80DB4AE116C"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugDataTarget
    {
        [PreserveSig]
        uint GetPlatform(out CorDebugPlatform pTargetPlatform);
        [PreserveSig]
        uint ReadVirtual(UInt64 address, IntPtr pBuffer, UInt32 bytesRequested, out UInt32 pBytesRead);
        [PreserveSig]
        uint GetThreadContext(uint dwThreadId, UInt32 contextFlags, UInt32 contextSize, IntPtr pContext);
    }

    [ComImport, Guid("3D6F5F62-7538-11D3-8D5B-00104B35E7EF"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugController
    {
        void Stop([In] uint dwTimeoutIgnored);
        void Continue([In] int fIsOutOfBand);
        void IsRunning(out int pbRunning);
        void HasQueuedCallbacks([In, MarshalAs(UnmanagedType.Interface)] ICorDebugThread pThread, out int pbQueued);
        void EnumerateThreads([MarshalAs(UnmanagedType.Interface)] out ICorDebugThreadEnum ppThreads);
        void SetAllThreadsDebugState([In] CorDebugThreadState state, [In, MarshalAs(UnmanagedType.Interface)] ICorDebugThread pExceptThisThread);
        void Detach();
        void Terminate([In] uint exitCode);
        void CanCommitChanges([In] uint cSnapshots, [In, MarshalAs(UnmanagedType.Interface)] ref ICorDebugEditAndContinueSnapshot pSnapshots, [MarshalAs(UnmanagedType.Interface)] out ICorDebugErrorInfoEnum pError);
        void CommitChanges([In] uint cSnapshots, [In, MarshalAs(UnmanagedType.Interface)] ref ICorDebugEditAndContinueSnapshot pSnapshots, [MarshalAs(UnmanagedType.Interface)] out ICorDebugErrorInfoEnum pError);
    }

    [ComImport, Guid("CC7BCB01-8A68-11D2-983C-0000F808342D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugEnum
    {
        [PreserveSig]
        int Skip([In] uint celt);
        [PreserveSig]
        int Reset();
        [PreserveSig]
        int Clone([MarshalAs(UnmanagedType.Interface)] out ICorDebugEnum ppEnum);
        [PreserveSig]
        int GetCount(out uint pcelt);
    }

    [ComImport, Guid("3D6F5F64-7538-11D3-8D5B-00104B35E7EF"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugProcess : ICorDebugController
    {
        // ICorDebugController
        new void Stop([In] uint dwTimeoutIgnored);
        new void Continue([In] int fIsOutOfBand);
        new void IsRunning(out int pbRunning);
        new void HasQueuedCallbacks([In, MarshalAs(UnmanagedType.Interface)] ICorDebugThread pThread, out int pbQueued);
        [PreserveSig]
        new int EnumerateThreads([MarshalAs(UnmanagedType.Interface)] out ICorDebugThreadEnum ppThreads);
        new void SetAllThreadsDebugState([In] CorDebugThreadState state, [In, MarshalAs(UnmanagedType.Interface)] ICorDebugThread pExceptThisThread);
        new void Detach();
        new void Terminate([In] uint exitCode);
        new void CanCommitChanges([In] uint cSnapshots, [In, MarshalAs(UnmanagedType.Interface)] ref ICorDebugEditAndContinueSnapshot pSnapshots, [MarshalAs(UnmanagedType.Interface)] out ICorDebugErrorInfoEnum pError);
        new void CommitChanges([In] uint cSnapshots, [In, MarshalAs(UnmanagedType.Interface)] ref ICorDebugEditAndContinueSnapshot pSnapshots, [MarshalAs(UnmanagedType.Interface)] out ICorDebugErrorInfoEnum pError);
        // ICorDebugProcess
        [PreserveSig]
        int GetID(out uint pdwProcessId);
        void GetHandle([ComAliasName("HPROCESS*")] out IntPtr phProcessHandle);
        void GetThread([In] uint dwThreadId, [MarshalAs(UnmanagedType.Interface)] out ICorDebugThread ppThread);
        void EnumerateObjects([MarshalAs(UnmanagedType.Interface)] out ICorDebugObjectEnum ppObjects);
        void IsTransitionStub([In] ulong address, out int pbTransitionStub);
        void IsOSSuspended([In] uint threadID, out int pbSuspended);
        void GetThreadContext([In] uint threadID, [In] uint contextSize, [In, Out, MarshalAs(UnmanagedType.Interface)] ICorDebugProcess context);
        void SetThreadContext([In] uint threadID, [In] uint contextSize, [In, MarshalAs(UnmanagedType.Interface)] ICorDebugProcess context);
        [PreserveSig]
        //uint ReadMemory([In] ulong address, [In] uint size, [Out, MarshalAs(UnmanagedType.LPArray)] byte[] buffer, out uint read);
        int ReadMemory([In] ulong address, [In] uint size, IntPtr buffer, out uint read);
        void WriteMemory([In] ulong address, [In] uint size, [In] ref byte buffer, out uint written);
        void ClearCurrentException([In] uint threadID);
        void EnableLogMessages([In] int fOnOff);
        void ModifyLogSwitch([In] ref ushort pLogSwitchName, [In] int lLevel);
        [PreserveSig]
        int EnumerateAppDomains([MarshalAs(UnmanagedType.Interface)] out ICorDebugAppDomainEnum ppAppDomains);
        void GetObject([MarshalAs(UnmanagedType.Interface)] out ICorDebugValue ppObject);
        void ThreadForFiberCookie([In] uint fiberCookie, [MarshalAs(UnmanagedType.Interface)] out ICorDebugThread ppThread);
        void GetHelperThreadID(out uint pThreadID);
    }

    public struct COR_VERSION
    {
        uint dwMajor;
        uint dwMinor;
        uint dwBuild;
        uint dwSubBuild;
    }

    [ComImport, Guid("AD1B3588-0EF0-4744-A496-AA09A9F80371"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    // Untested
    public interface ICorDebugProcess2
    {
        [PreserveSig]
        int GetThreadForTaskID(ulong taskid, [MarshalAs(UnmanagedType.Interface)] out ICorDebugThread2 ppThread);
        [PreserveSig]
        int GetVersion(out COR_VERSION version);
        [PreserveSig]
        // Def not tested
        int SetUnmanagedBreakpoint(UInt64 address, UInt32 bufsize, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] byte[] buffer, out UInt32 bufLen);
        [PreserveSig]
        int ClearUnmanagedBreakpoint(UInt64 address);
        [PreserveSig]
        int SetDesiredNGENCompilerFlags(uint pdwFlags);
        [PreserveSig]
        int GetDesiredNGENCompilerFlags(out uint pdwFlags);
        [PreserveSig]
        int GetReferenceValueFromGCHandle(IntPtr handle,  [MarshalAs(UnmanagedType.Interface)] out ICorDebugReferenceValue pOutValue);
    }

    [ComImport, Guid("2EE06488-C0D4-42B1-B26D-F3795EF606FB"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    // Untested
    public interface ICorDebugProcess3
    {
        [PreserveSig]
        int SetEnableCustomNotification(ICorDebugClass pClass, bool fEnable);
    }

    public struct COR_HEAPINFO
    {
        public bool areGCStructuresValid;  // TRUE if it's ok to walk the heap, FALSE otherwise.
        public Int32 pointerSize;          // The size of pointers on the target architecture in bytes.
        public Int32 numHeaps;             // The number of logical GC heaps in the process.
        public bool concurrent;            // Is the GC concurrent?
        public CorDebugGCType gcType;      // Workstation or Server?
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct COR_TYPEID
    {
        public ulong token1;
        public ulong token2;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct COR_HEAPOBJECT
    {
        public UInt64 address;  // The address (in memory) of the object. (CORDB_ADDRESS == ULONG64)
        public ulong size;           // The total size of the object.
        public COR_TYPEID type;        // The fully instantiated type of the object.
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct COR_SEGMENT
    {
        public UInt64 start;          // The start address of the segment. CORDB_ADDRESS 
        public UInt64 end;            // The end address of the segment.
        public CorDebugGenerationTypes type; // The generation of the segment.
        public uint heap;                   // The heap the segment resides in.
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct COR_GC_REFERENCE
    {
        [MarshalAs(UnmanagedType.Interface)]
        public ICorDebugAppDomain Domain;         // The AppDomain of the handle/object, may be null.
        [MarshalAs(UnmanagedType.Interface)]
        public ICorDebugValue Location;           // A reference to the object
        public CorGCReferenceType Type;            // Where the root came from.
    
    /*
        DependentSource - for HandleDependent
        RefCount - for HandleStrongRefCount
        Size - for HandleSizedByref
    */
        UInt64 ExtraData;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct COR_ARRAY_LAYOUT 
    {
        public COR_TYPEID componentID;
        public CorElementType componentType;
        public uint firstElementOffset;
        public uint elementSize;
        public uint countOffset;
        public uint rankSize;
        public uint numRanks;
        public uint rankOffset; 
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct COR_TYPE_LAYOUT 
    {
        public COR_TYPEID parentID;
        public uint objectSize;
        public uint numFields;
        public uint boxOffset;
        public CorElementType type;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct COR_FIELD
    {
        public uint token; /* mdFieldDef */
        public uint offset;
        public COR_TYPEID id;
        public CorElementType fieldType;
    };

    [ComImport, Guid("21e9d9c0-fcb8-11df-8cff-0800200c9a66"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugProcess5
    {
        [PreserveSig]
        int GetGCHeapInformation(out COR_HEAPINFO pHeapInfo);
        [PreserveSig]
        int EnumerateHeap([MarshalAs(UnmanagedType.Interface)] out ICorDebugHeapEnum ppObjects);
        [PreserveSig]
        int EnumerateHeapRegions([Out, MarshalAs(UnmanagedType.Interface)] out ICorDebugHeapSegmentEnum ppRegions);
        [PreserveSig]
        int GetObject(UInt64 addr, [Out, MarshalAs(UnmanagedType.Interface)] out ICorDebugObjectValue pObject);
        [PreserveSig]
        int EnumerateGCReferences(bool enumerateWeakReferences, [Out, MarshalAs(UnmanagedType.Interface)] out ICorDebugGCReferenceEnum ppEnum);
        [PreserveSig]
        int EnumerateHandles([In] CorGCReferenceType types, [Out, MarshalAs(UnmanagedType.Interface)] out ICorDebugGCReferenceEnum ppEnum);
        [PreserveSig]
        int GetTypeID([In] UInt64 /*CORDB_ADDRESS*/ obj, out COR_TYPEID pId);
        [PreserveSig]
        int GetTypeForTypeID(COR_TYPEID id, [Out, MarshalAs(UnmanagedType.Interface)] out ICorDebugType ppType);
        [PreserveSig]
        int GetArrayLayout(COR_TYPEID id, [Out] out COR_ARRAY_LAYOUT pLayout);
        [PreserveSig]
        int GetTypeLayout(COR_TYPEID id, [Out] out COR_TYPE_LAYOUT pLayout);
        [PreserveSig]
        int GetTypeFields(COR_TYPEID id, uint celt, [Out, MarshalAs(UnmanagedType.LPArray)] COR_FIELD[] fields, [Out] out uint pceltNeeded);
        [PreserveSig]
        int EnableNGENPolicy(CorDebugNGENPolicy ePolicy);
    }

    [ComImport, Guid("938C6D66-7FB6-4F69-B389-425B8987329B"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugThread
    {
        [PreserveSig]
        int GetProcess([MarshalAs(UnmanagedType.Interface)] out ICorDebugProcess ppProcess);
        [PreserveSig]
        int GetID(out uint pdwThreadId);
        [PreserveSig]
        int GetHandle(out IntPtr phThreadHandle); //HTHREAD
        [PreserveSig]
        int GetAppDomain([MarshalAs(UnmanagedType.Interface)] out ICorDebugAppDomain ppAppDomain);
        [PreserveSig]
        int SetDebugState([In] CorDebugThreadState state);
        [PreserveSig]
        int GetDebugState(out CorDebugThreadState pState);
        [PreserveSig]
        int GetUserState(out CorDebugUserState pState);
        [PreserveSig]
        int GetCurrentException([MarshalAs(UnmanagedType.Interface)] out ICorDebugValue ppExceptionObject);
        [PreserveSig]
        int ClearCurrentException(); // Not implemented
        [PreserveSig]
        int CreateStepper([MarshalAs(UnmanagedType.Interface)] out ICorDebugStepper ppStepper);
        [PreserveSig]
        int EnumerateChains([MarshalAs(UnmanagedType.Interface)] out ICorDebugChainEnum ppChains);
        [PreserveSig]
        int GetActiveChain([MarshalAs(UnmanagedType.Interface)] out ICorDebugChain ppChain);
        [PreserveSig]
        int GetActiveFrame([MarshalAs(UnmanagedType.Interface)] out ICorDebugFrame ppFrame);
        [PreserveSig]
        int GetRegisterSet([MarshalAs(UnmanagedType.Interface)] out ICorDebugRegisterSet ppRegisters);
        [PreserveSig]
        int CreateEval([MarshalAs(UnmanagedType.Interface)] out ICorDebugEval ppEval);
        [PreserveSig]
        int GetObject([MarshalAs(UnmanagedType.Interface)] out ICorDebugValue ppObject);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct COR_ACTIVE_FUNCTION
    {
        public ICorDebugAppDomain pAppDomain;   // Pointer to the owning AppDomain of the below IL Offset.
        public ICorDebugModule pModule;         // Pointer to the owning Module of the below IL Offset.
        public ICorDebugFunction2 pFunction;    // Pointer to the owning Function of the below IL Offset.
        public uint ilOffset;                 // IL Offset of the frame.
        public uint flags;                    // Bit mask of flags, currently unused.  Reserved.
    };

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("2BD956D9-7B07-4BEF-8A98-12AA862417C5")]
    public interface ICorDebugThread2
    {
        [PreserveSig]
        int GetActiveFunctions([In] uint cFunctions, out uint pcFunctions, [In, Out, MarshalAs(UnmanagedType.LPArray)] COR_ACTIVE_FUNCTION[] pFunctions);
        [PreserveSig]
        int GetConnectionID(out uint pdwConnectionId);
        [PreserveSig]
        int GetTaskID(out ulong pTaskId);
        [PreserveSig]
        int GetVolatileOSThreadID(out uint pdwTid);
        [PreserveSig]
        int InterceptCurrentException([MarshalAs(UnmanagedType.Interface)] [In] ICorDebugFrame pFrame);
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("F8544EC3-5E4E-46C7-8D3E-A52B8405B1F5")]
    public interface ICorDebugThread3
    {
        [PreserveSig]
        int CreateStackWalk([MarshalAs(UnmanagedType.Interface)] out ICorDebugStackWalk ppStackWalk);
        [PreserveSig]
        int GetActiveInternalFrames([In] uint cInternalFrames, out uint pcInternalFrames, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] ICorDebugInternalFrame2[] ppInternalFrames);
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("1A1F204B-1C66-4637-823F-3EE6C744A69C")]
    public interface ICorDebugThread4
    {
        [PreserveSig]
        int HasUnhandledException();
        [PreserveSig]
        int GetBlockingObjects([MarshalAs(UnmanagedType.Interface)] out ICorDebugBlockingObjectEnum ppBlockingObjectEnum);
        [PreserveSig]
        int GetCurrentCustomDebuggerNotification([MarshalAs(UnmanagedType.Interface)] out ICorDebugValue ppNotificationObject);
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("3D6F5F63-7538-11D3-8D5B-00104B35E7EF")]
    public interface ICorDebugAppDomain : ICorDebugController
    {
        // ICorDebugController
        new void Stop([In] uint dwTimeoutIgnored);
        new void Continue([In] int fIsOutOfBand);
        new void IsRunning(out int pbRunning);
        new void HasQueuedCallbacks([In, MarshalAs(UnmanagedType.Interface)] ICorDebugThread pThread, out int pbQueued);
        new void EnumerateThreads([MarshalAs(UnmanagedType.Interface)] out ICorDebugThreadEnum ppThreads);
        new void SetAllThreadsDebugState([In] CorDebugThreadState state, [In, MarshalAs(UnmanagedType.Interface)] ICorDebugThread pExceptThisThread);
        new void Detach();
        new void Terminate([In] uint exitCode);
        new void CanCommitChanges([In] uint cSnapshots, [In, MarshalAs(UnmanagedType.Interface)] ref ICorDebugEditAndContinueSnapshot pSnapshots, [MarshalAs(UnmanagedType.Interface)] out ICorDebugErrorInfoEnum pError);
        new void CommitChanges([In] uint cSnapshots, [In, MarshalAs(UnmanagedType.Interface)] ref ICorDebugEditAndContinueSnapshot pSnapshots, [MarshalAs(UnmanagedType.Interface)] out ICorDebugErrorInfoEnum pError);
        // ICorDebugAppDomain
        [PreserveSig]
        int GetProcess([MarshalAs(UnmanagedType.Interface)] out ICorDebugProcess ppProcess);
        [PreserveSig]
        int EnumerateAssemblies([MarshalAs(UnmanagedType.Interface)] out ICorDebugAssemblyEnum ppAssemblies);
        void GetModuleFromMetaDataInterface([In, MarshalAs(UnmanagedType.IUnknown)] object pIMetaData, [MarshalAs(UnmanagedType.Interface)] out ICorDebugModule ppModule);
        void EnumerateBreakpoints([MarshalAs(UnmanagedType.Interface)] out ICorDebugBreakpointEnum ppBreakpoints);
        void EnumerateSteppers([MarshalAs(UnmanagedType.Interface)] out ICorDebugStepperEnum ppSteppers);
        void IsAttached(out int pbAttached);
        //void GetName([In] uint cchName, out uint pcchName, out char[] szName);
        int GetName([In] uint cchName, out uint pcchName, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder szName);
        void GetObject([MarshalAs(UnmanagedType.Interface)] out ICorDebugValue ppObject);
        void Attach();
        void GetID(out uint pId);
    }

    [ComImport, Guid("DF59507C-D47A-459E-BCE2-6427EAC8FD06"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugAssembly
    {
        [PreserveSig]
        int GetProcess([MarshalAs(UnmanagedType.Interface)] out ICorDebugProcess ppProcess);
        [PreserveSig]
        int GetAppDomain([MarshalAs(UnmanagedType.Interface)] out ICorDebugAppDomain ppAppDomain);
        [PreserveSig]
        int EnumerateModules([MarshalAs(UnmanagedType.Interface)] out ICorDebugModuleEnum ppModules);
        [PreserveSig]
        int GetCodeBase([In] uint cchName, out uint pcchName, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder szName); // Not implement according to cordebug.idl
        [PreserveSig]
        int GetName([In] uint cchName, out uint pcchName, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder szName);
    }

    [ComImport, Guid("DBA2D8C1-E5C5-4069-8C13-10A7C6ABF43D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugModule
    {
        [PreserveSig]
        int GetProcess([MarshalAs(UnmanagedType.Interface)] out ICorDebugProcess ppProcess);
        [PreserveSig]
        int GetBaseAddress(out ulong pAddress);
        [PreserveSig]
        int GetAssembly([MarshalAs(UnmanagedType.Interface)] out ICorDebugAssembly ppAssembly);
        [PreserveSig]
        int GetName([In] uint cchName, out uint pcchName, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder szName);
        void EnableJITDebugging([In] bool bTrackJITInfo, [In] bool bAllowJitOpts);
        void EnableClassLoadCallbacks([In] bool bClassLoadCallbacks);
        void GetFunctionFromToken([In] uint methodDef, [MarshalAs(UnmanagedType.Interface)] out ICorDebugFunction ppFunction);
        void GetFunctionFromRVA([In] ulong rva, [MarshalAs(UnmanagedType.Interface)] out ICorDebugFunction ppFunction);
        void GetClassFromToken([In] uint typeDef, [MarshalAs(UnmanagedType.Interface)] out ICorDebugClass ppClass);
        void CreateBreakpoint([MarshalAs(UnmanagedType.Interface)] out ICorDebugModuleBreakpoint ppBreakpoint);
        void GetEditAndContinueSnapshot([MarshalAs(UnmanagedType.Interface)] out ICorDebugEditAndContinueSnapshot ppEditAndContinueSnapshot);
        void GetMetaDataInterface([In] ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppObj);
        void GetToken(out uint pToken);
        void IsDynamic(out bool pDynamic);
        void GetGlobalVariableValue([In] uint fieldDef, [MarshalAs(UnmanagedType.Interface)] out ICorDebugValue ppValue);
        [PreserveSig]
        int GetSize(out uint pcBytes);
        void IsInMemory(out bool pInMemory);
    }

    [ComImport, Guid("CC7BCAF7-8A68-11D2-983C-0000F808342D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugValue
    {
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("18AD3D6E-B7D2-11D2-BD04-0000F80849BD")]
    public interface ICorDebugObjectValue : ICorDebugValue
    {
        //#region Inherited from ICorDebugValue
        //[PreserveSig]
        //new uint GetType(out CorElementType pType);
        //[PreserveSig]
        //new uint GetSize(out uint pSize);
        //[PreserveSig]
        //new uint GetAddress(out ulong pAddress);
        //[PreserveSig]
        //new uint CreateBreakpoint([MarshalAs(UnmanagedType.Interface)] out ICorDebugValueBreakpoint ppBreakpoint);
        //#endregion

        ///*
        // * GetClass returns the runtime class of the object in the value.
        // */
        //[PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        //uint GetClass([MarshalAs(UnmanagedType.Interface)] out ICorDebugClass ppClass);

        ///*
        // * GetFieldValue returns a value for the given field in the given
        // * class. The class must be on the class hierarchy of the object's
        // * class, and the field must be a field of that class.
        // */
        //[PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        //uint GetFieldValue([In, MarshalAs(UnmanagedType.Interface)] ICorDebugClass pClass, [In] uint fieldDef, [Out, MarshalAs(UnmanagedType.Interface)] out ICorDebugValue ppValue);

        ///*
        // * NOT YET IMPLEMENTED
        // */
        //[PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        //uint GetVirtualMethod([In] uint memberRef, [MarshalAs(UnmanagedType.Interface)] out ICorDebugFunction ppFunction);

        ///*
        // * NOT YET IMPLEMENTED
        // */
        //[PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        //uint GetContext([MarshalAs(UnmanagedType.Interface)] out ICorDebugContext ppContext);

        ///*
        // * IsValueClass returns true if the the class of this object is
        // * a value class.
        // */
        //[PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        //uint IsValueClass(out bool pbIsValueClass);

        ///*
        // * DEPRECATED
        // */
        //[PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        //uint GetManagedCopy([MarshalAs(UnmanagedType.IUnknown)] out object ppObject);

        ///*
        // * DEPRECATED
        // */
        //[PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        //uint SetFromManagedCopy([In, MarshalAs(UnmanagedType.IUnknown)] object pObject);
    }
    
    [ComImport, Guid("CC7BCAF9-8A68-11d2-983C-0000F808342D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugReferenceValue : ICorDebugValue
    {
/*
        // ICorDebugValue
        [PreserveSig]
        new uint GetType(out CorElementType pType);
        [PreserveSig]
        new uint GetSize(out uint pSize);
        [PreserveSig]
        new uint GetAddress(out ulong pAddress);
        [PreserveSig]
        new uint CreateBreakpoint([MarshalAs(UnmanagedType.Interface)] out ICorDebugValueBreakpoint ppBreakpoint);
        // ICorDebugReferenceValue

        [PreserveSig]
        uint IsNull([Out] out bool pbNull);

        [PreserveSig]
        uint GetValue([Out] out ulong pValue); //  CORDB_ADDRESS 

        [PreserveSig]
        uint SetValue(ulong value);  // CORDB_ADDRESS 

        [PreserveSig]
        uint Dereference([MarshalAs(UnmanagedType.Interface), Out] out ICorDebugValue ppValue);

        [PreserveSig]
        uint DereferenceStrong([MarshalAs(UnmanagedType.Interface), Out]  out ICorDebugValue ppValue);
*/
    };



    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("CC7BCAF5-8A68-11D2-983C-0000F808342D")]
    public interface ICorDebugClass
    {
    }

    [ComImport, Guid("CC7BCAF3-8A68-11D2-983C-0000F808342D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugFunction
    {
    }

    [ComImport, Guid("EF0C490B-94C3-4E4D-B629-DDC134C532D8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugFunction2
    {
        void SetJMCStatus([In] int bIsJustMyCode);
        void GetJMCStatus(out int pbIsJustMyCode);
        void EnumerateNativeCode([MarshalAs(UnmanagedType.Interface)] out ICorDebugCodeEnum ppCodeEnum);
        void GetVersionNumber(out uint pnVersion);
    }

    [ComImport, Guid("CC7BCAEF-8A68-11D2-983C-0000F808342D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugFrame
    {
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("C0815BDC-CFAB-447E-A779-C116B454EB5B")]
    public interface ICorDebugInternalFrame2
    {
        void GetAddress(out ulong pAddress);
        void IsCloserToLeaf([MarshalAs(UnmanagedType.Interface)] [In] ICorDebugFrame pFrameToCompare, out int pIsCloser);
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("CC7BCB0B-8A68-11D2-983C-0000F808342D")]
    public interface ICorDebugRegisterSet
    {
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("CC7BCAF4-8A68-11D2-983C-0000F808342D")]
    public interface ICorDebugCode
    {
        void IsIL(out int pbIL);
        void GetFunction([MarshalAs(UnmanagedType.Interface)] out ICorDebugFunction ppFunction);
        void GetAddress(out ulong pStart);
        void GetSize(out uint pcBytes);
        void CreateBreakpoint([In] uint offset, [MarshalAs(UnmanagedType.Interface)] out ICorDebugFunctionBreakpoint ppBreakpoint);
        void GetCode([In] uint startOffset, [In] uint endOffset, [In] uint cBufferAlloc, [Out, MarshalAs(UnmanagedType.Interface)] ICorDebugCode buffer, out uint pcBufferSize);
        void GetVersionNumber(out uint nVersion);
        void GetILToNativeMapping([In] uint cMap, out uint pcMap, [Out, MarshalAs(UnmanagedType.Interface)] ICorDebugCode map);
        void GetEnCRemapSequencePoints([In] uint cMap, out uint pcMap, [Out, MarshalAs(UnmanagedType.Interface)] ICorDebugCode offsets);
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("A0647DE9-55DE-4816-929C-385271C64CF7")]
    public interface ICorDebugStackWalk
    {
        [PreserveSig]
        int GetContext([In] uint contextFlags, [In] uint contextBufSize, out uint contextSize, IntPtr contextBuf);
        [PreserveSig]
        int SetContext([In] CorDebugSetContextFlag flag, [In] uint contextSize, IntPtr context);
        [PreserveSig]
        int Next();
        [PreserveSig]
        int GetFrame([MarshalAs(UnmanagedType.Interface)] out ICorDebugFrame pFrame);
    }

    [ComImport, Guid("D613F0BB-ACE1-4C19-BD72-E4C08D5DA7F5"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugType
    {
/*
        [PreserveSig]
        uint GetType(out uint ty);
        [PreserveSig]
        uint GetClass([MarshalAs(UnmanagedType.Interface)] out ICorDebugClass ppClass);
        [PreserveSig]
        uint EnumerateTypeParameters([MarshalAs(UnmanagedType.Interface)] out ICorDebugTypeEnum ppTyParEnum);
        [PreserveSig]
        uint GetFirstTypeParameter([MarshalAs(UnmanagedType.Interface)] out ICorDebugType value);
        [PreserveSig]
        uint GetBase([MarshalAs(UnmanagedType.Interface)] out ICorDebugType pBase);
        [PreserveSig]
        uint GetStaticFieldValue([In] uint fieldDef, [In, MarshalAs(UnmanagedType.Interface)] ICorDebugFrame pFrame, [Out, MarshalAs(UnmanagedType.Interface)] out ICorDebugValue ppValue);
        [PreserveSig]
        uint GetRank(out uint pnRank);
*/
    }

    #region Enumerators

    [ComImport, Guid("CC7BCB09-8A68-11D2-983C-0000F808342D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugModuleEnum : ICorDebugEnum
    {
        // ICorDebugEnum
        new int Skip([In] uint celt);
        new int Reset();
        new int Clone([MarshalAs(UnmanagedType.Interface)] out ICorDebugEnum ppEnum);
        new int GetCount(out uint pcelt);
        // ICorDebugModuleEnum
        [PreserveSig]
        int Next([In] uint celt, [Out, MarshalAs(UnmanagedType.LPArray)] ICorDebugModule[] values, out uint pceltFetched);
    }

    [ComImport, Guid("4A2A1EC9-85EC-4BFB-9F15-A89FDFE0FE83"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugAssemblyEnum : ICorDebugEnum
    {
        // ICorDebugEnum
        new int Skip([In] uint celt);
        new int Reset();
        new int Clone([MarshalAs(UnmanagedType.Interface)] out ICorDebugEnum ppEnum);
        new int GetCount(out uint pcelt);
        // ICorDebugAssemblyEnum
        [PreserveSig]
        int Next([In] uint celt, [Out, MarshalAs(UnmanagedType.LPArray)] ICorDebugAssembly[] values, out uint pceltFetched);
    }

    [ComImport, Guid("63CA1B24-4359-4883-BD57-13F815F58744"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugAppDomainEnum : ICorDebugEnum
    {
        // ICorDebugEnum
        new int Skip([In] uint celt);
        new int Reset();
        new int Clone([MarshalAs(UnmanagedType.Interface)] out ICorDebugEnum ppEnum);
        new int GetCount([Out, MarshalAs(UnmanagedType.U4)] out uint pcelt);
        // ICorDebugAppDomainEnum
        void Next([In] uint celt, [Out, MarshalAs(UnmanagedType.LPArray)] ICorDebugAppDomain[] values, out uint pceltFetched);
    }

    [ComImport, Guid("CC7BCB06-8A68-11D2-983C-0000F808342D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugThreadEnum : ICorDebugEnum
    {
        // ICorDebugEnum
        new int Skip([In] uint celt);
        new int Reset();
        new int Clone([MarshalAs(UnmanagedType.Interface)] out ICorDebugEnum ppEnum);
        new int GetCount(out uint pcelt);
        // ICorDebugEnum
        [PreserveSig]
        int Next([In] uint celt, [Out, MarshalAs(UnmanagedType.LPArray)] ICorDebugThread[] threads, out uint pceltFetched);
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("55E96461-9645-45E4-A2FF-0367877ABCDE")]
    public interface ICorDebugCodeEnum : ICorDebugEnum
    {
        // ICorDebugEnum
        new int Skip([In] uint celt);
        new int Reset();
        new int Clone([MarshalAs(UnmanagedType.Interface)] out ICorDebugEnum ppEnum);
        new int GetCount(out uint pcelt);
        // ICorDebugCodeEnum
        void Next([In] uint celt, [Out, MarshalAs(UnmanagedType.Interface)] ICorDebugCode[] values, out uint pceltFetched);
    }


    [ComImport, Guid("CC7BCB02-8A68-11D2-983C-0000F808342D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown),]
    public interface ICorDebugObjectEnum : ICorDebugEnum
    {
    }

    [ComImport, Guid("976A6278-134A-4a81-81A3-8F277943F4C3"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown),]
    public interface ICorDebugBlockingObjectEnum : ICorDebugEnum
    {
    }

    [ComImport, Guid("76D7DAB8-D044-11DF-9A15-7E29DFD72085"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugHeapEnum : ICorDebugEnum
    {
        // ICorDebugEnum
        new int Skip([In] uint celt);
        new int Reset();
        new int Clone([MarshalAs(UnmanagedType.Interface)] out ICorDebugEnum ppEnum);
        new int GetCount(out uint pcelt);
        // ICorDebugHeapEnum
        [PreserveSig]
        int Next([In] uint celt, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] COR_HEAPOBJECT[] objects, out uint pceltFetched);
    }

    [ComImport, Guid("A2FA0F8E-D045-11DF-AC8E-CE2ADFD72085"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugHeapSegmentEnum : ICorDebugEnum
    {
        // ICorDebugHeapSegmentEnum
        new int Skip([In] uint celt);
        new int Reset();
        new int Clone([MarshalAs(UnmanagedType.Interface)] out ICorDebugEnum ppEnum);
        new int GetCount(out uint pcelt);
        // ICorDebugHeapSegmentEnum
        int Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] COR_SEGMENT[] objects, out uint pceltFetched);
    }

    [ComImport, Guid("7F3C24D3-7E1D-4245-AC3A-F72F8859C80C"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugGCReferenceEnum : ICorDebugEnum
    {
        // ICorDebugEnum
        new int Skip([In] uint celt);
        new int Reset();
        new int Clone([MarshalAs(UnmanagedType.Interface)] out ICorDebugEnum ppEnum);
        new int GetCount(out uint pcelt);
        // ICorDebugGCReferenceEnum
        [PreserveSig]
        int Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1), Out] COR_GC_REFERENCE[] roots, out uint pceltFetched);
    }


    #endregion

    #region Incomplete/unused interfaces (breakpoints, steppers, etc)

    [ComImport, Guid("F0E18809-72B5-11D2-976F-00A0C9B4D50C"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugErrorInfoEnum : ICorDebugEnum
    {
    }

    [ComImport, Guid("6DC3FA01-D7CB-11D2-8A95-0080C792E5D8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugEditAndContinueSnapshot
    {
    }

    [ComImport, Guid("CC7BCB04-8A68-11D2-983C-0000F808342D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugStepperEnum : ICorDebugEnum
    {
        // ICorDebugEnum
        new int Skip([In] uint celt);
        new int Reset();
        new int Clone([MarshalAs(UnmanagedType.Interface)] out ICorDebugEnum ppEnum);
        new int GetCount(out uint pcelt);
        // ICorDebugStepperEnum
        void Next([In] uint celt, [Out, MarshalAs(UnmanagedType.Interface)] ICorDebugStepperEnum steppers, out uint pceltFetched);
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("CC7BCB03-8A68-11D2-983C-0000F808342D")]
    public interface ICorDebugBreakpointEnum : ICorDebugEnum
    {
        // ICorDebugEnum
        new int Skip([In] uint celt);
        new int Reset();
        new int Clone([MarshalAs(UnmanagedType.Interface)] out ICorDebugEnum ppEnum);
        new int GetCount(out uint pcelt);
        // ICorDebugBreakpointEnum
        void Next([In] uint celt, [Out, MarshalAs(UnmanagedType.Interface)] ICorDebugBreakpointEnum breakpoints, out uint pceltFetched);
    }

    [ComImport, Guid("CC7BCAE8-8A68-11D2-983C-0000F808342D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugBreakpoint
    {
        void Activate([In] int bActive);
        void IsActive(out int pbActive);
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("CC7BCAEA-8A68-11D2-983C-0000F808342D")]
    public interface ICorDebugModuleBreakpoint : ICorDebugBreakpoint
    {
    }

    [ComImport, Guid("CC7BCAEC-8A68-11D2-983C-0000F808342D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugStepper
    {
    }

    [ComImport, Guid("CC7BCB08-8A68-11D2-983C-0000F808342D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugChainEnum : ICorDebugEnum
    {
    }

    [ComImport, Guid("CC7BCAEE-8A68-11D2-983C-0000F808342D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICorDebugChain
    {
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("CC7BCAF6-8A68-11D2-983C-0000F808342D")]
    public interface ICorDebugEval
    {
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("CC7BCAE9-8A68-11D2-983C-0000F808342D")]
    public interface ICorDebugFunctionBreakpoint : ICorDebugBreakpoint
    {
    }


    #endregion


}
