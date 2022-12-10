#pragma once


namespace Shapes
{
	using namespace Engine;

	class Square: public IShape
	{
		Square(const Vector a, const Vector b, const Vector c, ITexturedMaterial material);
		Square(const Vector a, const Vector b, const Vector c, IMaterial material);

		HitPoint Intersection(Vector start, Vector direction) const;

	private:
		const Vector a;
		const Vector ba;
		const Vector ca;
		const Vector normal;
		const Vector n;
		ITexturedMaterial material;
	};
}
