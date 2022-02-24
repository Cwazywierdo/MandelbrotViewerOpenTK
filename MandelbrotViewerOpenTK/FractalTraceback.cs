using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandelbrotViewerOpenTK
{
    class FractalTraceback
    {
        private static readonly double[] vertices =
        {
            0d, 0d, 0d,
            1d, 1d, 0d
        };

        private static Shader shader;
        private static int transformationMatrixLocation;

        private static int vertexBufferObject;
        private static int vertexArrayObject;

        public readonly FractalDisplay fractal;

        public FractalTraceback(FractalDisplay fractal)
        {
            this.fractal = fractal;
        }

        public static void OnLoad()
        {
            shader = new Shader(@"shaders/FractalTraceback.vert", @"shaders/FractalTraceback.frag");

            transformationMatrixLocation = shader.GetUniformLocation("transformationMatrix");

            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(double), vertices, BufferUsageHint.StaticDraw);

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            int positionIndex = shader.GetAttribLocation("position");
            GL.VertexAttribPointer(positionIndex, 3, VertexAttribPointerType.Double, false, 3 * sizeof(double), 0);
            GL.EnableVertexAttribArray(positionIndex);
        }

        public static void OnUnload()
        {
            shader.Dispose();
            GL.DeleteBuffer(vertexBufferObject);
            GL.DeleteVertexArray(vertexArrayObject);
        }

        public void Draw(Matrix4d transformationMatrix)
        {
            shader.Use();
            GL.UniformMatrix4(transformationMatrixLocation, true, ref transformationMatrix);
            GL.BindVertexArray(vertexArrayObject);

            GL.DrawArrays(PrimitiveType.LineStrip, 0, 2);
        }
    }
}
