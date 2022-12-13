#pragma once


namespace Shapes
{
	using namespace Engine;

	class Sphere : public IShape
	{
		Sphere(Vector center, double r, IMaterial material);
	
		HitPoint Intersection(Vector start, Vector direction);

	private:
		readonly double r2;
		readonly double rinv;
		readonly Vector center;
		IMaterial material;
	};
}