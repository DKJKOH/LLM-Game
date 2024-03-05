Shader "Custom/LitSpriteShader"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" { }
    }

    SubShader
    {
        Tags { "Queue" = "Overlay" }

        Blend SrcAlpha OneMinusSrcAlpha  // Enable alpha blending

        CGPROGRAM
        #pragma surface surf Lambert alpha

        struct Input
        {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;

        void surf(Input IN, inout SurfaceOutput o)
        {
            // Sample the texture
            fixed4 col = tex2D(_MainTex, IN.uv_MainTex);

            // Add basic lighting
            fixed3 lighting = _LightColor0.rgb * _LightColor0.a;
            col.rgb *= lighting;

            o.Albedo = col.rgb;
            o.Alpha = col.a;
        }
        ENDCG
    }

    FallBack "Diffuse"
}
