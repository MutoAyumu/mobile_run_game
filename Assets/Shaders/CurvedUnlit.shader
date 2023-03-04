Shader "Unlit/CurvedUnlit"
{ 
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DiffuseShade("Diffuse Shade",Range(0,1)) = 0.5
	    _BaseColor("Color", Color) = (1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
				
			#include "CurvedCode.cginc"

			ENDCG
		}
	}
}
