// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "IL3DN/Grass"
{
    Properties
    {
		_Color("Color", Color) = (1,1,1,1)
		_AlphaCutoff("Alpha Cutoff", Range( 0 , 1)) = 0.5
		[NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
		[Toggle(_SNOW_ON)] _Snow("Snow", Float) = 1
		[Toggle(_WIND_ON)] _Wind("Wind", Float) = 1
		_WindStrenght("Wind Strenght", Range( 0 , 1)) = 0.5
		[Toggle(_WIGGLE_ON)] _Wiggle("Wiggle", Float) = 1
		_WiggleStrenght("Wiggle Strenght", Range( 0 , 1)) = 0.5
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
            #define _SPECULAR_SETUP 1
            #define _AlphaClip 1

            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            

        	// -------------------------------------
            // Lightweight Pipeline keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
            
        	// -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_fog

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #pragma vertex vert
        	#pragma fragment frag

        	#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"
        	#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Lighting.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
        	#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/ShaderGraphFunctions.hlsl"
		
			#pragma multi_compile __ _WIND_ON
			#pragma multi_compile __ _SNOW_ON
			#pragma multi_compile __ _WIGGLE_ON


			float3 WindDirection;
			sampler2D NoiseTextureFloat;
			float WindSpeedFloat;
			float WindTurbulenceFloat;
			float WindStrenghtFloat;
			float SnowGrassFloat;
			sampler2D _MainTex;
			float GrassWiggleFloat;
			CBUFFER_START( UnityPerMaterial )
			float _WindStrenght;
			float4 _Color;
			float _WiggleStrenght;
			float _AlphaCutoff;
			CBUFFER_END

            struct GraphVertexInput
            {
                float4 vertex : POSITION;
                float3 ase_normal : NORMAL;
                float4 ase_tangent : TANGENT;
                float4 texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

        	struct GraphVertexOutput
            {
                float4 clipPos                : SV_POSITION;
                float4 lightmapUVOrVertexSH	  : TEXCOORD0;
        		half4 fogFactorAndVertexLight : TEXCOORD1; // x: fogFactor, yzw: vertex light
            	float4 shadowCoord            : TEXCOORD2;
				float4 tSpace0					: TEXCOORD3;
				float4 tSpace1					: TEXCOORD4;
				float4 tSpace2					: TEXCOORD5;
				float4 ase_color : COLOR;
				float4 ase_texcoord7 : TEXCOORD7;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            	UNITY_VERTEX_OUTPUT_STEREO
            };

			
            GraphVertexOutput vert (GraphVertexInput v  )
        	{
        		GraphVertexOutput o = (GraphVertexOutput)0;
                UNITY_SETUP_INSTANCE_ID(v);
            	UNITY_TRANSFER_INSTANCE_ID(v, o);
        		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 temp_output_1059_0 = float3( (WindDirection).xz ,  0.0 );
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float2 panner1065 = ( 1.0 * _Time.y * ( temp_output_1059_0 * WindSpeedFloat * 10.0 ).xy + (ase_worldPos).xz);
				float4 worldNoise1038 = ( tex2Dlod( NoiseTextureFloat, float4( ( ( panner1065 * WindTurbulenceFloat ) / float2( 10,10 ) ), 0, 0.0) ) * _WindStrenght * WindStrenghtFloat );
				float4 transform1029 = mul(GetWorldToObjectMatrix(),( float4( WindDirection , 0.0 ) * ( ( v.ase_color.a * worldNoise1038 ) + ( worldNoise1038 * v.ase_color.g ) ) ));
				#ifdef _WIND_ON
				float4 staticSwitch1031 = transform1029;
				#else
				float4 staticSwitch1031 = float4( 0,0,0,0 );
				#endif
				
				o.ase_color = v.ase_color;
				o.ase_texcoord7.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord7.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = v.vertex.xyz;
				#else
				float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = staticSwitch1031.xyz;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal =  v.ase_normal ;

        		// Vertex shader outputs defined by graph
                float3 lwWNormal = TransformObjectToWorldNormal(v.ase_normal);
				float3 lwWorldPos = TransformObjectToWorld(v.vertex.xyz);
				float3 lwWTangent = TransformObjectToWorldDir(v.ase_tangent.xyz);
				float3 lwWBinormal = normalize(cross(lwWNormal, lwWTangent) * v.ase_tangent.w);
				o.tSpace0 = float4(lwWTangent.x, lwWBinormal.x, lwWNormal.x, lwWorldPos.x);
				o.tSpace1 = float4(lwWTangent.y, lwWBinormal.y, lwWNormal.y, lwWorldPos.y);
				o.tSpace2 = float4(lwWTangent.z, lwWBinormal.z, lwWNormal.z, lwWorldPos.z);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
                
         		// We either sample GI from lightmap or SH.
        	    // Lightmap UV and vertex SH coefficients use the same interpolator ("float2 lightmapUV" for lightmap or "half3 vertexSH" for SH)
                // see DECLARE_LIGHTMAP_OR_SH macro.
        	    // The following funcions initialize the correct variable with correct data
        	    OUTPUT_LIGHTMAP_UV(v.texcoord1, unity_LightmapST, o.lightmapUVOrVertexSH.xy);
        	    OUTPUT_SH(lwWNormal, o.lightmapUVOrVertexSH.xyz);

        	    half3 vertexLight = VertexLighting(vertexInput.positionWS, lwWNormal);
        	    half fogFactor = ComputeFogFactor(vertexInput.positionCS.z);
        	    o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
        	    o.clipPos = vertexInput.positionCS;

        	#ifdef _MAIN_LIGHT_SHADOWS
        		o.shadowCoord = GetShadowCoord(vertexInput);
        	#endif
        		return o;
        	}

        	half4 frag (GraphVertexOutput IN  ) : SV_Target
            {
            	UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

        		float3 WorldSpaceNormal = normalize(float3(IN.tSpace0.z,IN.tSpace1.z,IN.tSpace2.z));
				float3 WorldSpaceTangent = float3(IN.tSpace0.x,IN.tSpace1.x,IN.tSpace2.x);
				float3 WorldSpaceBiTangent = float3(IN.tSpace0.y,IN.tSpace1.y,IN.tSpace2.y);
				float3 WorldSpacePosition = float3(IN.tSpace0.w,IN.tSpace1.w,IN.tSpace2.w);
				float3 WorldSpaceViewDirection = SafeNormalize( _WorldSpaceCameraPos.xyz  - WorldSpacePosition );
    
				#ifdef _SNOW_ON
				float staticSwitch1054 = ( saturate( pow( ( 1.0 - IN.ase_color.g ) , 2.0 ) ) * SnowGrassFloat );
				#else
				float staticSwitch1054 = 0.0;
				#endif
				float2 uv0746 = IN.ase_texcoord7.xy * float2( 1,1 ) + float2( 0,0 );
				float2 uv01080 = IN.ase_texcoord7.xy * float2( 1,1 ) + float2( 0,0 );
				float3 temp_output_1059_0 = float3( (WindDirection).xz ,  0.0 );
				float2 panner1065 = ( 1.0 * _Time.y * ( temp_output_1059_0 * WindSpeedFloat * 10.0 ).xy + (WorldSpacePosition).xz);
				float4 worldNoise1038 = ( tex2D( NoiseTextureFloat, ( ( panner1065 * WindTurbulenceFloat ) / float2( 10,10 ) ) ) * _WindStrenght * WindStrenghtFloat );
				float cos1081 = cos( ( ( tex2D( NoiseTextureFloat, worldNoise1038.rg ) * IN.ase_color.g ) * GrassWiggleFloat * _WiggleStrenght ).r );
				float sin1081 = sin( ( ( tex2D( NoiseTextureFloat, worldNoise1038.rg ) * IN.ase_color.g ) * GrassWiggleFloat * _WiggleStrenght ).r );
				float2 rotator1081 = mul( uv01080 - float2( 0.5,0.5 ) , float2x2( cos1081 , -sin1081 , sin1081 , cos1081 )) + float2( 0.5,0.5 );
				#ifdef _WIGGLE_ON
				float2 staticSwitch1033 = rotator1081;
				#else
				float2 staticSwitch1033 = uv0746;
				#endif
				float4 tex2DNode97 = tex2D( _MainTex, staticSwitch1033 );
				
				float3 temp_cast_5 = (0.0).xxx;
				
				
		        float3 Albedo = saturate( ( staticSwitch1054 + ( _Color * tex2DNode97 ) ) ).rgb;
				float3 Normal = float3(0, 0, 1);
				float3 Emission = 0;
				float3 Specular = temp_cast_5;
				float Metallic = 0.0;
				float Smoothness = 0.0;
				float Occlusion = 1;
				float Alpha = tex2DNode97.a;
				float AlphaClipThreshold = _AlphaCutoff;

        		InputData inputData;
        		inputData.positionWS = WorldSpacePosition;

        #ifdef _NORMALMAP
        	    inputData.normalWS = normalize(TransformTangentToWorld(Normal, half3x3(WorldSpaceTangent, WorldSpaceBiTangent, WorldSpaceNormal)));
        #else
            #if !SHADER_HINT_NICE_QUALITY
                inputData.normalWS = WorldSpaceNormal;
            #else
        	    inputData.normalWS = normalize(WorldSpaceNormal);
            #endif
        #endif

        #if !SHADER_HINT_NICE_QUALITY
        	    // viewDirection should be normalized here, but we avoid doing it as it's close enough and we save some ALU.
        	    inputData.viewDirectionWS = WorldSpaceViewDirection;
        #else
        	    inputData.viewDirectionWS = normalize(WorldSpaceViewDirection);
        #endif

        	    inputData.shadowCoord = IN.shadowCoord;

        	    inputData.fogCoord = IN.fogFactorAndVertexLight.x;
        	    inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;
        	    inputData.bakedGI = SAMPLE_GI(IN.lightmapUVOrVertexSH.xy, IN.lightmapUVOrVertexSH.xyz, inputData.normalWS);

        		half4 color = LightweightFragmentPBR(
        			inputData, 
        			Albedo, 
        			Metallic, 
        			Specular, 
        			Smoothness, 
        			Occlusion, 
        			Emission, 
        			Alpha);

			#ifdef TERRAIN_SPLAT_ADDPASS
				color.rgb = MixFogColor(color.rgb, half3( 0, 0, 0 ), IN.fogFactorAndVertexLight.x );
			#else
				color.rgb = MixFog(color.rgb, IN.fogFactorAndVertexLight.x);
			#endif

        #if _AlphaClip
        		clip(Alpha - AlphaClipThreshold);
        #endif

		#if ASE_LW_FINAL_COLOR_ALPHA_MULTIPLY
				color.rgb *= color.a;
		#endif
		
		#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition (IN.clipPos.xyz, unity_LODFade.x);
		#endif
        		return color;
            }

        	ENDHLSL
        }

		
        Pass
        {
			
        	Name "ShadowCaster"
            Tags { "LightMode"="ShadowCaster" }

			ZWrite On
			ZTest LEqual

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
            #pragma multi_compile __ _WIGGLE_ON


            struct GraphVertexInput
            {
                float4 vertex : POSITION;
                float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

			float3 WindDirection;
			sampler2D NoiseTextureFloat;
			float WindSpeedFloat;
			float WindTurbulenceFloat;
			float WindStrenghtFloat;
			sampler2D _MainTex;
			float GrassWiggleFloat;
			CBUFFER_START( UnityPerMaterial )
			float _WindStrenght;
			float4 _Color;
			float _WiggleStrenght;
			float _AlphaCutoff;
			CBUFFER_END

        	struct VertexOutput
        	{
        	    float4 clipPos      : SV_POSITION;
                float4 ase_texcoord7 : TEXCOORD7;
                float4 ase_texcoord8 : TEXCOORD8;
                float4 ase_color : COLOR;
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
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO (o);

				float3 temp_output_1059_0 = float3( (WindDirection).xz ,  0.0 );
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float2 panner1065 = ( 1.0 * _Time.y * ( temp_output_1059_0 * WindSpeedFloat * 10.0 ).xy + (ase_worldPos).xz);
				float4 worldNoise1038 = ( tex2Dlod( NoiseTextureFloat, float4( ( ( panner1065 * WindTurbulenceFloat ) / float2( 10,10 ) ), 0, 0.0) ) * _WindStrenght * WindStrenghtFloat );
				float4 transform1029 = mul(GetWorldToObjectMatrix(),( float4( WindDirection , 0.0 ) * ( ( v.ase_color.a * worldNoise1038 ) + ( worldNoise1038 * v.ase_color.g ) ) ));
				#ifdef _WIND_ON
				float4 staticSwitch1031 = transform1029;
				#else
				float4 staticSwitch1031 = float4( 0,0,0,0 );
				#endif
				
				o.ase_texcoord8.xyz = ase_worldPos;
				
				o.ase_texcoord7.xy = v.ase_texcoord.xy;
				o.ase_color = v.ase_color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord7.zw = 0;
				o.ase_texcoord8.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = v.vertex.xyz;
				#else
				float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = staticSwitch1031.xyz;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal =  v.ase_normal ;

        	    float3 positionWS = TransformObjectToWorld(v.vertex.xyz);
                float3 normalWS = TransformObjectToWorldDir(v.ase_normal);

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

               float2 uv0746 = IN.ase_texcoord7.xy * float2( 1,1 ) + float2( 0,0 );
               float2 uv01080 = IN.ase_texcoord7.xy * float2( 1,1 ) + float2( 0,0 );
               float3 temp_output_1059_0 = float3( (WindDirection).xz ,  0.0 );
               float3 ase_worldPos = IN.ase_texcoord8.xyz;
               float2 panner1065 = ( 1.0 * _Time.y * ( temp_output_1059_0 * WindSpeedFloat * 10.0 ).xy + (ase_worldPos).xz);
               float4 worldNoise1038 = ( tex2D( NoiseTextureFloat, ( ( panner1065 * WindTurbulenceFloat ) / float2( 10,10 ) ) ) * _WindStrenght * WindStrenghtFloat );
               float cos1081 = cos( ( ( tex2D( NoiseTextureFloat, worldNoise1038.rg ) * IN.ase_color.g ) * GrassWiggleFloat * _WiggleStrenght ).r );
               float sin1081 = sin( ( ( tex2D( NoiseTextureFloat, worldNoise1038.rg ) * IN.ase_color.g ) * GrassWiggleFloat * _WiggleStrenght ).r );
               float2 rotator1081 = mul( uv01080 - float2( 0.5,0.5 ) , float2x2( cos1081 , -sin1081 , sin1081 , cos1081 )) + float2( 0.5,0.5 );
               #ifdef _WIGGLE_ON
               float2 staticSwitch1033 = rotator1081;
               #else
               float2 staticSwitch1033 = uv0746;
               #endif
               float4 tex2DNode97 = tex2D( _MainTex, staticSwitch1033 );
               

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

            #pragma vertex vert
            #pragma fragment frag


            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

            #pragma multi_compile __ _WIND_ON
            #pragma multi_compile __ _WIGGLE_ON


			float3 WindDirection;
			sampler2D NoiseTextureFloat;
			float WindSpeedFloat;
			float WindTurbulenceFloat;
			float WindStrenghtFloat;
			sampler2D _MainTex;
			float GrassWiggleFloat;
			CBUFFER_START( UnityPerMaterial )
			float _WindStrenght;
			float4 _Color;
			float _WiggleStrenght;
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
        	    float4 clipPos      : SV_POSITION;
                float4 ase_texcoord : TEXCOORD0;
                float4 ase_texcoord1 : TEXCOORD1;
                float4 ase_color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
        	};

			           

            VertexOutput vert(GraphVertexInput v  )
            {
                VertexOutput o = (VertexOutput)0;
        	    UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 temp_output_1059_0 = float3( (WindDirection).xz ,  0.0 );
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float2 panner1065 = ( 1.0 * _Time.y * ( temp_output_1059_0 * WindSpeedFloat * 10.0 ).xy + (ase_worldPos).xz);
				float4 worldNoise1038 = ( tex2Dlod( NoiseTextureFloat, float4( ( ( panner1065 * WindTurbulenceFloat ) / float2( 10,10 ) ), 0, 0.0) ) * _WindStrenght * WindStrenghtFloat );
				float4 transform1029 = mul(GetWorldToObjectMatrix(),( float4( WindDirection , 0.0 ) * ( ( v.ase_color.a * worldNoise1038 ) + ( worldNoise1038 * v.ase_color.g ) ) ));
				#ifdef _WIND_ON
				float4 staticSwitch1031 = transform1029;
				#else
				float4 staticSwitch1031 = float4( 0,0,0,0 );
				#endif
				
				o.ase_texcoord1.xyz = ase_worldPos;
				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				o.ase_color = v.ase_color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				o.ase_texcoord1.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = v.vertex.xyz;
				#else
				float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = staticSwitch1031.xyz;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal =  v.ase_normal ;

        	    o.clipPos = TransformObjectToHClip(v.vertex.xyz);
        	    return o;
            }

            half4 frag(VertexOutput IN  ) : SV_TARGET
            {
                UNITY_SETUP_INSTANCE_ID(IN);

				float2 uv0746 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 uv01080 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float3 temp_output_1059_0 = float3( (WindDirection).xz ,  0.0 );
				float3 ase_worldPos = IN.ase_texcoord1.xyz;
				float2 panner1065 = ( 1.0 * _Time.y * ( temp_output_1059_0 * WindSpeedFloat * 10.0 ).xy + (ase_worldPos).xz);
				float4 worldNoise1038 = ( tex2D( NoiseTextureFloat, ( ( panner1065 * WindTurbulenceFloat ) / float2( 10,10 ) ) ) * _WindStrenght * WindStrenghtFloat );
				float cos1081 = cos( ( ( tex2D( NoiseTextureFloat, worldNoise1038.rg ) * IN.ase_color.g ) * GrassWiggleFloat * _WiggleStrenght ).r );
				float sin1081 = sin( ( ( tex2D( NoiseTextureFloat, worldNoise1038.rg ) * IN.ase_color.g ) * GrassWiggleFloat * _WiggleStrenght ).r );
				float2 rotator1081 = mul( uv01080 - float2( 0.5,0.5 ) , float2x2( cos1081 , -sin1081 , sin1081 , cos1081 )) + float2( 0.5,0.5 );
				#ifdef _WIGGLE_ON
				float2 staticSwitch1033 = rotator1081;
				#else
				float2 staticSwitch1033 = uv0746;
				#endif
				float4 tex2DNode97 = tex2D( _MainTex, staticSwitch1033 );
				

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

        // This pass it not used during regular rendering, only for lightmap baking.
		
        Pass
        {
			
        	Name "Meta"
            Tags { "LightMode"="Meta" }

            Cull Off

            HLSLPROGRAM
            #pragma multi_compile _ LOD_FADE_CROSSFADE
            #define ASE_SRP_VERSION 60900
            #define _AlphaClip 1

            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #pragma vertex vert
            #pragma fragment frag

			
            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/MetaInput.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

            #pragma multi_compile __ _WIND_ON
            #pragma multi_compile __ _SNOW_ON
            #pragma multi_compile __ _WIGGLE_ON


			float3 WindDirection;
			sampler2D NoiseTextureFloat;
			float WindSpeedFloat;
			float WindTurbulenceFloat;
			float WindStrenghtFloat;
			float SnowGrassFloat;
			sampler2D _MainTex;
			float GrassWiggleFloat;
			CBUFFER_START( UnityPerMaterial )
			float _WindStrenght;
			float4 _Color;
			float _WiggleStrenght;
			float _AlphaCutoff;
			CBUFFER_END

            #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature EDITOR_VISUALIZATION


            struct GraphVertexInput
            {
                float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

        	struct VertexOutput
        	{
        	    float4 clipPos      : SV_POSITION;
                float4 ase_color : COLOR;
                float4 ase_texcoord : TEXCOORD0;
                float4 ase_texcoord1 : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
        	};

			
            VertexOutput vert(GraphVertexInput v  )
            {
                VertexOutput o = (VertexOutput)0;
        	    UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				float3 temp_output_1059_0 = float3( (WindDirection).xz ,  0.0 );
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float2 panner1065 = ( 1.0 * _Time.y * ( temp_output_1059_0 * WindSpeedFloat * 10.0 ).xy + (ase_worldPos).xz);
				float4 worldNoise1038 = ( tex2Dlod( NoiseTextureFloat, float4( ( ( panner1065 * WindTurbulenceFloat ) / float2( 10,10 ) ), 0, 0.0) ) * _WindStrenght * WindStrenghtFloat );
				float4 transform1029 = mul(GetWorldToObjectMatrix(),( float4( WindDirection , 0.0 ) * ( ( v.ase_color.a * worldNoise1038 ) + ( worldNoise1038 * v.ase_color.g ) ) ));
				#ifdef _WIND_ON
				float4 staticSwitch1031 = transform1029;
				#else
				float4 staticSwitch1031 = float4( 0,0,0,0 );
				#endif
				
				o.ase_texcoord1.xyz = ase_worldPos;
				
				o.ase_color = v.ase_color;
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				o.ase_texcoord1.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = v.vertex.xyz;
				#else
				float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = staticSwitch1031.xyz;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal =  v.ase_normal ;
#if !defined( ASE_SRP_VERSION ) || ASE_SRP_VERSION  > 51300				
                o.clipPos = MetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord1.xy, unity_LightmapST, unity_DynamicLightmapST);
#else
				o.clipPos = MetaVertexPosition (v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST);
#endif
        	    return o;
            }

            half4 frag(VertexOutput IN  ) : SV_TARGET
            {
                UNITY_SETUP_INSTANCE_ID(IN);

           		#ifdef _SNOW_ON
           		float staticSwitch1054 = ( saturate( pow( ( 1.0 - IN.ase_color.g ) , 2.0 ) ) * SnowGrassFloat );
           		#else
           		float staticSwitch1054 = 0.0;
           		#endif
           		float2 uv0746 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
           		float2 uv01080 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
           		float3 temp_output_1059_0 = float3( (WindDirection).xz ,  0.0 );
           		float3 ase_worldPos = IN.ase_texcoord1.xyz;
           		float2 panner1065 = ( 1.0 * _Time.y * ( temp_output_1059_0 * WindSpeedFloat * 10.0 ).xy + (ase_worldPos).xz);
           		float4 worldNoise1038 = ( tex2D( NoiseTextureFloat, ( ( panner1065 * WindTurbulenceFloat ) / float2( 10,10 ) ) ) * _WindStrenght * WindStrenghtFloat );
           		float cos1081 = cos( ( ( tex2D( NoiseTextureFloat, worldNoise1038.rg ) * IN.ase_color.g ) * GrassWiggleFloat * _WiggleStrenght ).r );
           		float sin1081 = sin( ( ( tex2D( NoiseTextureFloat, worldNoise1038.rg ) * IN.ase_color.g ) * GrassWiggleFloat * _WiggleStrenght ).r );
           		float2 rotator1081 = mul( uv01080 - float2( 0.5,0.5 ) , float2x2( cos1081 , -sin1081 , sin1081 , cos1081 )) + float2( 0.5,0.5 );
           		#ifdef _WIGGLE_ON
           		float2 staticSwitch1033 = rotator1081;
           		#else
           		float2 staticSwitch1033 = uv0746;
           		#endif
           		float4 tex2DNode97 = tex2D( _MainTex, staticSwitch1033 );
           		
				
		        float3 Albedo = saturate( ( staticSwitch1054 + ( _Color * tex2DNode97 ) ) ).rgb;
				float3 Emission = 0;
				float Alpha = tex2DNode97.a;
				float AlphaClipThreshold = _AlphaCutoff;

         #if _AlphaClip
        		clip(Alpha - AlphaClipThreshold);
        #endif

                MetaInput metaInput = (MetaInput)0;
                metaInput.Albedo = Albedo;
                metaInput.Emission = Emission;
                
                return MetaFragment(metaInput);
            }
            ENDHLSL
        }
		
    }
    Fallback "Hidden/InternalErrorShader"
	CustomEditor "ASEMaterialInspector"
	
}
/*ASEBEGIN
Version=17009
847;197;1924;1015;-477.1657;-152.959;2.365776;True;False
Node;AmplifyShaderEditor.CommentaryNode;1057;1264.994,1452.417;Inherit;False;1606.407;663.0706;World Noise;15;1072;1071;1070;1069;1068;1067;1066;1065;1064;1063;1062;1061;1060;1059;1058;World Noise;1,0,0.02020931,1;0;0
Node;AmplifyShaderEditor.Vector3Node;867;983.5457,1364.163;Float;False;Global;WindDirection;WindDirection;14;0;Create;True;0;0;False;0;0,0,0;-0.7071068,0,-0.7071068;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SwizzleNode;1058;1294.63,1727.753;Inherit;False;FLOAT2;0;2;1;2;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TransformDirectionNode;1059;1502.631,1727.753;Inherit;False;World;World;True;Fast;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;1060;1580.234,2002.023;Inherit;False;Constant;_Float1;Float 0;9;0;Create;True;0;0;False;0;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;1062;1294.24,1512.096;Float;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;1061;1459.936,1902.553;Float;False;Global;WindSpeedFloat;WindSpeedFloat;3;0;Create;False;0;0;False;0;0.5;0.4;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1064;1767.859,1886.415;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SwizzleNode;1063;1578.445,1508.249;Inherit;False;FLOAT2;0;2;2;2;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;1066;1838.408,1664.29;Float;False;Global;WindTurbulenceFloat;WindTurbulenceFloat;4;0;Create;False;0;0;False;0;0.5;0.1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;1065;1930.274,1515.888;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1067;2132.6,1521.719;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;1068;2294.918,1520.376;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;10,10;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;1071;2435.755,1890.563;Float;False;Global;WindStrenghtFloat;WindStrenghtFloat;3;0;Create;False;0;0;False;0;0.5;0.8;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1069;2430.572,1791.345;Float;False;Property;_WindStrenght;Wind Strenght;6;0;Create;False;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1070;2430.437,1515.128;Inherit;True;Global;NoiseTextureFloat;NoiseTextureFloat;3;0;Create;False;0;0;False;0;None;e5055e0d246bd1047bdb28057a93753c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1072;2733.088,1763.831;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1038;2940.231,1756.362;Float;False;worldNoise;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;1073;1865.86,736.3556;Inherit;False;1007.189;586.5881;UV Animation;8;1081;1080;1079;1078;1077;1076;1075;1074;UV Animation;0.7678117,1,0,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;1040;1651.89,800.5345;Inherit;False;1038;worldNoise;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1074;1974.696,779.9953;Inherit;True;Property;_TextureSample2;Texture Sample 0;3;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;1070;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;1075;2111.275,977.8968;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;1047;1810.946,256.1416;Inherit;False;1075.409;358.2535;Snow;6;1053;1052;1051;1050;1049;1048;Snow;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1077;2321.674,1010.482;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;1078;2014.434,1139.744;Float;False;Global;GrassWiggleFloat;GrassWiggleFloat;4;0;Create;False;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;1048;1937.884,312.6477;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;1076;2015.757,1229.172;Float;False;Property;_WiggleStrenght;Wiggle Strenght;8;0;Create;False;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;1080;2359.133,841.0495;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1079;2467.743,1011.321;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;1041;2257.943,2164.903;Inherit;False;614.128;676.7801;Vertex Animation;5;857;854;855;853;856;Vertex Animation;0,1,0.8708036,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;1049;2165.041,353.91;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;746;3286.935,776.4822;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;856;2308.056,2663.818;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;853;2312.685,2253.399;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;1039;1946.026,2520.28;Inherit;False;1038;worldNoise;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;1050;2356.345,355.4845;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;1081;2611.064,892.0168;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;854;2545.984,2342.072;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;855;2550.746,2563.964;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;1052;1842.215,498.9716;Inherit;False;Global;SnowGrassFloat;SnowGrassFloat;5;0;Create;True;0;0;False;0;1;1;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;1051;2536.197,362.6673;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;1033;3577.498,858.7823;Float;False;Property;_Wiggle;Wiggle;7;0;Create;True;0;0;False;0;1;1;1;True;_WIND_ON;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;292;3969.53,927.8489;Float;False;Property;_Color;Color;0;0;Create;True;0;0;False;0;1,1,1,1;0.3764702,0.470588,0.188235,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;857;2730.745,2447.178;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1053;2741.701,476.3654;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;97;3893.026,1123.991;Inherit;True;Property;_MainTex;MainTex;2;1;[NoScaleOffset];Create;True;0;0;False;0;None;a73c218e0d8156240a793d22710686d1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;872;3167.726,1367.195;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;293;4335.922,1060.561;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;1054;3579.471,442.7478;Inherit;False;Property;_Snow;Snow;4;0;Create;True;0;0;False;0;1;1;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;1055;4468.764,720.1292;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;1029;3341.557,1372.28;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;1031;3576.195,1345.762;Float;False;Property;_Wind;Wind;5;0;Create;True;0;0;False;0;1;1;1;True;_WIND_ON;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT4;0,0,0,0;False;0;FLOAT4;0,0,0,0;False;2;FLOAT4;0,0,0,0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;0,0,0,0;False;5;FLOAT4;0,0,0,0;False;6;FLOAT4;0,0,0,0;False;7;FLOAT4;0,0,0,0;False;8;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SaturateNode;1056;4638.426,1058.951;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;1046;4332.093,1163.248;Float;False;Constant;_Float0;Float 0;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1036;3911.207,1400.223;Float;False;Property;_AlphaCutoff;Alpha Cutoff;1;0;Create;True;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1043;4525.362,1096.846;Float;False;False;2;ASEMaterialInspector;0;2;Hidden/Templates/LightWeightSRPPBR;1976390536c6c564abb90fe41f6ee334;True;ShadowCaster;0;1;ShadowCaster;0;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=LightweightPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;0;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1042;4794.849,1068.211;Float;False;True;2;ASEMaterialInspector;0;2;IL3DN/Grass;1976390536c6c564abb90fe41f6ee334;True;Base;0;0;Base;11;False;False;False;True;2;False;-1;False;False;False;False;False;True;3;RenderPipeline=LightweightPipeline;RenderType=TransparentCutout=RenderType;Queue=AlphaTest=Queue=0;True;2;0;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=LightweightForward;False;0;Hidden/InternalErrorShader;0;0;Standard;3;Vertex Position,InvertActionOnDeselection;1;Receive Shadows;1;LOD CrossFade;1;1;_FinalColorxAlpha;0;4;True;True;True;True;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1045;4525.362,1096.846;Float;False;False;2;ASEMaterialInspector;0;2;Hidden/Templates/LightWeightSRPPBR;1976390536c6c564abb90fe41f6ee334;True;Meta;0;3;Meta;0;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=LightweightPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;0;False;False;False;True;2;False;-1;False;False;False;False;False;True;1;LightMode=Meta;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1044;4525.362,1096.846;Float;False;False;2;ASEMaterialInspector;0;2;Hidden/Templates/LightWeightSRPPBR;1976390536c6c564abb90fe41f6ee334;True;DepthOnly;0;2;DepthOnly;0;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=LightweightPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;0;False;False;False;False;True;False;False;False;False;0;False;-1;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
WireConnection;1058;0;867;0
WireConnection;1059;0;1058;0
WireConnection;1064;0;1059;0
WireConnection;1064;1;1061;0
WireConnection;1064;2;1060;0
WireConnection;1063;0;1062;0
WireConnection;1065;0;1063;0
WireConnection;1065;2;1064;0
WireConnection;1067;0;1065;0
WireConnection;1067;1;1066;0
WireConnection;1068;0;1067;0
WireConnection;1070;1;1068;0
WireConnection;1072;0;1070;0
WireConnection;1072;1;1069;0
WireConnection;1072;2;1071;0
WireConnection;1038;0;1072;0
WireConnection;1074;1;1040;0
WireConnection;1077;0;1074;0
WireConnection;1077;1;1075;2
WireConnection;1079;0;1077;0
WireConnection;1079;1;1078;0
WireConnection;1079;2;1076;0
WireConnection;1049;0;1048;2
WireConnection;1050;0;1049;0
WireConnection;1081;0;1080;0
WireConnection;1081;2;1079;0
WireConnection;854;0;853;4
WireConnection;854;1;1039;0
WireConnection;855;0;1039;0
WireConnection;855;1;856;2
WireConnection;1051;0;1050;0
WireConnection;1033;1;746;0
WireConnection;1033;0;1081;0
WireConnection;857;0;854;0
WireConnection;857;1;855;0
WireConnection;1053;0;1051;0
WireConnection;1053;1;1052;0
WireConnection;97;1;1033;0
WireConnection;872;0;867;0
WireConnection;872;1;857;0
WireConnection;293;0;292;0
WireConnection;293;1;97;0
WireConnection;1054;0;1053;0
WireConnection;1055;0;1054;0
WireConnection;1055;1;293;0
WireConnection;1029;0;872;0
WireConnection;1031;0;1029;0
WireConnection;1056;0;1055;0
WireConnection;1042;0;1056;0
WireConnection;1042;9;1046;0
WireConnection;1042;3;1046;0
WireConnection;1042;4;1046;0
WireConnection;1042;6;97;4
WireConnection;1042;7;1036;0
WireConnection;1042;8;1031;0
ASEEND*/
//CHKSM=A70826D4B6A8FACB7D7033290DADF343F3DE0E44