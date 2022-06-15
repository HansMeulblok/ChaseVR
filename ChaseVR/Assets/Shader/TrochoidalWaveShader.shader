Shader "Custom/TrochoidalWaveShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap("Bumpmap", 2D) = "bump" {}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0

        _DeltaSpeed("Delta Speed", Float) = 1
        _Offset("Offset", Float) = 1
        _Radius("Radius", Float) = 1

        _WaveStartPos("Wave Start Position", Vector) = (-50, 0, -45, 1)
        _SurfBoardPos("Surf Board Pos", Vector) = (0, 0, 0, 1)

        _TimeValue("Time Value", Float) = 1
        _Translation("Translation", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard alpha vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        float _DeltaSpeed;
        float _Offset;
        float _Radius;

        float _TimeValue;

        float3 _WaveStartPos;
        float3 _SurfBoardPos;
        float3 _WorldSpace;
        float3 _RotatedOffset;

        float _Translation;


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void vert (inout appdata_full v) 
        {
            _WorldSpace = mul(unity_ObjectToWorld, v.vertex);
            _RotatedOffset = float3(0, 0, 0);

            _Radius = 0.7 * log(-(distance(_WorldSpace, _WaveStartPos) - 120)) - 1;
            _Radius /= distance(_WorldSpace.x, _SurfBoardPos.x) * 0.3;
        
            _Radius = clamp(_Radius, 0.1, 2);

            _RotatedOffset.x = sin((_TimeValue * _DeltaSpeed + _WorldSpace.x * _Offset)/* + _Translation*/) * _Radius;
            _RotatedOffset.y = cos(/*1.2 **/ (_TimeValue * _DeltaSpeed + _WorldSpace.x * _Offset) /*+ _Translation*/) * _Radius;
            v.vertex.xyz += mul(unity_WorldToObject, _RotatedOffset);
            
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    /*FallBack "Diffuse"*/
}
