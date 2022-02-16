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
        public Vector2 Translation { get; set; }
        public float zoom = 1f;

        private readonly NativeWindow nativeWindow;

        public Camera(NativeWindow nativeWindow)
        {
            this.nativeWindow = nativeWindow;
        }

        public Matrix4 GetTransformation()
        {
            Vector3 translationV3 = new Vector3(Translation.X, Translation.Y, 0);
            Vector2i clientSize = nativeWindow.ClientSize;
            Vector3 aspectScale = new Vector3((float)clientSize.X / clientSize.Y, 1, 1);
            return Matrix4.CreateTranslation(new Vector3(-clientSize.X/2, -clientSize.Y/2, 0)) *
                Matrix4.CreateScale(zoom) *
                Matrix4.CreateScale(aspectScale) *
                Matrix4.CreateOrthographic(clientSize.X, clientSize.Y, -1, 1) *
                Matrix4.CreateTranslation(translationV3);
        }
    }
}
