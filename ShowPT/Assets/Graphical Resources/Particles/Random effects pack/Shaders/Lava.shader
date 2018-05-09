Shader "ErbGameArt/Lava" {
    Properties {
        _color ("color", Color) = (1,0.475862,0,1)
        _FlowMap ("FlowMap", 2D) = "bump" {}
        _U_Speed ("U_Speed", Float ) = 0.5
        _V_Speed ("V_Speed", Float ) = 0.25
        _Strench ("Strench", Float ) = 0.2
        _MainTexture ("MainTexture", 2D) = "black" {}
        _TextureDistanse ("TextureDistanse", Range(0, 1)) = 1
        _FresnelStrench ("FresnelStrench", Float ) = 1
        _TextureStrench ("TextureStrench", Range(-1, 1)) = 1
        _UndoEffectTexture ("UndoEffectTexture", 2D) = "black" {}
        [MaterialToggle] _Fresnel ("Fresnel", Float ) = 0
        _EmmisionStrench ("EmmisionStrench", Range(0, 8)) = 1.8
        _Relief ("Relief", Range(0, 10)) = 0.5
        _TextureChangespeed ("TextureChange speed", Range(0, 20)) = 5
        _Outlinefresnel ("Outline fresnel", Float ) = 2
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform sampler2D _FlowMap; uniform float4 _FlowMap_ST;
            uniform float _U_Speed;
            uniform float _V_Speed;
            uniform float _Strench;
            uniform sampler2D _MainTexture; uniform float4 _MainTexture_ST;
            uniform float4 _color;
            uniform float _TextureDistanse;
            uniform float _FresnelStrench;
            uniform float _TextureStrench;
            uniform sampler2D _UndoEffectTexture; uniform float4 _UndoEffectTexture_ST;
            uniform fixed _Fresnel;
            uniform float _EmmisionStrench;
            uniform float _Relief;
            uniform float _TextureChangespeed;
            uniform float _Outlinefresnel;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                UNITY_FOG_COORDS(3)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_4620 = _Time + _TimeEditor;
                float2 node_1175 = (o.uv0+(node_4620.g*_TextureChangespeed)*float2(0.1,0.1));
                float4 node_7690 = tex2Dlod(_MainTexture,float4(TRANSFORM_TEX(node_1175, _MainTexture),0.0,0));
                v.vertex.xyz += (saturate(node_7690.rgb)*v.normal*_Relief);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 node_3379 = _Time + _TimeEditor;
                float2 node_388 = (i.uv0+(node_3379.g*_U_Speed)*float2(1,0));
                float3 node_1054 = UnpackNormal(tex2D(_FlowMap,TRANSFORM_TEX(node_388, _FlowMap)));
                float2 node_2673 = (i.uv0+(node_3379.g*_V_Speed)*float2(0,1));
                float3 node_7769 = UnpackNormal(tex2D(_FlowMap,TRANSFORM_TEX(node_2673, _FlowMap)));
                float2 node_7014 = ((i.uv0+(float2(node_1054.r,node_7769.g)*_Strench))+_TextureDistanse*float2(1,0));
                float4 node_9700 = tex2D(_MainTexture,TRANSFORM_TEX(node_7014, _MainTexture));
                float4 node_4620 = _Time + _TimeEditor;
                float2 node_1175 = (i.uv0+(node_4620.g*_TextureChangespeed)*float2(0.1,0.1));
                float4 node_7690 = tex2D(_MainTexture,TRANSFORM_TEX(node_1175, _MainTexture));
                float3 node_4951 = (_color.rgb*(node_9700.r*node_7690.g)*_EmmisionStrench);
                float2 UV = i.uv0;
                float2 node_4937 = UV;
                float4 _UndoEffectTexture_var = tex2D(_UndoEffectTexture,TRANSFORM_TEX(node_4937, _UndoEffectTexture));
                float3 node_6801 = saturate((_UndoEffectTexture_var.rgb+_TextureStrench));
                float node_3101 = (1.0 - pow(1.0-max(0,dot(normalDirection, viewDirection)),_FresnelStrench));
                float3 emissive = lerp( (node_4951*node_6801), (node_4951*saturate((node_3101*node_3101*_Outlinefresnel))*node_6801), _Fresnel );
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform sampler2D _MainTexture; uniform float4 _MainTexture_ST;
            uniform float _Relief;
            uniform float _TextureChangespeed;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_4620 = _Time + _TimeEditor;
                float2 node_1175 = (o.uv0+(node_4620.g*_TextureChangespeed)*float2(0.1,0.1));
                float4 node_7690 = tex2Dlod(_MainTexture,float4(TRANSFORM_TEX(node_1175, _MainTexture),0.0,0));
                v.vertex.xyz += (saturate(node_7690.rgb)*v.normal*_Relief);
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
