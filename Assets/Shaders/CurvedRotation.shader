Shader "Unlit/CurvedRotation"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_DiffuseShade("Diffuse Shade",Range(0,1)) = 0.5
		_BaseColor("Color", Color) = (1,1,1)
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" "DisableBatching" = "True"}
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
			float3 normal : NORMAL;
		};

		struct v2f
		{
			float2 uv : TEXCOORD0;
			float4 color : TEXCOORD2;
			float4 vertex : SV_POSITION;
			half3 worldNormal : TEXCOOR1;
			SHADOW_COORDS(1)
		};

		sampler2D _MainTex;
		float4 _MainTex_ST;
		float _DiffuseShade;
		fixed4 _BaseColor;
		float _CurveStrengthY;
		float _CurveStrengthX;

		v2f vert(appdata v)
		{
			v2f o;

			float _Horizon = 100.0f;
			float _FadeDist = 50.0f;

			float4 rotVert = v.vertex;
			rotVert.x = v.vertex.x * cos(_Time.y * 3.14f) - v.vertex.y * sin(_Time.y * 3.14f);
			rotVert.y = v.vertex.x * sin(_Time.y * 3.14f) + v.vertex.y * cos(_Time.y * 3.14f);

			o.vertex = UnityObjectToClipPos(rotVert);

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

		fixed4 frag(v2f i) : SV_Target
		{
			//1つ目のライトのベクトルを正規化
			float3 L = normalize(_WorldSpaceLightPos0.xyz);
			//ワールド座標系の法線を正規化
			float3 N = normalize(i.worldNormal);
			//ライトベクトルと法線の内積からピクセルの明るさを計算 ランバートの調整もここで行う
			fixed4 diffuseColor = max(0, dot(N, L) * _DiffuseShade + (1 - _DiffuseShade));
			// sample the texture
			fixed4 col = tex2D(_MainTex, i.uv) * i.color;
			//ライトの色を乗算
			col *= _BaseColor * diffuseColor * _LightColor0;
			// 影を計算
			col *= SHADOW_ATTENUATION(i);

			return col;
		}

			ENDCG
		}
	}
}