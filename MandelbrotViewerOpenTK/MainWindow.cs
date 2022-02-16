using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MandelbrotViewerOpenTK
{
    class MainWindow : GameWindow
    {
        FractalDisplay fractalDisplay;

        public MainWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {

        }

        protected override void OnLoad()
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            FractalDisplay.OnLoad();

            fractalDisplay = new FractalDisplay();

            base.OnLoad();
        }
        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            FractalDisplay.OnUnload();

            base.OnUnload();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            fractalDisplay.Draw();

            Context.SwapBuffers();

            base.OnRenderFrame(args);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            //Might remove later if it gets too expensive
            OnRenderFrame(new FrameEventArgs());
            base.OnResize(e);
        }
    }
}
