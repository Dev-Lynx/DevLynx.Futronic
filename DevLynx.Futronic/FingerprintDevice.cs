using DevLynx.Futronic.Extensions;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DevLynx.Futronic
{
    public struct LightState
    {
        public bool Green;
        public bool Red;
    }

    public class FingerprintDevice : IDisposable
    {
        const int CHECK_INTERVAL = 100;
        const int FINGER_CONTRAST_THRESHOLD = 800;

        private readonly IntPtr _handle;
        private readonly int _port;
        private readonly string _serial;

        private readonly Timer _timer;
        private bool _isDisposed;
        private int _tick;

        private bool _hasDimensions;
        internal FutronicDimensions _dim;

        public int FrameWidth => _dim.nWidth;
        public int FrameHeight => _dim.nHeight;
        public string SerialNumber => _serial;

        private Memory<byte> _buffer;
        private IMemoryOwner<byte> _memHandle;

        public LightState Light { get; }
        public bool FingerActive { get; private set; }

        public event EventHandler FingerPresent;
        public event EventHandler FingerAbsent;

        internal FingerprintDevice(IntPtr handle, int port, string serialNumber)
        {
            _handle = handle;
            _port = port;
            _serial = serialNumber;

            _timer = new Timer(HandleTick, null, -1, -1);
            EnsureDimensions();
        }

        public void StartCapture()
        {
            if (_isDisposed) return;

            _timer.Change(0, CHECK_INTERVAL);
        }

        public void StopCapture()
        {
            if (_isDisposed) return;

            _timer.Change(-1, -1);
        }

        void EnsureDimensions()
        {
            if (_hasDimensions) return;
            _hasDimensions = FutronicAPI.ftrScanGetImageSize(_handle, out _dim);

            if (!_hasDimensions)
                return;

            _memHandle = MemoryPool<byte>.Shared.Rent(_dim.nImageSize);
            _buffer = _memHandle.Memory;
        }

        public unsafe Span<byte> GetFrame()
        {
            EnsureDimensions();
            Span<byte> span = _buffer.Span;

            fixed (byte* segment = span)
            {
                if (!FutronicAPI.ftrScanGetImage(_handle, 4, segment))
                    return Span<byte>.Empty;
            }

            return span;
        }

        private void HandleTick(object? obj)
        {
            if (Interlocked.CompareExchange(ref _tick, 1, 0) != 0)
                return;

            try
            {
                bool fingerPresent = FutronicAPI.ftrScanIsFingerPresent(_handle, out var state);
                bool hadFinger = FingerActive;

                if (fingerPresent != hadFinger)
                {
                    FingerActive = fingerPresent;

                    if (fingerPresent)
                    {
                        FingerPresent?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        FingerAbsent?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
            catch
            {
                //Console.WriteLine(ex);
            }
            finally
            {
                Interlocked.Exchange(ref _tick, 0);
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            try
            {
                CodeExtensions.Try(() => _timer.Change(-1, -1));
                CodeExtensions.Try(() => _timer.Dispose());
                CodeExtensions.Try(() => FutronicAPI.ftrScanCloseDevice(_handle));
                CodeExtensions.Try(() =>
                {
                    _buffer = null;
                    _memHandle.Dispose();
                });

                _isDisposed = true;
            }
            catch
            {
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is not FingerprintDevice device)
                return false;

            return _handle == device._handle && _port == device._port;
        }

        public override int GetHashCode()
        {
            return CodeExtensions.GenerateHashCode(_handle, _port);
        }
    }
}
