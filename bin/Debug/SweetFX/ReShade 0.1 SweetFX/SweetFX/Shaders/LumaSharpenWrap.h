

float3 LumaSharpenWrap(float4 position : SV_Position, float2 texcoord : TEXCOORD0) : SV_Target
{
	return LumaSharpenPass(texcoord);
}