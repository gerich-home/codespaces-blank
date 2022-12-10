#pragma once


namespace Shapes
{
	using namespace Engine;

	class Triangle: IShape
	{
		Triangle(Vector a, Vector b, Vector c, IMaterial material);

		HitPoint Intersection(Vector start, Vector direction) const;

		public readonly Vector normal;
		public readonly Vector n;
		public readonly Vector ba;
		public readonly Vector ca;
		public readonly Vector a;
		public readonly IMaterial material;
	};
}