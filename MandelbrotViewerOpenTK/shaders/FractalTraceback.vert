#version 410 core

uniform dmat4 transformationMatrix;

in dvec3 position;

void main()
{
	gl_Position = vec4(dvec4(position, 1.0) * transformationMatrix);
}