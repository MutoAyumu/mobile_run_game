Shader "Unlit/UVScroll"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _ScrollX("ScrollX", float) = 0
        _ScrollY("ScrollY", float) = 0
    }

        SubShader
        {
            Tags { "RenderType" = "Opaque" }

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                fixed _ScrollX, _ScrollY;
                float _CurveStrengthY;
                float _CurveStrengthX;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);

                    float dist = UNITY_Z_0_FAR_FROM_CLIPSPACE(o.vertex.z);

                    o.vertex.y -= _CurveStrengthY * dist * dist * _ProjectionParams.x;
                    o.vertex.x -= _CurveStrengthX * dist * dist * _ProjectionParams.y;

                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                    o.uv = o.uv + fixed2(frac(_ScrollX * _Time.y), frac(_ScrollY * _Time.y));

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    return tex2D(_MainTex, i.uv);
                }

                ENDCG
            }
        }
}