#version 400 core

uniform dmat4 transformationMatrix;

in vec3 position;

void main()
{
	gl_Position = vec4(dvec4(position, 1.0) * transformationMatrix);
}