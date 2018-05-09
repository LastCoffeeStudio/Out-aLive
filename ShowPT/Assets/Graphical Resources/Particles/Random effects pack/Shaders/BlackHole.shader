Shader "ErbGameArt/BlackHole" {
    Properties {
        _EffectPower ("EffectPower", Range(0.5, 10)) = 2.4
        _Speed ("Speed", Range(-2, 2)) = 1.2
        _Spin ("Spin", Range(-20, 20)) = 8.4
        _PowerOfLines ("PowerOfLines", Range(1, 7)) = 1.5
        _Strensh ("Strensh", Range(0, 2)) = 0.41
        _Color ("Color", Color) = (0,0,0,1)
        _MultiLines ("MultiLines", Float ) = 0.5
        _SpeenPower ("SpeenPower", Range(0.1, 1)) = 0.1
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            ZWrite Off
            Cull Off
			
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            uniform sampler2D _GrabTexture;
            uniform float4 _TimeEditor;
            uniform float _EffectPower;
            uniform float _Speed;
            uniform float _Spin;
            uniform float _PowerOfLines;
            uniform float _Strensh;
            uniform float4 _Color;
            uniform float _MultiLines;
            uniform float _SpeenPower;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5;
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float4 node_6404 = _Time + _TimeEditor;
                float node_5231 = saturate((1.0 - length(abs(((i.uv0-0.5)/0.5)))));
                float node_9521_ang = ((frac((node_6404.g*_Speed))*3.141592654)+(node_5231*_Spin));
                float node_9521_cos = cos(1.0*node_9521_ang);
                float node_9521_sin = sin(1.0*node_9521_ang);
                float2 node_9521 = (mul(i.uv0-float2(0.5,0.5),float2x2( node_9521_cos, -node_9521_sin, node_9521_sin, node_9521_cos))+float2(0.5,0.5));
                float2 node_7679 = abs(((node_9521-0.5)/0.5)).rg;
                float node_1380 = node_5231;
                float node_6699 = saturate(((saturate(frac(pow(((_MultiLines*atan2(node_7679.r,node_7679.g))*(_MultiLines*atan2(node_7679.g,node_7679.r))),_SpeenPower)))*pow(node_1380,_EffectPower))+pow(node_1380,_PowerOfLines)));
                float node_9822 = (_Strensh*node_6699);
                float node_2935_ang = (node_9822*node_9822);
                float node_2935_cos = cos(1.0*node_2935_ang);
                float node_2935_sin = sin(1.0*node_2935_ang);
                float2 node_2935_piv = float2(0.5,0.5);
                float2 node_2935 = (mul(sceneUVs.rg-node_2935_piv,float2x2( node_2935_cos, -node_2935_sin, node_2935_sin, node_2935_cos))+node_2935_piv);
                float3 finalColor = lerp(saturate(tex2D( _GrabTexture, node_2935).rgb),_Color.rgb,node_6699);
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
