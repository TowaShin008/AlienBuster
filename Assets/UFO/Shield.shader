Shader "Unlit/Shield"
{
	Properties
	{
		_MainColor("MainColor", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Fresnel("Fresnel Intensity", Range(0,200)) = 3.0
		_FresnelWidth("Fresnel Width", Range(0,2)) = 3.0
		_Distort("Distort", Range(0, 100)) = 1.0
		_IntersectionThreshold("Highlight of intersection threshold", range(0,1)) = .1 //Max difference for intersections
		_ScrollSpeedU("Scroll U Speed",float) = 2
		_ScrollSpeedV("Scroll V Speed",float) = 0
		//[ToggleOff]_CullOff("Cull Front Side Intersection",float) = 1
		_MyAlpha("Alpha",Range(0,1)) = 1
	}
	SubShader
	{ 
		Tags{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent" 
			"RenderPipeline"="UniversalPipeline"
		}

		GrabPass{ "_GrabTexture" }
		Pass
		{
			Tags { "LightMode" = "UniversalForward"}

			Lighting Off ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"

			#define COMPUTE_EYEDEPTH(o) o = -TransformWorldToView( v.vertex.xyz ).z

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal: NORMAL;
				float3 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 rimColor :TEXCOORD1;
				float4 screenPos: TEXCOORD2;
			};

			sampler2D _MainTex, _CameraDepthTexture, _GrabTexture;

			CBUFFER_START(UnityPerMaterial)

			float4 _MainTex_ST,_MainColor,_GrabTexture_ST;
			float _Fresnel, _FresnelWidth, _Distort, _IntersectionThreshold;
			float _MyAlpha;

			CBUFFER_END

			float4 _GrabTexture_TexelSize;
			float2 _ScrollSpeedUV;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = TransformObjectToHClip(v.vertex.xyz);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				//scroll uv
				o.uv += _Time.xy * _ScrollSpeedUV;

				//fresnel 
				float3 viewDir = normalize(TransformWorldToObject(v.vertex.xyz));
				float dotProduct = 1 - saturate(dot(v.normal.xyz, viewDir));
				o.rimColor = smoothstep(1 - _FresnelWidth, 1.0, dotProduct) * .5f;
				o.screenPos = ComputeScreenPos(o.vertex);
				COMPUTE_EYEDEPTH(o.screenPos.z);//eye space depth of the vertex 
				return o;
			}
			
			float4 frag (v2f i,float face : VFACE) : SV_Target
			{
				//intersection
				float intersect = saturate((abs(LinearEyeDepth(tex2Dproj(_CameraDepthTexture,i.screenPos).r, _ZBufferParams) - i.screenPos.z)) / _IntersectionThreshold);

				float3 main = tex2D(_MainTex, i.uv).xyz;
				//distortion
				i.screenPos.xy += (main.rg * 2 - 1) * _Distort * _GrabTexture_TexelSize.xy;
				float3 distortColor = tex2Dproj(_GrabTexture, i.screenPos).xyz;
				distortColor *= _MainColor.xyz * _MainColor.a + 1;

				//intersect hightlight
				i.rimColor *= intersect * clamp(0,1,face);
				main *= _MainColor.xyz * pow(abs(_Fresnel), i.rimColor) ;
				
				//lerp distort color & fresnel color
				main = lerp(distortColor, main, i.rimColor.r);
				main += (1 - intersect) * (face > 0 ? .03:.3) * _MainColor.xyz * _Fresnel;				
				return float4(main, _MyAlpha);
				//return float4(main,.9);
			}
			ENDHLSL
		}
	}
}
