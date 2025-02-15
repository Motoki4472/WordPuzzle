Shader "Custom/TransparentTextByY"
{
    Properties
    {
        _MainTex ("Font Texture", 2D) = "white" {}
        _MinY ("Min Y (fully transparent)", Float) = -1.0
        _MaxY ("Max Y (fully opaque)", Float) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float worldY : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _MinY;
            float _MaxY;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldY = worldPos.y;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // ワールドY座標に基づいて透明度を設定
                float alpha = saturate((i.worldY - _MinY) / (_MaxY - _MinY));
                col.a *= alpha;

                return col;
            }
            ENDCG
        }
    }
}
