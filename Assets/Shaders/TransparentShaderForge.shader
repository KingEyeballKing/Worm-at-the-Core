// Shader created with Shader Forge Beta 0.36 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.36;sub:START;pass:START;ps:flbk:Transparent/Diffuse,lico:0,lgpr:1,nrmq:0,limd:0,uamb:False,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:2,bsrc:0,bdst:0,culm:2,dpts:2,wrdp:False,ufog:False,aust:True,igpj:True,qofs:1,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.6838235,fgcg:0.7195846,fgcb:0.75,fgca:1,fgde:0.1,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:True;n:type:ShaderForge.SFN_Final,id:1,x:32616,y:32825|diff-26-RGB,emission-66-OUT,alpha-26-A;n:type:ShaderForge.SFN_Tex2d,id:26,x:33176,y:32818,ptlb:Texture,ptin:_Texture,tex:838a0226c96744eac80e7c2df640a301,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:66,x:32874,y:32910|A-26-A,B-86-RGB;n:type:ShaderForge.SFN_Color,id:86,x:33167,y:33024,ptlb:Color,ptin:_Color,glob:False,c1:1,c2:1,c3:1,c4:1;proporder:26-86;pass:END;sub:END;*/

Shader "Shader Forge/TransparentShaderForge" {
    Properties {
        _Texture ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent+1"
            "RenderType"="Transparent"
        }
        LOD 300
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            Cull Off
            ZWrite Off
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers d3d9 d3d11 xbox360 ps3 flash d3d11_9x 
            #pragma target 2.0
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float4 _Color;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float2 node_107 = i.uv0;
                float4 node_26 = tex2D(_Texture,TRANSFORM_TEX(node_107.rg, _Texture));
                float3 emissive = (node_26.a*_Color.rgb);
                float3 finalColor = emissive;
/// Final Color:
                return fixed4(finalColor,node_26.a);
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
