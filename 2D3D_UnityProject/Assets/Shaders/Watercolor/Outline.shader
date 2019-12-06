Pass{
	NAME "OUTLINE"

	Cull Front

	CGPROGRAM

#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fog

#include "UnityCG.cginc"

	float _Outline,_DeckMultiplier;
fixed4 _OutlineColor;

struct a2v {
	float4 vertex : POSITION;
	float3 normal : NORMAL;
};

struct v2f {
	float4 pos : SV_POSITION;
	float3 worldPos : TEXCOORD2;
	UNITY_FOG_COORDS(3)
};

v2f vert(a2v v) {
	v2f o;
	float4 pos = mul(UNITY_MATRIX_MV, v.vertex);

	float3 normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
	pos = pos + float4(normalize(normal), 0) * _Outline;
	o.pos = mul(UNITY_MATRIX_P, pos);
	o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

	UNITY_TRANSFER_FOG(o, o.pos);
	return o;
}

float4 frag(v2f i) : SV_Target{
	float3 ColorTemp = _OutlineColor.rgb;
	UNITY_APPLY_FOG(i.fogCoord, ColorTemp);
	return float4(ColorTemp, 1);
}

ENDCG
}