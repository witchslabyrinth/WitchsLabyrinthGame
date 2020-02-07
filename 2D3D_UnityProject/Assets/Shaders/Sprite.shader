Shader "Unlit/Sprite"
{
	Properties{
		_Color("Glow Color", Color) = (0, 0, 0, 1)
		_MainTex("Texture", 2D) = "white" {}
		[Toggle]_Glow("Glow", Int) = 0
	}

	SubShader{
		Tags{
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
		}

		Blend SrcAlpha OneMinusSrcAlpha

		ZWrite off
		Cull off

		Pass{

			CGPROGRAM

			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			float4 _MainTex_ST;

			fixed4 _Color;
			int _Glow;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			v2f vert(appdata v) {
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
				return o;
			}

			fixed4 frag(v2f i) : SV_TARGET{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 glowCol = float4(_Color.rgb, col.a);

				fixed4 finalCol = _Glow == 0 ? col : lerp(col, glowCol, sin(_Time.z * 2) + 1);
				return finalCol;
			}

			ENDCG
		}
	}
}
