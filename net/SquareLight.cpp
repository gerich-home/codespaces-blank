
#define _USE_MATH_DEFINES

using namespace Engine;

Lights::Square::Square(const Vector a, const Vector b, const Vector c, Luminance le):
	a(a),
	ba(b - a),
	ca(c - a),
	normal((b - a).CrossProduct(c - a).Normalize()),
	probability(1 / (b - a).CrossProduct(c - a).Length()),
	le(le)
{
}

const LightPoint Lights::Square::SampleLightPoint(Vector point) const
{
	double t1 = (double) rand() / RAND_MAX;
	double t2 = (double) rand() / RAND_MAX;

	return LightPoint(a + t1 * ba + t2 * ca, normal, probability, le);
}

void Lights::Square::EmitPhotons(int nphotons, Photon photons[]) const
{
	Luminance energy = le / nphotons;
	for(int i = 0; i < nphotons; i++)
	{
		double t1 = (double) rand() / RAND_MAX;
		double t2 = (double) rand() / RAND_MAX;
		
		double cosa = (double) rand() / RAND_MAX;
		double sina = sqrt(1 - cosa * cosa);
		double b = 2 * M_PI * (double) rand() / RAND_MAX;

		Vector direction = Vector(sina * cos(b), sina * sin(b), cosa).Transform(normal);

		photons[i] = Photon(a + t1 * ba + t2 * ca, normal, direction, energy);
	}
}

Luminance Lights::Square::Le() const 
{
	return le;
}