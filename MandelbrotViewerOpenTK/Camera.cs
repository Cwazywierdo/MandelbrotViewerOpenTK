using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandelbrotViewerOpenTK
{
    class Camera
    {
        public Vector2d Translation { get; set; }
        public double zoom = 1d;

        private readonly NativeWindow nativeWindow;

        public Camera(NativeWindow nativeWindow)
        {
            this.nativeWindow = nativeWindow;
        }

        public Matrix4d GetFromViewSpaceTransformation()
        {
            Vector2i clientSize = nativeWindow.ClientSize;

            return
                Matrix4d.CreateTranslation(-clientSize.X / 2d, -clientSize.Y / 2d, 0) *
                Matrix4d.Scale(2d * zoom / clientSize.Y, 2d * zoom / clientSize.Y, 1) *
                Matrix4d.CreateTranslation(Translation.X, Translation.Y, 0);
        }

        public Matrix4d GetFromWorldSpaceTransformation()
        {
            double aspect = (double)nativeWindow.ClientSize.X / nativeWindow.ClientSize.Y;

            return Matrix4d.CreateTranslation(-Translation.X, -Translation.Y, 0) *
                Matrix4d.Scale(1d / zoom) *
                Matrix4d.CreateOrthographicOffCenter(-aspect, aspect, -1, 1, -1, 1); ;
        }
    }
}
