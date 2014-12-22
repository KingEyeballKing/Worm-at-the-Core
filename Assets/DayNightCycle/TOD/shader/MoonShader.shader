// Shader created with Shader Forge Beta 0.36 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.36;sub:START;pass:START;ps:flbk:Self-Illumin/Diffuse,lico:0,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:True,frtr:True,vitr:True,dbil:True,rmgx:True,rpth:0,hqsc:False,hqlp:False,tesm:0,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.1617647,fgcg:0.1617647,fgcb:0.1617647,fgca:1,fgde:0.015,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32719,y:32712|emission-29-OUT,alpha-206-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:33296,y:32683,ptlb:Texture/Emission,ptin:_TextureEmission,tex:f52adaff93fd744e3993a5151b295e1e,ntxv:1,isnm:False;n:type:ShaderForge.SFN_Add,id:29,x:33101,y:32846|A-2-RGB,B-30-OUT;n:type:ShaderForge.SFN_Vector1,id:30,x:33296,y:32964,v1:0.2;n:type:ShaderForge.SFN_ValueProperty,id:206,x:33076,y:33042,ptlb:Alpha,ptin:_Alpha,glob:False,v1:1;proporder:2-206;pass:END;sub:END;*/

Shader "Custom/MoonShader" {
    Properties {
        _TextureEmission ("Texture/Emission", 2D) = "gray" {}
        _Alpha ("Alpha", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers d3d9 d3d11 xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _TextureEmission; uniform float4 _TextureEmission_ST;
            uniform float _Alpha;
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
                float2 node_277 = i.uv0;
                float3 emissive = (tex2D(_TextureEmission,TRANSFORM_TEX(node_277.rg, _TextureEmission)).rgb+0.2);
                float3 finalColor = emissive;
/// Final Color:
                return fixed4(finalColor,_Alpha);
            }
            ENDCG
        }
    }
    FallBack "Self-Illumin/Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
