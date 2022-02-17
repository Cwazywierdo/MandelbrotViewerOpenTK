#version 400 core

uniform dmat4 transformationMatrix;
//uniform float threshold;
//uniform int maxIterations;

out vec4 FragColor;

void main()
{
	dvec4 c = gl_FragCoord;
	c = c * transformationMatrix;
	double threshold = 2;
	int maxIterations = 5000;

	double x = 0;
	double y = 0;
	int i = 0;

	while(x*x + y*y <= threshold*threshold)
	{
		double oldX = x;
		x = x*x - y*y + c.x;
		y = 2 * oldX * y + c.y;
		if(i++ == maxIterations)
		{
			FragColor = vec4(0,0,0,1);
			return;
		}
	}
	
	FragColor = vec4(mod(i / 20f,1), mod(i / 20f + .4f,1), mod(i / 20f + .7f,1), 1);
}

float modI(float a,float b) {
    float m=a-floor((a+0.5)/b)*b;
    return floor(m+0.5);
}