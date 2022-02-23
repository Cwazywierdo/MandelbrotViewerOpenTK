﻿using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
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
        private Camera camera;
        private FractalDisplay fractalDisplay;

        private const double scrollSpeed = 1f;
        private const double kbZoomFactor = 1f;

        public MainWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            camera = new Camera(this);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            FractalDisplay.OnLoad();

            fractalDisplay = new FractalDisplay();
        }
        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            FractalDisplay.OnUnload();

            base.OnUnload();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState kbState = KeyboardState;
            MouseState mState = MouseState;
            Matrix4d transformationMatrix = camera.GetTransformation();

            // Position.Y is subtracted from ClientSize.Y because while OpenTK uses Y-up world coordinates,
            // the screen coordinate system uses Y-down.
            Vector3 mousePos = new Vector3(mState.Position.X, ClientSize.Y - mState.Position.Y, 0);
            Vector3d mouseWorldPosV3d = Vector3d.TransformPosition(mousePos, transformationMatrix);
            Vector2d mouseWorldPos = new Vector2d(mouseWorldPosV3d.X, mouseWorldPosV3d.Y);

            if (mState.IsButtonDown(MouseButton.Left))
            {
                Vector3d mouseDelta = new Vector3d(mState.Position - mState.PreviousPosition);
                Vector3d.TransformVector(in mouseDelta, in transformationMatrix, out mouseDelta);
                // negative y because screen coordinates are Y-down
                Vector2d mouseWorldDeltaV2d = new Vector2d(mouseDelta.X, -mouseDelta.Y);
                camera.Translation += -mouseWorldDeltaV2d;
            }

            if (kbState.IsKeyDown(Keys.Up))
                camera.Translation += Vector2d.UnitY * camera.zoom * scrollSpeed * e.Time;
            if (kbState.IsKeyDown(Keys.Down))
                camera.Translation += -Vector2d.UnitY * camera.zoom * scrollSpeed * e.Time;
            if (kbState.IsKeyDown(Keys.Right))
                camera.Translation += Vector2d.UnitX * camera.zoom * scrollSpeed * e.Time;
            if (kbState.IsKeyDown(Keys.Left))
                camera.Translation += -Vector2d.UnitX * camera.zoom * scrollSpeed * e.Time;

            if (kbState.IsKeyDown(Keys.Equal))
                camera.zoom /= 1 + (kbZoomFactor * e.Time);
            if (kbState.IsKeyDown(Keys.Minus))
                camera.zoom *= 1 + (kbZoomFactor * e.Time);

            if (kbState.IsKeyDown(Keys.Period))
                fractalDisplay.maxIterations++;
            if (kbState.IsKeyDown(Keys.Comma))
                fractalDisplay.maxIterations--;

            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            Matrix4d transformationMatrix = camera.GetTransformation();

            fractalDisplay.Draw(transformationMatrix);

            Context.SwapBuffers();

            base.OnRenderFrame(e);
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
