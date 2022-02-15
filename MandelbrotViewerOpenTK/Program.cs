using OpenTK.Windowing.Desktop;
using System;

namespace MandelbrotViewerOpenTK
{
    class Program
    {
        static void Main()
        {
            GameWindowSettings gameWindowSettings = new GameWindowSettings()
            {
                //RenderFrequency = 60d,
                //UpdateFrequency = 60d,
                //IsMultiThreaded = true
            };
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings()
            {
                Title = "Mandelbrot Viewer",
                StartFocused = true,
                StartVisible = true,
                IsEventDriven = true,
            };
            using (MainWindow mainWindow = new MainWindow(gameWindowSettings, nativeWindowSettings))
            {
                mainWindow.Run();
            }
        }
    }
}
