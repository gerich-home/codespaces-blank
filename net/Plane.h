#pragma once


namespace Shapes
{
	using namespace Engine;

	class Plane : public IShape
	{
		Plane(Vector normal, double d, IMaterial material);
		Plane(Vector a, Vector b, Vector A, IMaterial material);
		Plane(double a, double b, double c, double d, IMaterial material);

		HitPoint Intersection(Vector start, Vector direction);

	private:
		double d;
		readonly Vector normal;
		readonly Vector A;
		IMaterial material;
	};
};