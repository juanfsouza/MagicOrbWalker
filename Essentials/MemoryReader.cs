using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MagicOrbwalker1.Essentials
{
    public class MemoryReader : IDisposable
    {
        private Process lolProcess;
        private IntPtr processHandle;

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);

        private const int PROCESS_VM_READ = 0x0010;

        public MemoryReader()
        {
            try
            {
                lolProcess = Process.GetProcessesByName("League of Legends")[0];
                processHandle = OpenProcess(PROCESS_VM_READ, false, lolProcess.Id);
            }
            catch
            {
                throw new Exception("Não foi possível encontrar o processo do League of Legends.");
            }
        }

        public T ReadMemory<T>(IntPtr baseAddress, int[]? offsets = null) where T : struct
        {
            IntPtr address = baseAddress;
            if (offsets != null && offsets.Length > 0)
            {
                byte[] buffer = new byte[IntPtr.Size];
                int bytesReadOuter;
                for (int i = 0; i < offsets.Length - 1; i++)
                {
                    if (!ReadProcessMemory(processHandle, address + offsets[i], buffer, buffer.Length, out bytesReadOuter))
                        throw new Exception("Falha ao ler memória intermediária.");
                    address = (IntPtr)BitConverter.ToInt32(buffer, 0);
                }
                address += offsets[offsets.Length - 1];
            }

            byte[] data = new byte[Marshal.SizeOf<T>()];
            if (!ReadProcessMemory(processHandle, address, data, data.Length, out int bytesRead))
                throw new Exception("Falha ao ler memória final.");

            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                object? result = Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
                return result != null ? (T)result : throw new Exception("Falha ao converter dados da memória.");
            }
            finally
            {
                handle.Free();
            }
        }

        public byte[] ReadMemoryBytes(IntPtr baseAddress, int size, int[]? offsets = null)
        {
            IntPtr address = baseAddress;
            if (offsets != null && offsets.Length > 0)
            {
                byte[] buffer = new byte[IntPtr.Size];
                int bytesReadOuter;
                for (int i = 0; i < offsets.Length - 1; i++)
                {
                    if (!ReadProcessMemory(processHandle, address + offsets[i], buffer, buffer.Length, out bytesReadOuter))
                        throw new Exception("Falha ao ler memória intermediária.");
                    address = (IntPtr)BitConverter.ToInt32(buffer, 0);
                }
                address += offsets[offsets.Length - 1];
            }

            byte[] data = new byte[size];
            if (!ReadProcessMemory(processHandle, address, data, size, out int bytesRead))
                throw new Exception("Falha ao ler memória final.");
            return data;
        }

        public void Dispose()
        {
            CloseHandle(processHandle);
        }
    }
}