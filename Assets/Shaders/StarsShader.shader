Shader "Custom/StarsShader"
{
	Properties
	{
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

			inline float4 star_brightness(float starNum);
			float gold_noise(float2 xy, float seed = 5);
			float random(float2 uv);
			float2 perlin_noise_dir(float2 p);
			float perlin_noise(float2 p);
			inline float2 voronoi_noise_random_vector(float2 UV, float offset);
			VoronoiOutput voronoi(float2 UV, float AngleOffset, float CellDensity);
			float easeOutSine(float x);

			float4 vert(appdata_base v) : POSITION {
				return UnityObjectToClipPos(v.vertex);
			}

			float4 frag(float4 sp : VPOS) : SV_Target {
				float2 coord = sp.xy;
				sp /= float4(_ScreenParams.xy, 1, 1);

				float spaceBrightness = (1 - sp.y) / 4 + 0.5;
				float3 skyColor = float3(spaceBrightness * 0.15, spaceBrightness * 0.15, spaceBrightness * 0.15);

				// Stars
				float2 starRelativePos = float2(coord.x, coord.y);
				VoronoiOutput voronoiData = voronoi(starRelativePos / 50, 70, 6);
				float starValue = 1 - voronoiData.value;
				starValue = pow(starValue + 0.5, 10) / pow(1.5, 10);
				starValue = gold_noise(float2(voronoiData.cell, voronoiData.value)) < 0.0003 ? starValue : 0;
				starValue *= star_brightness(voronoiData.cell);
				starValue = max(0, starValue);
				float3 starColor = starValue;

				float4 finalValue = float4(skyColor + starColor, 1);
				return finalValue;
			}

			inline float4 star_brightness(float starNum) {
				starNum = random(starNum);
				float sinVal = sin((_Time / 2 / (starNum + 1) + starNum) * 100);
				float scaledSinVal = sinVal * (starNum + 1) - (starNum);
				return scaledSinVal;
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