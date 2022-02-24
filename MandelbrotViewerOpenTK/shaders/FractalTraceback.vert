#version 400 core

uniform dmat4 transformationMatrix;

in vec3 position;

void main()
{
	gl_Position = vec4(position * transformationMatrix, 1.0);
}