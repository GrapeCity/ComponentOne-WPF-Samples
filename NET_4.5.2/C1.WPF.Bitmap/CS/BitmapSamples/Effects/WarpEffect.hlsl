
cbuffer Direct2DTransforms : register(b0)
{
	float2x1 sceneToOutputX;
	float2x1 sceneToOutputY;
	float2x1 sceneToInput0X;
	float2x1 sceneToInput0Y;
};

cbuffer constants : register(b1)
{
	float2 size;
	float2 startPos;
	float2 endPos;
	float affectDist;
};

struct VSOut
{
	float4 clipSpaceOutput  : SV_POSITION;
	float4 sceneSpaceOutput : SCENE_POSITION;
	float4 texelSpaceInput0 : TEXCOORD0;
};

VSOut main(float2 meshPosition : MESH_POSITION)
{
    VSOut output;

	float2 pos = size * meshPosition;
	if (distance(startPos, pos) < affectDist)
	{
		if (distance(endPos, pos) < 0.5)
		{
			pos = startPos;
		}
		else
		{
			float a = pos.x - endPos.x;
			float b = endPos.x;
			float c = startPos.x;
			float d = pos.y - endPos.y;
			float e = endPos.y;
			float f = startPos.y;
			float g = affectDist * affectDist;

			g = sqrt(d * (2 * a * (b - c) * (e - f) - d * (b * b - 2 * b * c + c * c - g)) - a * a * (e * e - 2 * e * f + f * f - g));
			e = a * (c - b) + d * (f - e);
			f = (g + e > 0 ? g + e : g - e) / (a * a + d * d);

			g = -endPos.x / a;
			f = (g > 0.0f && g < f) ? g : f;
			g = -endPos.y / d;
			f = (g > 0.0f && g < f) ? g : f;
			g = (size.x - endPos.x) / a;
			f = (g > 0.0f && g < f) ? g : f;
			g = (size.y - endPos.y) / d;
			f = (g > 0.0f && g < f) ? g : f;

			pos.x = startPos.x + (endPos.x - startPos.x + (pos.x - endPos.x) * f) / f;
			pos.y = startPos.y + (endPos.y - startPos.y + (pos.y - endPos.y) * f) / f;
		}
	}

	output.sceneSpaceOutput.xy = pos.xy;
	output.sceneSpaceOutput.z = 0;
	output.sceneSpaceOutput.w = 1;

	output.clipSpaceOutput.x = output.sceneSpaceOutput.x * sceneToOutputX[0] + sceneToOutputX[1];
	output.clipSpaceOutput.y = output.sceneSpaceOutput.y * sceneToOutputY[0] + sceneToOutputY[1];
	output.clipSpaceOutput.z = 0;
	output.clipSpaceOutput.w = 1;

	output.texelSpaceInput0.x = size.x * meshPosition.x * sceneToInput0X[0] + sceneToInput0X[1];
	output.texelSpaceInput0.y = size.y * meshPosition.y * sceneToInput0Y[0] + sceneToInput0Y[1];
	output.texelSpaceInput0.z = sceneToInput0X[0];
	output.texelSpaceInput0.w = sceneToInput0Y[0];

    return output;
}
