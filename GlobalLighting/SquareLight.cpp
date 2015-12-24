#include "StdAfx.h"
#include "SquareLight.h"

using namespace Engine;

Lights::Square::Square(const Vector a, const Vector b, const Vector c, const Luminance& Le):
	a(a),
	ba(b - a),
	ca(c - a),
	normal((b - a).CrossProduct(c - a).Normalize()),
	probability(1 / (b - a).CrossProduct(c - a).Length()),
	Le(Le)
{
}

const LightPoint Lights::Square::SampleLightPoint(const Vector& point) const
{
	GO_FLOAT t1 = (GO_FLOAT) rand() / RAND_MAX;
	GO_FLOAT t2 = (GO_FLOAT) rand() / RAND_MAX;

	Vector p = a + t1 * ba + t2 * ca;
	return LightPoint(p, (point - p).Normalize(), probability, Le);
}