using DevLynx.Futronic.Extensions;
using System.Text;
using static DevLynx.Futronic.FutronicAPI;

namespace DevLynx.Futronic
{
    public partial class FutronicDeviceManager
    {
        const int CHECK_INTERVAL = 3000;

        private readonly Timer _timer;
        private readonly Dictionary<int, FingerprintDevice> _devices;
        private int _tick;
        private bool _isRunning = false;

        public IReadOnlyDictionary<int, FingerprintDevice> Devices => _devices;
        
        public event EventHandler<FingerprintDevice> DeviceReady;
        public event EventHandler<FingerprintDevice> DeviceDisconnected;

        static FutronicDeviceManager()
        {
        }

        public FutronicDeviceManager()
        {
            EnsureInit();

            _devices = new Dictionary<int, FingerprintDevice>(FTR_MAX_INTERFACE_NUMBER);
            _timer = new Timer(HandleTick, null, -1, -1);
        }

        unsafe void InspectInterfaces()
        {
            Span<int> interfaces = stackalloc int[FTR_MAX_INTERFACE_NUMBER];
            
            fixed (int* segment = interfaces)
            {
                bool success = ftrScanGetInterfaces(segment);
                if (!success) return;

                bool known, connected;
                for (int i = 0; i < interfaces.Length; i++)
                {
                    known = _devices.ContainsKey(i);
                    connected = segment[i] == 0;

                    if (!known && connected)
                        RegisterDevice(i);
                    else if (known && !connected)
                        DestroyDevice(i);
                }
            }
        }

        private void RegisterDevice(int port)
        {
            IntPtr handle = ftrScanOpenDeviceOnInterface(port);
            if (handle == IntPtr.Zero)
                return;

            string serial = string.Empty;

            unsafe
            {
                Span<byte> sn = stackalloc byte[8];
                fixed (byte* segment = sn)
                {
                    bool success = ftrScanGetSerialNumber(handle, segment);
                    serial = Encoding.ASCII.GetString(segment, sn.Length);
                }
            }

            FingerprintDevice device = new FingerprintDevice(handle, port, serial);
            _devices[port] = device;

            DeviceReady?.Invoke(this, device);
        }

        private void DestroyDevice(int port)
        {
            try
            {
                if (!_devices.Remove(port, out FingerprintDevice device))
                    return;

                CodeExtensions.Try(() => device.Dispose());
                DeviceDisconnected?.Invoke(this, device);
            }
            catch
            {
            }
        }

        private void HandleTick(object? state)
        {
            if (Interlocked.CompareExchange(ref _tick, 1, 0) != 0)
                return;

            try
            {
                //int lastErr = ftrScanGetLastError();
                //Console.WriteLine("Last error is: {0}", lastErr);

                InspectInterfaces();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Interlocked.Exchange(ref _tick, 0);
            }
        }

        public void Start()
        {
            if (_isRunning) return;
            
            _timer.Change(0, CHECK_INTERVAL);
            _isRunning = true;
        }

        public void Stop()
        {
            if (!_isRunning) return;

            _timer.Change(-1, -1);
            _isRunning = false;

            foreach (var pair in _devices)
                DestroyDevice(pair.Key);
        }
    }
}
