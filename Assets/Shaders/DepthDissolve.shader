Shader "Unlit/DepthDissolve"
{
    Properties
    {
        _BaseColor ("Color", Color) = (1,1,1)
        _EdgeColor ("Dissolve Color", Color) = (0, 0, 0)
        _MainTex ("Texture", 2D) = "white" {}
        _DiffuseShade("Diffuse Shade",Range(0,1)) = 0.5
        _DissolveTex ("Dissolve Texture", 2D) = "white" {}
        _AlphaClipThreshold ("Alpha Clip Threshold", Range(0,1)) = 0.5
        _EdgeWidth ("Disolve Margin Width", Range(0,1)) = 0.01
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "LightMode" = "ForwardBase"}
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float3 normal:NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : TEXCOORD2;
                half3 worldNormal:TEXCOOR1;
                SHADOW_COORDS(1)
            };

            fixed4 _BaseColor;
            fixed4 _EdgeColor;
            half _AlphaClipThreshold;
            half _EdgeWidth;

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _DiffuseShade;

            float _CurveStrengthY;
            float _CurveStrengthX;

            sampler2D _DissolveTex;
            float4 _DissolveTex_ST;

            v2f vert (appdata v)
            {
                v2f o;

                float _Horizon = 100.0f;
                float _FadeDist = 50.0f;

                o.vertex = UnityObjectToClipPos(v.vertex);


                float dist = UNITY_Z_0_FAR_FROM_CLIPSPACE(o.vertex.z);

                o.vertex.y -= _CurveStrengthY * dist * dist * _ProjectionParams.x;
                o.vertex.x -= _CurveStrengthX * dist * dist * _ProjectionParams.y;

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                o.color = v.color;

                //法線方向のベクトル
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                TRANSFER_SHADOW(o)

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 edgeCol = fixed4(1, 1, 1, 1);
                
                // noise textureからalpha値を取得
                fixed4 dissolve = tex2D(_DissolveTex, i.uv);
                float alpha = dissolve.r * 0.2 + dissolve.g * 0.7 + dissolve.b * 0.1;

                // dissolveを段階的な色変化によって実現する
                if (alpha < _AlphaClipThreshold + _EdgeWidth && _AlphaClipThreshold > 0) {
                    edgeCol = _EdgeColor;
                }
                if (alpha < _AlphaClipThreshold) {
                    discard;
                }

                //1つ目のライトのベクトルを正規化
                float3 L = normalize(_WorldSpaceLightPos0.xyz);
                //ワールド座標系の法線を正規化
                float3 N = normalize(i.worldNormal);
                //ライトベクトルと法線の内積からピクセルの明るさを計算 ランバートの調整もここで行う
                fixed4 diffuseColor = max(0, dot(N, L) * _DiffuseShade + (1 - _DiffuseShade));
                //ライトの色を乗算
                fixed4 col = tex2D(_MainTex, i.uv) * _BaseColor * edgeCol * diffuseColor * _LightColor0;
                // 影を計算
                col *= SHADOW_ATTENUATION(i);

                return col;
            }
            ENDCG
        }
    }
}