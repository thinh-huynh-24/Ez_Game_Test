// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "T70/URP/SelfIlluminDiffuse"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _Illum ("Illumination Mask (Alpha)", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        _Emission ("Emission Strength", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }
        LOD 100

        Pass
        {
            Name "ForwardUnlit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            sampler2D _Illum;
            float4 _Color;
            float _Emission;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 baseColor = tex2D(_MainTex, IN.uv) * _Color;
                half illumMask = tex2D(_Illum, IN.uv).a;
                half3 emission = baseColor.rgb * illumMask * _Emission;
                return half4(baseColor.rgb + emission, baseColor.a);
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/InternalErrorShader"
}
