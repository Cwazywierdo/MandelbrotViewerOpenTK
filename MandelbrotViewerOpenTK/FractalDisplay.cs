using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandelbrotViewerOpenTK
{
    class FractalDisplay
    {
        private static readonly float[] vertices =
        {
            -1f, -1f, 0f,
            1f, -1f, 0f,
            1f, 1f, 0f,
            -1f, 1f, 0f,
        };

        private static Shader shader;
        private static int transformationMatrixLocation;

        private static int vertexBufferObject;
        private static int vertexArrayObject;

        public static void OnLoad()
        {
            shader = new Shader(@"shaders/FractalDisplay.vert", @"shaders/FractalDisplay.frag");

            transformationMatrixLocation = shader.GetUniformLocation("transformationMatrix");

            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            int positionIndex = shader.GetAttribLocation("position");
            GL.VertexAttribPointer(positionIndex, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(positionIndex);
        }

        public static void OnUnload()
        {
            shader.Dispose();
            GL.DeleteBuffer(vertexBufferObject);
            GL.DeleteVertexArray(vertexArrayObject);
        }

        public void Draw(Matrix4 transformationMatrix)
        {
            shader.Use();
            GL.UniformMatrix4(transformationMatrixLocation, true, ref transformationMatrix);

            GL.BindVertexArray(vertexArrayObject);

            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);
        }
    }
}
