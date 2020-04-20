// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "IL3DN/CardComplex"
{
    Properties
    {
		_Bark("Bark", Color) = (0,0,0,0)
		_Leaves("Leaves", Color) = (1,1,1,1)
		_AlphaCutoff("Alpha Cutoff", Range( 0 , 1)) = 0.5
		_MainTex("MainTex", 2D) = "white" {}
		[Toggle(_WIND_ON)] _Wind("Wind", Float) = 1
		_WindStrenght("Wind Strenght", Range( 0 , 1)) = 0.5
		[Toggle(_ISDEAD_ON)] _IsDead("Is Dead", Float) = 0
    }

    SubShader
    {
		

        Tags { "RenderPipeline"="LightweightPipeline" "RenderType"="TransparentCutout" "Queue"="AlphaTest" }
        Cull Off
		HLSLINCLUDE
		#pragma target 3.0
		ENDHLSL

		
        Pass
        {
            Tags { "LightMode"="LightweightForward" }
            Name "Base"

            Blend One Zero
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			

            HLSLPROGRAM
            #pragma multi_compile _ LOD_FADE_CROSSFADE
            #define ASE_SRP_VERSION 60900
            #define _AlphaClip 1

            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            // -------------------------------------
            // Lightweight Pipeline keywords
            #pragma shader_feature _SAMPLE_GI

            // -------------------------------------
            // Unity defined keywords
			#ifdef ASE_FOG
            #pragma multi_compile_fog
			#endif
            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            
            #pragma vertex vert
            #pragma fragment frag


            // Lighting include is needed because of GI
            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/Shaders/UnlitInput.hlsl"

            #pragma multi_compile __ _WIND_ON
            #pragma shader_feature _ISDEAD_ON


			float3 WindDirection;
			sampler2D NoiseTextureFloat;
			float WindSpeedFloat;
			float WindTurbulenceFloat;
			float WindStrenghtFloat;
			sampler2D _MainTex;
			CBUFFER_START( UnityPerMaterial )
			float _WindStrenght;
			float4 _Bark;
			float4 _Leaves;
			float _AlphaCutoff;
			CBUFFER_END

            struct GraphVertexInput
            {
                float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct GraphVertexOutput
            {
                float4 position : POSITION;
				#ifdef ASE_FOG
				float fogCoord : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

			
            GraphVertexOutput vert (GraphVertexInput v)
            {
                GraphVertexOutput o = (GraphVertexOutput)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				float3 temp_output_918_0 = float3( (WindDirection).xz ,  0.0 );
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float2 panner924 = ( 1.0 * _Time.y * ( temp_output_918_0 * WindSpeedFloat * 10.0 ).xy + (ase_worldPos).xy);
				float4 worldNoise908 = ( tex2Dlod( NoiseTextureFloat, float4( ( ( panner924 * WindTurbulenceFloat ) / float2( 10,10 ) ), 0, 0.0) ) * _WindStrenght * WindStrenghtFloat );
				float4 transform886 = mul(GetWorldToObjectMatrix(),( float4( WindDirection , 0.0 ) * ( ( v.ase_color.a * worldNoise908 ) + ( worldNoise908 * v.ase_color.g ) ) ));
				#ifdef _WIND_ON
				float4 staticSwitch897 = transform886;
				#else
				float4 staticSwitch897 = float4( 0,0,0,0 );
				#endif
				
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = v.vertex.xyz;
				#else
				float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = staticSwitch897.xyz;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue; 
				#else
				v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal =  v.ase_normal ;
                o.position = TransformObjectToHClip(v.vertex.xyz);
				#ifdef ASE_FOG
				o.fogCoord = ComputeFogFactor( o.position.z );
				#endif
                return o;
            }

            half4 frag (GraphVertexOutput IN ) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
				float2 uv0906 = IN.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 uv0905 = IN.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,-0.5 );
				#ifdef _ISDEAD_ON
				float2 staticSwitch903 = uv0905;
				#else
				float2 staticSwitch903 = uv0906;
				#endif
				float4 tex2DNode97 = tex2D( _MainTex, staticSwitch903 );
				float4 lerpResult901 = lerp( _Bark , _Leaves , tex2DNode97.g);
				
		        float3 Color = lerpResult901.rgb;
		        float Alpha = tex2DNode97.a;
		        float AlphaClipThreshold = _AlphaCutoff;
			
			#if _AlphaClip
				clip(Alpha - AlphaClipThreshold);
			#endif

			#ifdef ASE_FOG
				Color = MixFog( Color, IN.fogCoord );
			#endif

			#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition (IN.clipPos.xyz, unity_LODFade.x);
			#endif

                return half4(Color, Alpha);
            }
            ENDHLSL
        }

		
        Pass
        {
			
            Name "ShadowCaster"
            Tags { "LightMode"="ShadowCaster" }
			ZWrite On
			ColorMask 0

            HLSLPROGRAM
            #pragma multi_compile _ LOD_FADE_CROSSFADE
            #define ASE_SRP_VERSION 60900
            #define _AlphaClip 1

            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            
            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment


            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

            #pragma multi_compile __ _WIND_ON
            #pragma shader_feature _ISDEAD_ON


			float3 WindDirection;
			sampler2D NoiseTextureFloat;
			float WindSpeedFloat;
			float WindTurbulenceFloat;
			float WindStrenghtFloat;
			sampler2D _MainTex;
			CBUFFER_START( UnityPerMaterial )
			float _WindStrenght;
			float4 _Bark;
			float4 _Leaves;
			float _AlphaCutoff;
			CBUFFER_END

            struct GraphVertexInput
            {
                float4 vertex : POSITION;
                float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct VertexOutput
            {
                float4 clipPos : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            // x: global clip space bias, y: normal world space bias
            float3 _LightDirection;

			
            VertexOutput ShadowPassVertex(GraphVertexInput v )
            {
                VertexOutput o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                
				float3 temp_output_918_0 = float3( (WindDirection).xz ,  0.0 );
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float2 panner924 = ( 1.0 * _Time.y * ( temp_output_918_0 * WindSpeedFloat * 10.0 ).xy + (ase_worldPos).xy);
				float4 worldNoise908 = ( tex2Dlod( NoiseTextureFloat, float4( ( ( panner924 * WindTurbulenceFloat ) / float2( 10,10 ) ), 0, 0.0) ) * _WindStrenght * WindStrenghtFloat );
				float4 transform886 = mul(GetWorldToObjectMatrix(),( float4( WindDirection , 0.0 ) * ( ( v.ase_color.a * worldNoise908 ) + ( worldNoise908 * v.ase_color.g ) ) ));
				#ifdef _WIND_ON
				float4 staticSwitch897 = transform886;
				#else
				float4 staticSwitch897 = float4( 0,0,0,0 );
				#endif
				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = v.vertex.xyz;
				#else
				float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = staticSwitch897.xyz;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal =  v.ase_normal ;

                float3 positionWS = TransformObjectToWorld(v.vertex.xyz);
                float3 normalWS = TransformObjectToWorldDir(v.ase_normal.xyz);

                float invNdotL = 1.0 - saturate(dot(_LightDirection, normalWS));
                float scale = invNdotL * _ShadowBias.y;

                // normal bias is negative since we want to apply an inset normal offset
                positionWS = _LightDirection * _ShadowBias.xxx + positionWS;
				positionWS = normalWS * scale.xxx + positionWS;
                float4 clipPos = TransformWorldToHClip(positionWS);

                // _ShadowBias.x sign depens on if platform has reversed z buffer
                //clipPos.z += _ShadowBias.x; 

            #if UNITY_REVERSED_Z
                clipPos.z = min(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
            #else
                clipPos.z = max(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
            #endif
                o.clipPos = clipPos;

                return o;
            }

            half4 ShadowPassFragment(VertexOutput IN  ) : SV_TARGET
            {
                UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
        		float2 uv0906 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
        		float2 uv0905 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,-0.5 );
        		#ifdef _ISDEAD_ON
        		float2 staticSwitch903 = uv0905;
        		#else
        		float2 staticSwitch903 = uv0906;
        		#endif
        		float4 tex2DNode97 = tex2D( _MainTex, staticSwitch903 );
        		

				float Alpha = tex2DNode97.a;
				float AlphaClipThreshold = _AlphaCutoff;
			#if _AlphaClip
        		clip(Alpha - AlphaClipThreshold);
			#endif

			#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition (IN.clipPos.xyz, unity_LODFade.x);
			#endif
                return 0;
            }

            ENDHLSL
        }

		
        Pass
        {
			
            Name "DepthOnly"
            Tags { "LightMode"="DepthOnly" }

            ZWrite On
			ZTest LEqual
			ColorMask 0

            HLSLPROGRAM
            #pragma multi_compile _ LOD_FADE_CROSSFADE
            #define ASE_SRP_VERSION 60900
            #define _AlphaClip 1

            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #pragma vertex vert
            #pragma fragment frag


            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

            #pragma multi_compile __ _WIND_ON
            #pragma shader_feature _ISDEAD_ON


			float3 WindDirection;
			sampler2D NoiseTextureFloat;
			float WindSpeedFloat;
			float WindTurbulenceFloat;
			float WindStrenghtFloat;
			sampler2D _MainTex;
			CBUFFER_START( UnityPerMaterial )
			float _WindStrenght;
			float4 _Bark;
			float4 _Leaves;
			float _AlphaCutoff;
			CBUFFER_END

			struct GraphVertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

            struct VertexOutput
            {
                float4 clipPos : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

			
			VertexOutput vert( GraphVertexInput v  )
			{
					VertexOutput o = (VertexOutput)0;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					float3 temp_output_918_0 = float3( (WindDirection).xz ,  0.0 );
					float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
					float2 panner924 = ( 1.0 * _Time.y * ( temp_output_918_0 * WindSpeedFloat * 10.0 ).xy + (ase_worldPos).xy);
					float4 worldNoise908 = ( tex2Dlod( NoiseTextureFloat, float4( ( ( panner924 * WindTurbulenceFloat ) / float2( 10,10 ) ), 0, 0.0) ) * _WindStrenght * WindStrenghtFloat );
					float4 transform886 = mul(GetWorldToObjectMatrix(),( float4( WindDirection , 0.0 ) * ( ( v.ase_color.a * worldNoise908 ) + ( worldNoise908 * v.ase_color.g ) ) ));
					#ifdef _WIND_ON
					float4 staticSwitch897 = transform886;
					#else
					float4 staticSwitch897 = float4( 0,0,0,0 );
					#endif
					
					o.ase_texcoord.xy = v.ase_texcoord.xy;
					
					//setting value to unused interpolator channels and avoid initialization warnings
					o.ase_texcoord.zw = 0;
					#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
					#else
					float3 defaultVertexValue = float3(0, 0, 0);
					#endif
					float3 vertexValue = staticSwitch897.xyz;	
					#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
					#else
					v.vertex.xyz += vertexValue;
					#endif
					v.ase_normal =  v.ase_normal ;
					o.clipPos = TransformObjectToHClip(v.vertex.xyz);
					return o;
			}

            half4 frag( VertexOutput IN  ) : SV_TARGET
            {
                UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
				float2 uv0906 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 uv0905 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,-0.5 );
				#ifdef _ISDEAD_ON
				float2 staticSwitch903 = uv0905;
				#else
				float2 staticSwitch903 = uv0906;
				#endif
				float4 tex2DNode97 = tex2D( _MainTex, staticSwitch903 );
				

				float Alpha = tex2DNode97.a;
				float AlphaClipThreshold = _AlphaCutoff;

			#if _AlphaClip
        		clip(Alpha - AlphaClipThreshold);
			#endif
                return 0;
			#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition (IN.clipPos.xyz, unity_LODFade.x);
			#endif
            }
            ENDHLSL
        }
		
    }
    Fallback "Hidden/InternalErrorShader"
	CustomEditor "ASEMaterialInspector"
	
}
/*ASEBEGIN
Version=17009
847;197;1924;1015;-1577.375;-236.329;1.957672;True;False
Node;AmplifyShaderEditor.Vector3Node;867;1274.78,1334.219;Float;False;Global;WindDirection;WindDirection;14;0;Create;True;0;0;False;0;0,0,0;-0.7071068,0,-0.7071068;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;916;1697.059,721.5222;Inherit;False;1619.491;623.7284;World Noise;15;931;930;929;928;927;926;925;924;923;922;921;920;919;918;917;World Noise;1,0,0.02020931,1;0;0
Node;AmplifyShaderEditor.SwizzleNode;917;1726.696,996.8585;Inherit;False;FLOAT2;0;2;1;2;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TransformDirectionNode;918;1934.695,996.8585;Inherit;False;World;World;True;Fast;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;920;1733.676,776.8593;Float;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;919;1893.301,1172.808;Float;False;Global;WindSpeedFloat;WindSpeedFloat;3;0;Create;False;0;0;False;0;0.5;0.4;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;921;2005.251,1259.398;Inherit;False;Constant;_Float1;Float 0;8;0;Create;True;0;0;False;0;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;922;2011.105,774.4605;Inherit;False;FLOAT2;0;1;2;2;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;923;2213.386,1115.243;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PannerNode;924;2378.114,779.5185;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;925;2283.138,956.9163;Float;False;Global;WindTurbulenceFloat;WindTurbulenceFloat;4;0;Create;False;0;0;False;0;0.5;0.1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;926;2594.437,779.4326;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;927;2743.651,780.5986;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;10,10;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;929;2788.354,1157.9;Float;False;Global;WindStrenghtFloat;WindStrenghtFloat;3;0;Create;False;0;0;False;0;0.5;0.8;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;930;2792,1051.62;Float;False;Property;_WindStrenght;Wind Strenght;6;0;Create;False;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;928;2882.857,781.3643;Inherit;True;Global;NoiseTextureFloat;NoiseTextureFloat;4;0;Create;False;0;0;False;0;None;e5055e0d246bd1047bdb28057a93753c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;931;3165.147,1032.935;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;908;3386.517,1029.588;Float;False;worldNoise;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;910;2771.767,1459.964;Inherit;False;548.2126;642.8162;Vertex Animation;5;857;854;855;853;856;;0,1,0.8708036,1;0;0
Node;AmplifyShaderEditor.VertexColorNode;856;2813.532,1915.7;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;853;2811.804,1507.523;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;909;2492.381,1748.838;Inherit;False;908;worldNoise;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;855;3027.265,1828.483;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;854;3024.265,1635.483;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;857;3188.073,1720.939;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;912;3651.867,844.188;Inherit;False;264.2837;355.7888;Offset UV;2;905;906;;0,1,0.8708036,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;872;3568.628,1342.612;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;905;3675.535,1053.795;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,-0.5;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;906;3677.693,902.3102;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;903;3984.095,971.1102;Float;False;Property;_IsDead;Is Dead;7;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;886;3753.615,1344.448;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;97;4245.857,1047.629;Inherit;True;Property;_MainTex;MainTex;3;0;Create;True;0;0;False;0;None;46fa255f11489784a86ff91e1485cca5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;897;3983.99,1315.858;Float;False;Property;_Wind;Wind;5;0;Create;True;0;0;False;0;1;1;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT4;0,0,0,0;False;0;FLOAT4;0,0,0,0;False;2;FLOAT4;0,0,0,0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;0,0,0,0;False;5;FLOAT4;0,0,0,0;False;6;FLOAT4;0,0,0,0;False;7;FLOAT4;0,0,0,0;False;8;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;902;4280.179,511.4594;Float;False;Property;_Bark;Bark;0;0;Create;True;0;0;False;0;0,0,0,0;0.5377356,0.3495415,0.1953092,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;292;4277.176,701.7771;Float;False;Property;_Leaves;Leaves;1;0;Create;True;0;0;False;0;1,1,1,1;0.0627447,0.3686269,0.2644943,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;907;4286.509,1435.701;Float;False;Property;_AlphaCutoff;Alpha Cutoff;2;0;Create;True;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;901;4593.94,645.3884;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;915;4851.913,1065.111;Float;False;False;2;ASEMaterialInspector;0;1;Hidden/Templates/LightWeightSRPUnlit;e2514bdcf5e5399499a9eb24d175b9db;True;DepthOnly;0;2;DepthOnly;0;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=LightweightPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;0;False;False;False;False;True;False;False;False;False;0;False;-1;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=DepthOnly;True;0;0;Hidden/InternalErrorShader;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;914;4851.913,1065.111;Float;False;False;2;ASEMaterialInspector;0;1;Hidden/Templates/LightWeightSRPUnlit;e2514bdcf5e5399499a9eb24d175b9db;True;ShadowCaster;0;1;ShadowCaster;0;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=LightweightPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;0;False;False;False;False;True;False;False;False;False;0;False;-1;False;True;1;False;-1;False;False;True;1;LightMode=ShadowCaster;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;913;4869.196,1066.44;Float;False;True;2;ASEMaterialInspector;0;3;IL3DN/CardComplex;e2514bdcf5e5399499a9eb24d175b9db;True;Base;0;0;Base;5;False;False;False;True;2;False;-1;False;False;False;False;False;True;3;RenderPipeline=LightweightPipeline;RenderType=TransparentCutout=RenderType;Queue=AlphaTest=Queue=0;True;2;0;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=LightweightForward;False;0;Hidden/InternalErrorShader;0;0;Standard;4;Vertex Position,InvertActionOnDeselection;1;Receive Shadows;1;Built-in Fog;0;LOD CrossFade;1;0;3;True;True;True;False;0
WireConnection;917;0;867;0
WireConnection;918;0;917;0
WireConnection;922;0;920;0
WireConnection;923;0;918;0
WireConnection;923;1;919;0
WireConnection;923;2;921;0
WireConnection;924;0;922;0
WireConnection;924;2;923;0
WireConnection;926;0;924;0
WireConnection;926;1;925;0
WireConnection;927;0;926;0
WireConnection;928;1;927;0
WireConnection;931;0;928;0
WireConnection;931;1;930;0
WireConnection;931;2;929;0
WireConnection;908;0;931;0
WireConnection;855;0;909;0
WireConnection;855;1;856;2
WireConnection;854;0;853;4
WireConnection;854;1;909;0
WireConnection;857;0;854;0
WireConnection;857;1;855;0
WireConnection;872;0;867;0
WireConnection;872;1;857;0
WireConnection;903;1;906;0
WireConnection;903;0;905;0
WireConnection;886;0;872;0
WireConnection;97;1;903;0
WireConnection;897;0;886;0
WireConnection;901;0;902;0
WireConnection;901;1;292;0
WireConnection;901;2;97;2
WireConnection;913;0;901;0
WireConnection;913;1;97;4
WireConnection;913;2;907;0
WireConnection;913;3;897;0
ASEEND*/
//CHKSM=5E2FDEEBDAE398A60F8427ABD8F2EC00A3060567