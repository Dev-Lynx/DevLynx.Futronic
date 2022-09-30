using System.Runtime.InteropServices;

namespace DevLynx.Futronic
{
    internal struct FutronicDimensions
    {
        public int nWidth;
        public int nHeight;
        public int nImageSize;
    }

    internal class FutronicAPI
    {
        [DllImport("ftrScanAPI.dll")]
        internal static extern IntPtr ftrScanOpenDevice();

        [DllImport("ftrScanAPI.dll")]
        internal static extern IntPtr ftrScanOpenDeviceOnInterface(int nInterface);

        [DllImport("ftrScanAPI.dll")]
        internal static extern int ftrGetBaseInterfaceNumber();

        [DllImport("ftrScanAPI.dll")]
        internal unsafe static extern bool ftrScanGetInterfaces(int* pBuffer);

        [DllImport("ftrScanAPI.dll")]
        internal static extern int ftrScanGetLastError();

        [DllImport("ftrScanAPI.dll")]
        internal unsafe static extern bool ftrScanGetSerialNumber(IntPtr handle, byte* pBuffer);

        [DllImport("ftrScanAPI.dll")]
        internal static extern void ftrScanCloseDevice(IntPtr ftrHandle);

        [DllImport("ftrScanAPI.dll")]
        internal static extern bool ftrScanIsFingerPresent(IntPtr ftrHandle, out _FRAME_STATE pFrameParameters);

        [DllImport("ftrScanAPI.dll")]
        internal static extern bool ftrScanSetDiodesStatus(IntPtr ftrHandle, byte byGreenDiodeStatus, byte byRedDiodeStatus);

        [DllImport("ftrScanAPI.dll")]
        internal static extern bool ftrScanGetDiodesStatus(IntPtr ftrHandle, out bool pbIsGreenDiodeOn, out bool pbIsRedDiodeOn);

        [DllImport("ftrScanAPI.dll")]
        internal static extern bool ftrScanGetImageSize(IntPtr ftrHandle, out FutronicDimensions frameSize);

        [DllImport("ftrScanAPI.dll")]
        internal static extern bool ftrScanGetImage(IntPtr ftrHandle, int nDose, byte[] pBuffer);

        [DllImport("ftrScanAPI.dll")]
        internal static extern bool ftrScanGetImage(IntPtr ftrHandle, int nDose, Span<byte> pBuffer);

        [DllImport("ftrScanAPI.dll")]
        internal unsafe static extern bool ftrScanGetImage(IntPtr ftrHandle, int nDose, byte* pBuffer);

        [DllImport("ftrScanAPI.dll")]
        internal unsafe static extern bool ftrScanSetOptions(IntPtr ftrHandle, _CONFIG_OPTIONS dwMask, _CONFIG_OPTIONS dwFlags);

        [DllImport("ftrScanAPI.dll")]
        internal unsafe static extern bool ftrScanGetOptions(IntPtr ftrHandle, out _CONFIG_OPTIONS lpdwFlags);

        internal const int FTR_MAX_INTERFACE_NUMBER = 128;

        internal struct _FAKE_REPLICA_PARAMETERS
        {
            public bool bCalculated;
            public int nCalculatedSum1;
            public int nCalculatedSumFuzzy;
            public int nCalculatedSumEmpty;
            public int nCalculatedSum2;
            public double dblCalculatedTremor;
            public double dblCalculatedValue;
        }

        internal struct _FRAME_STATE
        {
            public int nContrastOnDose2;
            public int nContrastOnDose4;
            public int nDose;
            public int nBrightnessOnDose1;
            public int nBrightnessOnDose2;
            public int nBrightnessOnDose3;
            public int nBrightnessOnDose4;
            public _FAKE_REPLICA_PARAMETERS FakeReplicaParams;
            public _FAKE_REPLICA_PARAMETERS Reserved;
        }

        [Flags]
        internal enum _CONFIG_OPTIONS : int
        {
            CHECK_FAKE_REPLICA = 0x1,
            DETECT_FAKE_FINGER = 0x1,
            USE_FAST_FINGER_DETECT_METHOD = 0x2,
            RECEIVE_LONG_IMAGE = 0x4,
            RECEIVE_FAKE_IMAGE = 0x8,
            SCALE_IMAGE = 0x10,
            IMPROVE_IMAGE = 0x20,
            INVERT_IMAGE = 0x40,
            PREVIEW_MODE = 0x80,
            IMAGE_FORMAT_MASK = 0x700,
            IMAGE_FORMAT_1 = 0x100,
            ELIMINATE_BACKGROUND = 0x800,
            IMPROVE_BACKGROUND = 0x1000,
            ROLL_THRESHOLD_MASK = 0x1F0000,
            ROLL_THRESHOLD_1 = 0x10000
        }
    }
}