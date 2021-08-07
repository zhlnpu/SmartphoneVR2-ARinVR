Shader "SmartPhoneExp/SmartPhonePointCloudGeomSkin" {
	Properties{
		[NoScaleOffset]_MainTex("Texture", 2D) = "white" {}
				[NoScaleOffset]_MainTexColorDepth ("_ColorDepth", 2D) = "white" {}

		[NoScaleOffset]_UVMap("UV", 2D) = "white" {}
		_PointSize("Point Size", Float) = 4.0
		_ColorSkin("PointCloud Color", Color) = (1, 1, 1, 1)
		[Toggle(USE_DISTANCE)]_UseDistance("Scale by distance?", float) = 0
	}

		SubShader
		{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}

			Cull Off

			LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma geometry geom
				#pragma fragment frag
				#pragma shader_feature USE_DISTANCE
				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float2 uv : TEXCOORD0;
				};

				float _PointSize;
				fixed4 _ColorSkin;

				sampler2D _MainTex;
				float4 _MainTex_TexelSize;
			sampler2D	_MainTexColorDepth;
			float4	__MainTexColorDepth_TexelSize;

				sampler2D _UVMap;
				float4 _UVMap_TexelSize;


				struct g2f
				{
					float4 vertex : SV_POSITION;
					float2 uv : TEXCOORD0;
				};

				[maxvertexcount(4)]
				void geom(point v2f i[1], inout TriangleStream<g2f> triStream)
				{
					g2f o;
					float4 v = i[0].vertex;
					v.y = -v.y;

					// TODO: interpolate uvs on quad
					float2 uv = i[0].uv;
					float2 p = _PointSize * 0.001;
					p.y *= _ScreenParams.x / _ScreenParams.y;

					o.vertex = UnityObjectToClipPos(v);
					#ifdef USE_DISTANCE
					o.vertex += float4(-p.x, p.y, 0, 0);
					#else
					o.vertex += float4(-p.x, p.y, 0, 0) * o.vertex.w;
					#endif
					o.uv = uv;
					triStream.Append(o);

					o.vertex = UnityObjectToClipPos(v);
					#ifdef USE_DISTANCE
					o.vertex += float4(-p.x, -p.y, 0, 0);
					#else
					o.vertex += float4(-p.x, -p.y, 0, 0) * o.vertex.w;
					#endif
					o.uv = uv;
					triStream.Append(o);

					o.vertex = UnityObjectToClipPos(v);
					#ifdef USE_DISTANCE
					o.vertex += float4(p.x, p.y, 0, 0);
					#else
					o.vertex += float4(p.x, p.y, 0, 0) * o.vertex.w;
					#endif
					o.uv = uv;
					triStream.Append(o);

					o.vertex = UnityObjectToClipPos(v);
					#ifdef USE_DISTANCE
					o.vertex += float4(p.x, -p.y, 0, 0);
					#else
					o.vertex += float4(p.x, -p.y, 0, 0) * o.vertex.w;
					#endif
					o.uv = uv;
					triStream.Append(o);

				}

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = v.vertex;
					o.uv = v.uv;
					return o;
				}

				float3 rgb_to_hsv_no_clip(float3 RGB)
				{
					float3 HSV;

					float minChannel, maxChannel;
					if (RGB.x > RGB.y) {
						maxChannel = RGB.x;
						minChannel = RGB.y;
					}
					else {
						maxChannel = RGB.y;
						minChannel = RGB.x;
					}

					if (RGB.z > maxChannel) maxChannel = RGB.z;
					if (RGB.z < minChannel) minChannel = RGB.z;

					HSV.xy = 0;
					HSV.z = maxChannel;
					float delta = maxChannel - minChannel;             //Delta RGB value
					if (delta != 0) {                    // If gray, leave H  S at zero
						HSV.y = delta / HSV.z;
						float3 delRGB;
						delRGB = (HSV.zzz - RGB + 3 * delta) / (6.0*delta);
						if (RGB.x == HSV.z) HSV.x = delRGB.z - delRGB.y;
						else if (RGB.y == HSV.z) HSV.x = (1.0 / 3.0) + delRGB.x - delRGB.z;
						else if (RGB.z == HSV.z) HSV.x = (2.0 / 3.0) + delRGB.y - delRGB.x;
					}
					return (HSV);
				}






				float GetMax(float r, float g, float b)
				{
					return r > g ? (r > b ? r : b) : (g > b ? g : b);
				}

				float GetMin(float r, float g, float b)
				{
					return r < g ? (r < b ? r : b) : (g < b ? g : b);
				}


				fixed4 frag(g2f i) : SV_Target
				{
					float2 uv = tex2D(_UVMap, i.uv);
					if (any(uv <= 0 || uv >= 1))
						discard;
					// offset to pixel center	
					uv += 0.5 * _MainTex_TexelSize.xy;



					float4 aa = tex2D(_MainTex, uv) * _ColorSkin;


					//hsv
					float3 rgb;
					rgb.x = aa.x;
					rgb.y = aa.y;
					rgb.z = aa.z; 


					float3 hsv = rgb_to_hsv_no_clip(rgb);

					//	0 <= H <= 17 and 15 <= S <= 170 and 0 <= V <= 255
					if (    hsv.x > 1 / 255 || hsv.y >100 / 255 )
					{
						//rgb
						//original
						//if (aa.x < 0.37255 || aa.x>0.7 || aa.y < 0.15686 || aa.z < 0.07843 || aa.x < aa.z || aa.x - aa.y < 0.0588 || GetMax(aa.x, aa.y, aa.z) - GetMin(aa.x, aa.y, aa.z) < 0.0588)
								//extended						
						if (aa.x < 0.13 || aa.x>0.7 || aa.y < 0.05|| aa.y >0.7 || aa.z < 0.05 || aa.z>0.7 || aa.x < aa.z || aa.x - aa.y < 0.05 || GetMax(aa.x, aa.y, aa.z) - GetMin(aa.x, aa.y, aa.z) < 0.04)
							discard;
					}
					else
					{
						if (aa.x < 0.13 || aa.x>0.7 || aa.y < 0.05 || aa.y >0.7 || aa.z < 0.05 || aa.z>0.7 || aa.x < aa.z || aa.x - aa.y < 0.05 || GetMax(aa.x, aa.y, aa.z) - GetMin(aa.x, aa.y, aa.z) < 0.04)
							discard;
					}
					

				//	if ( hsv.x<0.2f/255||  hsv.x > 17.0f / 255 || hsv.y <15.0f / 255 || hsv.y >170.0f / 255 || hsv.z > 0.8)
				//		
				////	if (aa.x < 0.3 || aa.x>0.5 || aa.y < 0.1 || aa.y>0.8 || aa.z < 0.07 || aa.z>0.8 || aa.x < aa.z || aa.x - aa.y < 0.05
				////				|| GetMax(aa.x, aa.y, aa.z) - GetMin(aa.x, aa.y, aa.z) < 0.05)
				//			discard;

					//return float4(rgb.x, rgb.y, rgb.z, 1);
					




				/*		float4 newColor= tex2D(_MainTex, uv) * _ColorSkin;

						newColor.w = 0.5;
						return newColor;*/





					//return  _ColorSkin;
					//return tex2D(_MainTex, uv) * _ColorSkin;

					return tex2D(_MainTex, uv) * _ColorSkin;


				}
					ENDCG
			}
		}
}
