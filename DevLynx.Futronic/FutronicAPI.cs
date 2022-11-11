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
        [DllImport("ftrScanAPI.dll", SetLastError = true)]
        internal static extern IntPtr ftrScanOpenDevice();

        [DllImport("ftrScanAPI.dll", SetLastError = true)]
        internal static extern IntPtr ftrScanOpenDeviceOnInterface(int nInterface);

        [DllImport("ftrScanAPI.dll", SetLastError = true)]
        internal static extern int ftrGetBaseInterfaceNumber();

        [DllImport("ftrScanAPI.dll", SetLastError = true)]
        internal unsafe static extern bool ftrScanGetInterfaces(int* pBuffer);

        [DllImport("ftrScanAPI", SetLastError = true)]
        internal unsafe static extern bool ftrScanGetSerialNumber(IntPtr handle, byte* pBuffer);

        [DllImport("ftrScanAPI", SetLastError = true)]
        internal static extern void ftrScanCloseDevice(IntPtr ftrHandle);

        [DllImport("ftrScanAPI", SetLastError = true)]
        internal static extern bool ftrScanIsFingerPresent(IntPtr ftrHandle, out _FRAME_STATE pFrameParameters);

        [DllImport("ftrScanAPI", SetLastError = true)]
        internal static extern bool ftrScanSetDiodesStatus(IntPtr ftrHandle, byte byGreenDiodeStatus, byte byRedDiodeStatus);

        [DllImport("ftrScanAPI", SetLastError = true)]
        internal static extern bool ftrScanGetDiodesStatus(IntPtr ftrHandle, out bool pbIsGreenDiodeOn, out bool pbIsRedDiodeOn);

        [DllImport("ftrScanAPI", SetLastError = true)]
        internal static extern bool ftrScanGetImageSize(IntPtr ftrHandle, out FutronicDimensions frameSize);

        [DllImport("ftrScanAPI", SetLastError = true)]
        internal static extern bool ftrScanGetImage(IntPtr ftrHandle, int nDose, byte[] pBuffer);

        [DllImport("ftrScanAPI", SetLastError = true)]
        internal static extern bool ftrScanGetImage(IntPtr ftrHandle, int nDose, Span<byte> pBuffer);

        [DllImport("ftrScanAPI", SetLastError = true)]
        internal unsafe static extern bool ftrScanGetImage(IntPtr ftrHandle, int nDose, byte* pBuffer);

        [DllImport("ftrScanAPI", SetLastError = true)]
        internal unsafe static extern bool ftrScanSetOptions(IntPtr ftrHandle, _CONFIG_OPTIONS dwMask, _CONFIG_OPTIONS dwFlags);

        [DllImport("ftrScanAPI", SetLastError = true)]
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

        internal enum _FUTRONIC_ERROR : int
        {
            FTR_ERROR_NOT_READY = 21,
            FTR_ERROR_WRITE_PROTECT = 19,
            FTR_ERROR_NOT_ENOUGH_MEMORY = 8,
            FTR_ERROR_NOT_SUPPORTED = 50,
            FTR_ERROR_INVALID_PARAMETER = 87,
            FTR_ERROR_CALL_NOT_IMPLEMENTED = 120,
            FTR_ERROR_NO_MORE_ITEMS = 259,
            FTR_ERROR_PORT_UNREACHABLE = 1234,
            FTR_ERROR_NO_SYSTEM_RESOURCES = 1450,
            FTR_ERROR_TIMEOUT = 1460,
            FTR_ERROR_BAD_CONFIGURATION = 1610,
            FTR_ERROR_MESSAGE_EXCEEDS_MAX_SIZE = 4336,

            FTR_ERROR_NO_ERROR = 0,
            FTR_ERROR_EMPTY_FRAME = 4306,
            FTR_ERROR_MOVABLE_FINGER = 0x0001,
            FTR_ERROR_NO_FRAME = 0x0002,
            FTR_ERROR_USER_CANCELED = 0x0003,
            FTR_ERROR_HARDWARE_INCOMPATIBLE = 0x0004,
            FTR_ERROR_FIRMWARE_INCOMPATIBLE = 0x0005,
            FTR_ERROR_INVALID_AUTHORIZATION_CODE = 0x0006,
            FTR_ERROR_ROLL_NOT_STARTED = 0x0007,
            FTR_ERROR_ROLL_PROGRESS_DATA = 0x0008,
            FTR_ERROR_ROLL_TIMEOUT = 0x0009,
            FTR_ERROR_ROLL_ABORTED = 0x000A,
            FTR_ERROR_ROLL_ALREADY_STARTED = 0x000B,
            FTR_ERROR_ROLL_PROGRESS_REMOVE_FINGER = 0x000C,
            FTR_ERROR_ROLL_PROGRESS_PUT_FINGER = 0x000D,
            FTR_ERROR_ROLL_PROGRESS_POST_PROCESSING = 0x000E,
            FTR_ERROR_FINGER_IS_PRESENT = 0x000F,
            FTR_ERROR_NULL_PARAMETER = 0x0010,
            FTR_ERROR_LIBUSB_ERROR = 0x0011,
            FTR_ERROR_VERSION_NOT_SUPPORTED = 0x0012,
            FTR_ERROR_BAD_CALLBACK_FUNCTION = 0x0013,
            FTR_ERROR_GENERAL_ENCRYPTION = 0x0014,
        }
    }
}