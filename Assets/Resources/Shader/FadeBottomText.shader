Shader "Custom/FadeBottomText"
{
    Properties
    {
        _MainTex ("Font Texture", 2D) = "white" {}
        _FadeHeight ("Fade Height", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            Lighting Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float yPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float _FadeHeight;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color;
                o.yPos = v.vertex.y; // Y座標を保存
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
                
                // Y座標に基づいて透明度を変化
                float alphaFactor = smoothstep(0.0, _FadeHeight, i.yPos);
                col.a *= alphaFactor;

                return col;
            }
            ENDCG
        }
    }
}
