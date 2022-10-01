using System.IO;
using System.Runtime.InteropServices;

namespace DevLynx.Futronic
{
    public partial class FutronicDeviceManager
    {
        static bool _init;

        static void EnsureInit()
        {
            if (_init) return;

            const string x86 = nameof(x86);
            const string x64 = nameof(x64);

            bool is64Bit = Environment.Is64BitOperatingSystem;
            string path = Path.Combine(Directory.GetCurrentDirectory(), is64Bit ? x64 : x86, "ftrScanAPI.dll");

            if (!File.Exists(path))
                throw new DllNotFoundException($"Failed to locate frtScanAPI.dll. Excepted path was: {path}");

            IntPtr library = NativeLibrary.Load(path);
            
            if (library == IntPtr.Zero)
                throw new NotSupportedException($"Failed to load the Futronic Library. The Library may not be in the correct format.");

            _init = true;
        }
    }
}
