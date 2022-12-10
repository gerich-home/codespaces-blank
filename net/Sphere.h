#pragma once


namespace Shapes
{
	using namespace Engine;

	class Sphere : public IShape
	{
		Sphere(Vector center, double r, IMaterial material);
	
		HitPoint Intersection(Vector start, Vector direction) const;

	private:
		const double r2;
		const double rinv;
		const Vector center;
		IMaterial material;
	};
}