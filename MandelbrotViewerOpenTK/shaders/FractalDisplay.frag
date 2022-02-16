#version 330 core

//uniform mat4 projectionMatrix;
//uniform float threshold;
//uniform int maxIterations;

out vec4 FragColor;

void main()
{
	//vec2 c = gl_FragCoord.xy * projectionMatrix;
	vec2 c = gl_FragCoord.xy / 200;
	float threshold = 3;
	int maxIterations = 100;

	float x = 0;
	float y = 0;
	int i = 0;

	while(x*x + y*y <= threshold*threshold)
	{
		float oldX = x;
		x = x*x - y*y + c.x;
		y = 2 * oldX * y + c.y;
		if(i++ == maxIterations)
		{
			FragColor = vec4(0,0,0,1);
			return;
		}
	}

	/*if(c.x*c.x + c.y*c.y < 1){
		FragColor = vec4(0,0,0,1);
	}
	else{
		FragColor = vec4(1,1,1,1);
	}*/

	
	FragColor = vec4(mod(i / 20f,1), mod(i / 20f + .4f,1), mod(i / 20f + .7f,1), 1);
}

float modI(float a,float b) {
    float m=a-floor((a+0.5)/b)*b;
    return floor(m+0.5);
}