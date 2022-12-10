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

		const Luminance BRDF(Vector direction, Vector ndirection, Vector normal) const
		{
			return Luminance();
		}

		const RandomDirection SampleDirection(Vector direction, Vector normal, double ksi) const
		{
			const Vector R = direction - 2 * normal.DotProduct(direction) * normal;

			return RandomDirection(rs, R);
		}

	private:
		const Luminance rs; //koefficient specular reflection
	};
}