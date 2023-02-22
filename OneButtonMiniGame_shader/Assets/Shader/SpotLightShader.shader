Shader "Unlit/SpotLightShader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,0)
        _MainTex ("Texture", 2D) = "white" {}
        //_ColorMask ("Color Mask", Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        Tags { "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha 
        //Tags {"Queue" = "Geometry-1"}
        //LOD 100
        //ColorMask A
        Zwrite On
        Pass
        {
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members localPos)
//#pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            //#pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
//                float3 worldPos : TEXCOORD1;
                float2 localPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            //float4 _ColorMask;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            v2f vert (appdata v)
            {
                v2f o;
                o.localPos = v.vertex.xyz;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                float dist = distance(fixed2(0,0),i.localPos);
                _Color.a = (dist > 0) ? (1 - 1 / dist) : 1;
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                col = _Color;
                return col;
            }
            ENDCG
        }
    }
}
