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

        public Matrix4d GetTransformation()
        {
            Vector3d translationV3 = new Vector3d(Translation.X, Translation.Y, 0);
            Vector2i clientSize = nativeWindow.ClientSize;
            Vector3d aspectScale = new Vector3d((double)clientSize.X / clientSize.Y, 1, 1);
            return Matrix4d.CreateTranslation(new Vector3(-clientSize.X/2, -clientSize.Y/2, 0)) *
                Matrix4d.Scale(zoom) *
                Matrix4d.Scale(aspectScale) *
                Matrix4d.CreateOrthographic(clientSize.X, clientSize.Y, -1, 1) *
                Matrix4d.CreateTranslation(translationV3);
        }
    }
}
