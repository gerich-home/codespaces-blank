#pragma once


#define _USE_MATH_DEFINES

using namespace Engine;

namespace Materials
{
	class IdealMirrorMaterial: public IMaterial
	{
		IdealMirrorMaterial(double rs[]) :
			rs(rs)
		{
		}

		Luminance BRDF(Vector direction, Vector ndirection, Vector normal)
		{
			return new Luminance();
		}

		RandomDirection SampleDirection(Vector direction, Vector normal, double ksi)
		{
			readonly Vector R = direction - 2 * normal.DotProduct(direction) * normal;

			return new RandomDirection(rs, R);
		}

	private:
		readonly Luminance rs; //koefficient specular reflection
	};
}