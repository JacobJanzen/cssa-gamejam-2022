Shader "Custom/BackgroundShader"
{
	Properties
	{
		_Altitude("Altitude", float) = 0.0
		_PlayerX("PlayerX", float) = 0.0
		_MaxAltitude("MaxAltitude", float) = 1.0
	}

		SubShader
	{
		Lighting Off
		Blend One Zero

		Pass
		{
			CGPROGRAM

			#define STARS_PARALLAX_MULT 5
			#define CLOUDS_PARALLAX_MULT 0.05
			#define CLOUDS_WIND_SPEED 1

			#define PI 3.14159265
			#define PHI 1.61803398874989484820459  // Φ = Golden Ratio

			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"

			struct VoronoiOutput {
				float value;
				float cell;
			};

			inline float4 starBrightness(float starNum);
			float2 unity_gradientNoise_dir(float2 p);
			float unity_gradientNoise(float2 p);
			float gold_noise(float2 xy, float seed = 5);
			float random(float2 uv);
			inline float2 voronoi_noise_random_vector(float2 UV, float offset);
			VoronoiOutput voronoi(float2 UV, float AngleOffset, float CellDensity);
			float easeOutSine(float x);

			
			float _Altitude, _MaxAltitude;
			float _PlayerX;

			float4 vert(appdata_base v) : POSITION {
				return UnityObjectToClipPos(v.vertex);
			}

			float4 frag(float4 sp : VPOS) : SV_Target {
				float2 coord = sp.xy;
				sp /= float4(_ScreenParams.xy, 1, 1);

				float altitudeRatio = max(0, min(1, _Altitude / _MaxAltitude));

				// Background Color
				float earthBrightness = (1 - sp.y) / 2 + 1/2;
				float3 earthColor = float3(earthBrightness, earthBrightness, 1);
				
				float spaceBrightness = (1 - sp.y) / 4 + 0.5;
				float3 spaceColor = float3(spaceBrightness * 0.15, spaceBrightness * 0.15, spaceBrightness * 0.15);
				
				float spaceEarthColorRatio = altitudeRatio * 0.95;
				float3 skyColor = spaceEarthColorRatio * spaceColor + (1 - spaceEarthColorRatio) * earthColor;
				
				// Stars
				float2 starRelativePos = float2(coord.x + _PlayerX * STARS_PARALLAX_MULT, coord.y + _Altitude * STARS_PARALLAX_MULT);
				VoronoiOutput voronoiData = voronoi(starRelativePos / 50, 70, 6);
				float starValue = 1 - voronoiData.value;
				starValue = pow(starValue + 0.5, 10) / pow(1.5, 10);
				starValue = gold_noise(float2(voronoiData.cell, voronoiData.value)) < 0.0003 ? starValue : 0;
				starValue *= starBrightness(voronoiData.cell);
				starValue = max(0, starValue);
				starValue *= altitudeRatio;
				float3 starColor = starValue;

				// Sun
				float sunCenterDist = distance(coord.xy, _ScreenParams.xy);
				float sunRadius = _ScreenParams.y / 1.75;
				float sunValue = sunCenterDist < sunRadius ? 1 - sunCenterDist / sunRadius : 0;
				sunValue = easeOutSine(sunValue);
				sunValue *= max(0, 1 - easeOutSine(altitudeRatio) * 1.5);
				sunValue = min(1, max(0, sunValue));
				float3 sunOriginalColor = float3(0.9882352941176471, 0.8313725490196079, 0.25098039215686274);
				float3 sunColor = sunOriginalColor * sunValue;

				// Moon
				float moonCenterDist = distance(coord.xy, float2(_ScreenParams.x / 5, _ScreenParams.y * 0.65));
				float moonRadius = _ScreenParams.y / 2;
				float moonValue = moonCenterDist < moonRadius ? 1 - moonCenterDist / moonRadius : 0;
				moonValue = moonValue == 0 ? 0 : pow(moonValue + 0.5 + altitudeRatio / 4, 10) / pow(1.5 + altitudeRatio / 4, 10) * 5;
				moonValue *= 1 - max(0, 1 - easeOutSine(altitudeRatio) * 1.5);
				moonValue = max(0, min(1, max(0, moonValue)));
				float3 moonOriginalColor = float3(0.3, 0.3, 0.3);
				float moonPerlinValue = min(1,max(0,unity_gradientNoise(sp.xy * 30)/3));
				float moonPatternAlpha = min(1, max(0, 1 - max(35, moonCenterDist) / moonRadius * 3)) * easeOutSine(altitudeRatio);
				moonOriginalColor = moonOriginalColor * (1 - moonPatternAlpha) + (1 - moonPerlinValue) * moonPatternAlpha;
				float3 moonColor = moonOriginalColor * moonValue;

				// Clouds
				float2 cloudRelativePos = float2(sp.x + _Time.x * CLOUDS_WIND_SPEED + _PlayerX * CLOUDS_PARALLAX_MULT, sp.y + _Altitude * CLOUDS_PARALLAX_MULT);
				float cloudPerlinValue = unity_gradientNoise(cloudRelativePos * 4 * (_ScreenParams.y / 720));
				float cloudValue = pow(cloudPerlinValue + 0.5 + altitudeRatio / 4, 3) / pow(1.5 + altitudeRatio / 4, 3) * 2;
				cloudValue *= (1 - altitudeRatio);
				float3 cloudColor = cloudValue * (1 - sunValue * 6) * (1 - moonValue * 3) +
					sunOriginalColor * sunValue * 2;
				
				float4 finalValue = float4(
					skyColor * (1 - sunValue) * (1 - cloudValue) + 
					cloudColor + 
					starColor * max(1 - moonValue * 10, 0) * (1 - sunValue) +
					sunColor +
					moonColor
				, 1);
				return finalValue;
			}

			inline float4 starBrightness(float starNum) {
				starNum = random(starNum);
				float sinVal = sin((_Time / (starNum + 1) + starNum) * 100);
				float scaledSinVal = sinVal * (starNum + 1) - (starNum);
				return scaledSinVal;
			}

			// https://easings.net/#easeOutSine
			float easeOutSine(float x) {
				return sin((x * PI) / 2);
			}

			// https://stackoverflow.com/a/28095165
			float gold_noise(float2 xy, float seed) {
				return frac(tan(distance(xy * PHI, xy) * seed) * xy.x);
			}

			// https://stackoverflow.com/a/4275343
			float random(float2 uv)
			{
				return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453123);
			}

			// https://docs.unity3d.com/Packages/com.unity.shadergraph@6.9/manual/Gradient-Noise-Node.html
			float2 unity_gradientNoise_dir(float2 p)
			{
				p = p % 289;
				float x = (34 * p.x + 1) * p.x % 289 + p.y;
				x = (34 * x + 1) * x % 289;
				x = frac(x / 41) * 2 - 1;
				return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
			}

			float unity_gradientNoise(float2 p)
			{
				float2 ip = floor(p);
				float2 fp = frac(p);
				float d00 = dot(unity_gradientNoise_dir(ip), fp);
				float d01 = dot(unity_gradientNoise_dir(ip + float2(0, 1)), fp - float2(0, 1));
				float d10 = dot(unity_gradientNoise_dir(ip + float2(1, 0)), fp - float2(1, 0));
				float d11 = dot(unity_gradientNoise_dir(ip + float2(1, 1)), fp - float2(1, 1));
				fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
				return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x);
			}

			// https://docs.unity3d.com/Packages/com.unity.shadergraph@6.9/manual/Voronoi-Node.html
			VoronoiOutput voronoi(float2 UV, float AngleOffset, float CellDensity)
			{
				float2 g = floor(UV * CellDensity);
				float2 f = frac(UV * CellDensity);
				float t = 8.0;
				float3 res = float3(8.0, 0.0, 0.0);

				float val = 0, cells = 0;

				for (int y = -1; y <= 1; y++)
				{
					for (int x = -1; x <= 1; x++)
					{
						float2 lattice = float2(x, y);
						float2 offset = voronoi_noise_random_vector(lattice + g, AngleOffset);
						float d = distance(lattice + offset, f);
						if (d < res.x)
						{
							res = float3(d, offset.x, offset.y);
							val = res.x;
							cells = res.z;
						}
					}
				}
				
				VoronoiOutput o;
				o.value = val;
				o.cell = abs(cells);
				return o;
			}

			inline float2 voronoi_noise_random_vector(float2 UV, float offset)
			{
				float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
				UV = frac(sin(mul(UV, m)) * 46839.32);
				return float2(sin(UV.y * +offset) * 0.5 + 0.5, cos(UV.x * offset) * 0.5 + 0.5);
			}

			ENDCG
		}
	}
}