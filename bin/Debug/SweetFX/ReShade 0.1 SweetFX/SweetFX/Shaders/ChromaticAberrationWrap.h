float3 ChromaticAberrationWrap(float4 position : SV_Position, float2 texcoord : TEXCOORD0) : SV_Target
{
  float4 color = myTex2D(s0, texcoord);
  
	color = ChromaticAberrationPass(color,texcoord);
	
	return color.rgb;
}