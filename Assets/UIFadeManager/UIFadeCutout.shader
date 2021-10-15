Shader "Unlit/UIFadeCutout" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", color) = (0, 0, 0, 1)
        _Scale ("Scale", float) = 1.0
    }
    SubShader {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float4 _Color;
            float _Scale;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                const float2 center = float2(0.5, 0.5);
                const float2 r = (o.uv.xy - center) / _Scale;
                o.uv.xy = r + center;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                const fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 result;
                //α値によって描画の有無を決める.
                if(col.a > 0.1) {
                    discard;
                }
                result.rgb = _Color.xyz;
                result.a = 1;
                return result;
            }
            ENDCG
        }
    }
}