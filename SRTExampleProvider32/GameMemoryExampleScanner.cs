using ProcessMemory;
using static ProcessMemory.Extensions;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SRTExampleProvider32.Structs;
using System.Text;
using SRTExampleProvider32.Structs.GameStructs;

namespace SRTExampleProvider32
{
    internal class GameMemoryExampleScanner : IDisposable
    {
        /// <summary>
        /// READ ONLY VARIABLES
        /// </summary>
        private static readonly int MAX_ENEMIES = 64; // USE FOR ARRAYS OF ENEMIES MAX COUNT
        private static readonly int MAX_ITEMS = 24; // USE FOR ARRAYS OF ITEMS MAX COUNT

        /// <summary>
        /// VARIABLES
        /// </summary>
        private ProcessMemoryHandler memoryAccess;
        private GameMemoryExample gameMemoryValues;
        public bool HasScanned;
        public bool ProcessRunning => memoryAccess != null && memoryAccess.ProcessRunning;
        public int ProcessExitCode => (memoryAccess != null) ? memoryAccess.ProcessExitCode : 0;


        /// <summary>
        /// POINTER ADDRESS VARIABLES
        /// </summary>
        private int pointerAddressExample;

        /// <summary>
        /// POINTER VARIABLES
        /// </summary>
        private IntPtr BaseAddress { get; set; }
        private MultilevelPointer PointerExample { get; set; }

        /// <summary>
        /// CLASS CONTRUCTOR
        /// </summary>
        /// <param name="proc"></param>
        internal GameMemoryExampleScanner(Process process = null)
        {
            gameMemoryValues = new GameMemoryExample();
            if (process != null)
                Initialize(process);
        }

        internal void Initialize(Process process)
        {
            if (process == null)
                return; // Do not continue if this is null.

            SelectPointerAddresses(GameHashes.DetectVersion(process.MainModule.FileName));

            int pid = GetProcessId(process).Value;
            memoryAccess = new ProcessMemoryHandler(pid);
            if (ProcessRunning)
            {
                BaseAddress = NativeWrappers.GetProcessBaseAddress(pid, PInvoke.ListModules.LIST_MODULES_64BIT); // Bypass .NET's managed solution for getting this and attempt to get this info ourselves via PInvoke since some users are getting 299 PARTIAL COPY when they seemingly shouldn't.
                PointerExample = new MultilevelPointer(memoryAccess, IntPtr.Add(BaseAddress, pointerAddressExample));
            }
        }

        private void SelectPointerAddresses(GameVersion version)
        {
            if (version == GameVersion.GameName_Region_ReleaseData_Patch)
            {
                pointerAddressExample = 0x0;
            }
            else if (version == GameVersion.UNKNOWN)
            {
                pointerAddressExample = 0x0;
            }
        }

        internal IGameMemoryExample Refresh()
        {
            // Example Without MultiLevelPointer
            gameMemoryValues._example = memoryAccess.GetAt<GameExampleStruct>(IntPtr.Add(BaseAddress, pointerAddressExample));

            // Example With MultiLevelPointer
            gameMemoryValues._example = PointerExample.Deref<GameExampleStruct>(0x0);

            HasScanned = true;
            return gameMemoryValues;
        }

        private int? GetProcessId(Process process) => process?.Id;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (memoryAccess != null)
                        memoryAccess.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~REmake1Memory() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}