using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;
using System.Text;

namespace MandelbrotViewerOpenTK
{
    class Shader : IDisposable
    {
        private readonly int Handle;
        private bool disposedValue = false;

        public Shader(string vertexPath, string fragmentPath)
        {
            Handle = GL.CreateProgram();

            int vertexShader = ProcessShader(vertexPath, ShaderType.VertexShader);
            int fragmentShader = ProcessShader(fragmentPath, ShaderType.FragmentShader);

            GL.LinkProgram(Handle);

            GL.DetachShader(Handle, vertexShader);
            GL.DeleteShader(vertexShader);

            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
        }
        private int ProcessShader(string path, ShaderType shaderType)
        {
            string shaderSource;
            using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
            {
                shaderSource = reader.ReadToEnd();
            }

            int shader = GL.CreateShader(shaderType);

            GL.ShaderSource(shader, shaderSource);
            GL.CompileShader(shader);

            string infoLogVert = GL.GetShaderInfoLog(shader);
            if (infoLogVert != string.Empty)
                Console.WriteLine(infoLogVert);

            GL.AttachShader(Handle, shader);

            return shader;
        }

        ~Shader() => GL.DeleteProgram(Handle);
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Use() => GL.UseProgram(Handle);

        public int GetAttribLocation(string name) => GL.GetAttribLocation(Handle, name);
        public int GetUniformLocation(string name) => GL.GetUniformLocation(Handle, name);

    }
}
