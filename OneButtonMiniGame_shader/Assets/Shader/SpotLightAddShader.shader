Shader "Unlit/SpotLightAddShader"
{
    Properties
    {
        //_Color("Color", Color) = (1,1,1,0.3)
        _MainTex ("Texture", 2D) = "black" {}
        //_ColorMask ("Color Mask", Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        Tags { "Queue" = "Transparent+2" }
        //Tags { "IgnoreProjector"="True"}
        Blend SrcAlpha OneMinusSrcAlpha 
        //Blend One One
        LOD 100
        ZTest Always
        Cull Off
         Pass
         {
            // Stencil{
            //     Ref 1
            //     Comp Always
            //     Pass Replace
            //  }
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
            float half_size_x;
            float half_size_y;
            //float4 _ColorMask;
            //float localPos_x;
            //float localPos_y;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            //float4 _Color;
            float4 color_1;
            float4 color_2;
            v2f vert (appdata v)
            {
                v2f o;
                o.localPos = v.vertex.xyz;
                // o.localPos.x = localPos_x;
                // o.localPos.y = localPos_y;
                //o.localPos = v.vertex - mul(unity_WorldToObject,v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //float4 color_1 = (1,1,1,0.3);
                //float4 color_2 = _Color;
                //float4 color_1 = (1,1,1,0.3);
                //float4 color_2 = (0,0,0,0);
//***************************************************************
                // fixed4 col = tex2D(_MainTex, i.uv);

                // float dist = distance(fixed2(0,0),i.localPos);
                
                // float dist_half = distance(fixed2(0,0), (half_size_x,half_size_y));
                // float4 _Color = lerp(color_1,color_2, (dist)/dist_half);

                // col *= _Color;


//****************************************************************
                const float PI = 3.14159;
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 half_size;
                half_size.x = half_size_x;
                half_size.y = half_size_y;
                float dist = distance((0,0), i.localPos.xy/(PI*half_size.xy));
                float4 _Color = lerp(color_1,color_2, dist*10);
                col *= _Color;

                //端のごみを消去
                // if(dist*3 > 0.8)
                // {
                //     clip(-1);
                // }
//********************************************************************
                // const float PI = 3.14159;
                // fixed4 col = tex2D(_MainTex, i.uv);
                // float2 half_size;
                // half_size.x = half_size_x;
                // half_size.y = half_size_y;
                // float dist = (half_size.y-i.localPos.y)/(half_size.x-i.localPos.x);
                // float4 _Color = lerp(color_1,color_2, dist/5);
                // col *= _Color;

                return col;
            }
            ENDCG
        }
    }
}
