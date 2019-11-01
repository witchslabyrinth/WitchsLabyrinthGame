Shader "Custom/Masking/DoubleMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

		_MaskOnePosition("First Mask Position", Vector) = (0.0, 0.0, 0.0, 0.0)
		_MaskTwoPosition("Second Mask Position", Vector) = (0.0, 0.0, 0.0, 0.0)
		_MasksRadius("Masks' Radius", Float) = 0.0

		_NoiseFrequency("Noise Frequency", Range(0 , 1)) = 0.0
		_NoiseAmplitude("Noise Amplitude", Range(0 , 10)) = 0.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
			#include "SimplexNoise3D.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				float3 worldPos : TEXTCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			float3 _MaskOnePosition;
			float3 _MaskTwoPosition;
			float _MasksRadius;

			float _NoiseFrequency;
			float _NoiseAmplitude;

			float GetVertDistance(float3 vertPosition, float3 maskPosition)
			{
				return distance(vertPosition, maskPosition);
			}

            v2f vert (appdata v)
            {
                v2f o;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float distanceFromMaskOne = GetVertDistance(i.worldPos, _MaskOnePosition);
				float distanceFromMaskTwo = GetVertDistance(i.worldPos, _MaskTwoPosition);
				float noise = snoise(i.worldPos * _NoiseFrequency);
				noise *= _NoiseAmplitude;
				if ((distanceFromMaskOne > _MasksRadius + noise) && (distanceFromMaskTwo > _MasksRadius + noise))
					discard;

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
