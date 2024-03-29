﻿#version 400 core

uniform dmat4 transformationMatrix;
uniform int maxIterations = 100;
uniform double divergenceThreshold;

out vec4 FragColor;

void main()
{
	dvec4 c = gl_FragCoord;
	c = c * transformationMatrix;

	double x = 0;
	double y = 0;
	int i = 0;

	while(x*x + y*y <= divergenceThreshold*divergenceThreshold)
	{
		double oldX = x;
		x = x*x - y*y + c.x;
		y = 2 * oldX * y + c.y;
		if(i++ == maxIterations)
		{
			FragColor = vec4(0, 0, 0, 1);
			return;
		}
	}
	
	FragColor = vec4(mod(i / 20.0, 1.0), mod(i / 20.0 + 0.4, 1.0), mod(i / 20.0 + 0.7, 1.0), 1.0);
}

float modI(float a,float b) {
    float m = a - floor((a + 0.5) / b) * b;
    return floor(m + 0.5);
}