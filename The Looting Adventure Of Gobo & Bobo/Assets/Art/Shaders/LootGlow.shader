Shader "Custom/LootGlow"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
         _Emission("Emission", Range(0.0,2.0)) = 0
         [HDR] _EmissionColor("Color", Color) = (0,0,0)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        struct Input
        {
            float2 uv;
        };

        half _Glossiness;
        fixed4 _EmissionColor;
        float _Emission;
        half _Metallic;
        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Emission = c.rgb * _EmissionColor * _Emission;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
