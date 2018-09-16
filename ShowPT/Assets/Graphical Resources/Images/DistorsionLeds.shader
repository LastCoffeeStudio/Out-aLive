// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/DistorsionLeds"
{
	Properties
	{
		_NoiseTex("Noise Texture (RG)", 2D) = "white" {}
	_Distortion("Distortion", Float) = 1.0
		_Alpha("Alpha", Float) = 1.0
	}

		Category
	{
		Tags{ "Queue" = "Transparent+200" }
		SubShader
	{

		GrabPass
	{
		"_GrabTex"
		Name "BASE"
		Tags{ "LightMode" = "Always" } // no lighting
	}
		
		Pass
	{
		Stencil{
		Ref 5
		Comp Always
		Pass Replace
	}
		Name "BASE"
		Tags{ "LightMode" = "Always" }
		Lighting Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		sampler2D _GrabTex;
	float4 _NoiseTex_ST;
	sampler2D _NoiseTex;
	float _Distortion;
	float _Alpha;

	struct data
	{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float4 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		float4 position : POSITION;
		float4 screenPos : TEXCOORD0;
		float2 uvmain : TEXCOORD1;
		float distortion : TEXCOORD2;
	};

	v2f vert(data i)
	{
		v2f o;
		o.position = UnityObjectToClipPos(i.vertex);      // compute transformed vertex position
		o.uvmain = TRANSFORM_TEX(i.texcoord, _NoiseTex);   // compute the texcoords of the noise
		float viewAngle = dot(normalize(ObjSpaceViewDir(i.vertex)), i.normal);
		o.distortion = viewAngle * viewAngle;   // square viewAngle to make the effect fall off stronger
		float depth = -mul(UNITY_MATRIX_MV, i.vertex).z;  // compute vertex depth
		o.distortion /= 1 + depth;  // scale effect with vertex depth
		o.distortion *= _Distortion; // multiply with user controlled _Distortion
		o.screenPos = o.position; // pass the position to the pixel shader
		return o;
	}

	half4 frag(v2f i) : COLOR
	{
		// compute the texture coordinates
		float2 screenPos = i.screenPos.xy / i.screenPos.w;   // screenpos ranges from -1 to 1
		screenPos.x = (screenPos.x + 1) * 0.5;   // I need 0 to 1
		screenPos.y = (screenPos.y + 1) * 0.5;   // I need 0 to 1

												 // check if anti aliasing is used
												 //if (_ProjectionParams.x < 0) screenPos.y = 1 - screenPos.y;
		screenPos.y = 1 - screenPos.y;

		// get two offset values by looking up the noise texture shifted in different directions
		half4 offsetColor1 = tex2D(_NoiseTex, i.uvmain + _Time.xz);
		half4 offsetColor2 = tex2D(_NoiseTex, i.uvmain - _Time.yx);

		// use the r values from the noise texture lookups and combine them for x offset
		// use the g values from the noise texture lookups and combine them for y offset
		// use minus one to shift the texture back to the center
		// scale with distortion amount
		screenPos.x += ((offsetColor1.r + offsetColor2.r) - 1) * i.distortion;
		screenPos.y += ((offsetColor1.g + offsetColor2.g) - 1) * i.distortion;
		half4 col = tex2D(_GrabTex, screenPos);
		col.a = _Alpha;
		return col;
	}

		ENDCG
	}
	}
	}
}