// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Unlit/SphereSnitch"
{
	Properties
	{
		[Enum(Off,0,Front,1,Back,2)] _CullMode("Culling Mode", int) = 0
		[Enum(Off,0,On,1)] _ZWrite("ZWrite", int) = 0
		_Color("Diffuse Material Color", Color) = (1,1,1,1)
		_Progress("Progress",Range(0,1)) = 0
		_MainTex("Main Texture", 2D) = "white" {}
		_DissolveTex("Dissolve Texture", 2D) = "white" {}
		_Edge("Edge",Range(0.01,0.5)) = 0.01
		_Transparency("Transparency", Range(0.0,1.0)) = 0.25

		[Header(Edge Color)]
		[Toggle(EDGE_COLOR)] _UseEdgeColor("Edge Color?", Float) = 1
		[HideIfDisabled(EDGE_COLOR)][NoScaleOffset] _EdgeAroundRamp("Edge Ramp", 2D) = "white" {}
		[HideIfDisabled(EDGE_COLOR)]_EdgeAround("Edge Color Range",Range(0,0.5)) = 0
		[HideIfDisabled(EDGE_COLOR)]_EdgeAroundPower("Edge Color Power",Range(1,5)) = 1
		[HideIfDisabled(EDGE_COLOR)]_EdgeAroundHDR("Edge Color HDR",Range(1,3)) = 1
		[HideIfDisabled(EDGE_COLOR)]_EdgeDistortion("Edge Distortion",Range(0,1)) = 0
	}
	
	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Opaque" "LightMode" = "ForwardBase" }

		Pass
		{

			Blend SrcAlpha OneMinusSrcAlpha //Alpha Blend
			Cull[_CullMode] Lighting Off ZWrite[_ZWrite]

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature EDGE_COLOR

			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv3 : TEXCOORD3;
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

			float4 _Color;
			sampler2D _MainTex;
			sampler2D _DissolveTex;
			float4 _MainTex_ST;
			float4 _DissolveTex_ST;
			fixed _Edge;
			fixed _Progress;
			float _Transparency;

			#ifdef EDGE_COLOR
				sampler2D _EdgeAroundRamp;
				fixed _EdgeAround;
				float _EdgeAroundPower;
				float _EdgeAroundHDR;
				fixed _EdgeDistortion;
			#endif

			v2f vert(appdata v)
			{
				v2f o;


				float4x4 modelMatrixInverse = unity_WorldToObject;

				float3 normalDirection = normalize(mul(float4 (v.normal, 0.0), modelMatrixInverse));

				float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);

				float3 diffuseReflection = _LightColor0.rgb * _Color.rgb * max(0.0, dot(normalDirection, lightDirection));

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv3 = TRANSFORM_TEX(v.uv, _DissolveTex);
				o.color = v.color;

				o.color = float4(diffuseReflection, 1.0) + UNITY_LIGHTMODEL_AMBIENT;
				o.color.a *= _Progress;
				return o;
			}

		fixed4 frag(v2f i) : SV_Target
		{
				fixed4 col = tex2D(_DissolveTex, i.uv3);
				fixed x = col.r;
				fixed progress = i.color.a;

				//Edge
				fixed edge = lerp(x + _Edge, x - _Edge, progress);
				fixed alpha = smoothstep(progress + _Edge, progress - _Edge, edge);
				alpha *= _Transparency;
				#ifdef EDGE_COLOR
					//Edge Around Factor
					fixed edgearound = lerp(x + _EdgeAround, x - _EdgeAround, progress);
					edgearound = smoothstep(progress + _EdgeAround, progress - _EdgeAround, edgearound);
					edgearound = pow(edgearound, _EdgeAroundPower);

					//Edge Around Distortion
					fixed avoid = 0.15f;
					fixed distort = edgearound * alpha*avoid;
					float2 cuv = lerp(i.uv, i.uv + distort - avoid, progress * _EdgeDistortion);
					col = tex2D(_MainTex, cuv);
					col.rgb *= i.color.rgb;

					//Edge Around Color
					fixed3 ca = tex2D(_EdgeAroundRamp, fixed2(1 - edgearound, 0)).rgb;
					ca = (col.rgb + ca)*ca*_EdgeAroundHDR;
					col.rgb = lerp(ca, col.rgb, edgearound);
				#else
					col = tex2D(_MainTex, i.uv);
					col.rgb *= i.color.rgb;
				#endif

				col.a *= alpha;


				return col;
			}
			ENDCG
		}
	}
}
