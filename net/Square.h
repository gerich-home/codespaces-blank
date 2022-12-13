#pragma once


namespace Shapes
{
	using namespace Engine;

	class Square: public IShape
	{
		Square(Vector a, Vector b, Vector c, ITexturedMaterial material);
		Square(Vector a, Vector b, Vector c, IMaterial material);

		HitPoint Intersection(Vector start, Vector direction);

	private:
		readonly Vector a;
		readonly Vector ba;
		readonly Vector ca;
		readonly Vector normal;
		readonly Vector n;
		ITexturedMaterial material;
	};
}
