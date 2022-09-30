# DevLynx.Futronic

![preview](./assets/preview.gif)

DevLynx.Futronic is a .NET wrapper around the Futronic Fingerprint SDK.

## Getting Started

``` C#
    FutronicManager = new FutronicDeviceManager();
    FutronicManager.DeviceReady += DeviceReady;
    FutronicManager.Start();
```

Once your Futronic Device has been initialized, You can continuously fetch an image with the following:

```C#

    FingerprintDevice device;
    WriteableBitmap bitmap = new WriteableBitmap(device.FrameWidth, device.FrameHeight,
                96, 96, PixelFormats.Bgr32, null);

    //...
    DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Background, Dispatcher);
    timer.Interval = TimeSpan.FromMilliseconds(60);
    timer.Start();

    timer.Tick += (s, e) =>
    {
        device.GetImage(bitmap);
    };
```

More examples are included in the [sample folder](./samples/).