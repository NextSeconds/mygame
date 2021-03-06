Shader "37GameCoreShader/NPCModelRTT"
{
	Properties
	{
		_MainTex("MainTex",2D) = "white" {}
	}
	
	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200
		Cull Off
		AlphaTest Greater 0.1
		Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
			
			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
				o.uv = v.texcoord;
				return o;
			}
			
			
			sampler2D _MainTex;
			
			half4 frag(v2f i) : COLOR
			{
				half4 colorOrignal = tex2D(_MainTex,i.uv);
				half4 ret;
				ret.rgb = colorOrignal.rgb;
				if(colorOrignal.r > 0)
					ret.a	= 1;
				else				
					ret.a = 0;
				
				return ret;
			}
			ENDCG
		}
	}
	
	Fallback "VertexLit"
}