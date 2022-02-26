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
        private double[] vertices = Array.Empty<double>();

        private static Shader shader;
        private static int transformationMatrixLocation;

        private static int vertexBufferObject;
        private static int vertexArrayObject;

        public readonly FractalDisplay fractal;
        public Vector2d TracebackPoint { get; private set; }
        private int maxIterations;
        private double divergenceThreshold;
        private int iterations = -1;

        public FractalTraceback(FractalDisplay fractal)
        {
            this.fractal = fractal;
            maxIterations = fractal.maxIterations;
            divergenceThreshold = fractal.divergenceThreshold;
        }

        public static void OnLoad()
        {
            shader = new Shader(@"shaders/FractalTraceback.vert", @"shaders/FractalTraceback.frag");

            transformationMatrixLocation = shader.GetUniformLocation("transformationMatrix");

            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            int positionIndex = shader.GetAttribLocation("position");
            GL.VertexAttribLPointer(positionIndex, 3, VertexAttribDoubleType.Double, 3 * sizeof(double), (IntPtr)0);
            GL.EnableVertexAttribArray(positionIndex);
        }

        public static void OnUnload()
        {
            shader.Dispose();
            GL.DeleteBuffer(vertexBufferObject);
            GL.DeleteVertexArray(vertexArrayObject);
        }

        public void Update()
        {
            if (maxIterations != fractal.maxIterations ||
                divergenceThreshold != fractal.divergenceThreshold)
                SetTracebackPoint(TracebackPoint);
        }
        public void Draw(Matrix4d transformationMatrix)
        {
            shader.Use();
            GL.UniformMatrix4(transformationMatrixLocation, true, ref transformationMatrix);
            GL.BindVertexArray(vertexArrayObject);

            GL.DrawArrays(PrimitiveType.LineStrip, 0, iterations+1);
        }

        public void SetTracebackPoint(Vector2d point)
        {
            if (!(point != TracebackPoint || 
                maxIterations != fractal.maxIterations ||
                divergenceThreshold != fractal.divergenceThreshold))
                return;
            TracebackPoint = point;
            maxIterations = fractal.maxIterations;
            divergenceThreshold = fractal.divergenceThreshold;

            vertices = new double[maxIterations * 3];

            Vector2d z = Vector2d.Zero;
            for (iterations = 0; iterations < maxIterations; iterations++)
            {
                fractal.IteratePoint(ref z, point);
                vertices[iterations * 3] = z.X;
                vertices[iterations * 3 + 1] = z.Y;
                if (z.LengthSquared > divergenceThreshold * divergenceThreshold)
                    break;
            }
            iterations = Math.Min(iterations, maxIterations-1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, 3 * (iterations+1) * sizeof(double), vertices, BufferUsageHint.DynamicDraw);
        }
    }
}
