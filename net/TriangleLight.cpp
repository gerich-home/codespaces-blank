using namespace Engine;

Lights::Triangle::Triangle(const Vector a, const Vector b, const Vector c, Luminance Le): 
	a(a),
	ba(b - a),
	ca(c - a),
	normal((b - a).CrossProduct(c - a).Normalize()),
	probability(2 / (b - a).CrossProduct(c - a).Length()),
	le(le)
{
}

const LightPoint Lights::Triangle::SampleLightPoint(Vector point) const
{
	double t1 = (double) rand() / RAND_MAX;
	double t2 = (double) rand() / RAND_MAX;

	if(t1 + t2 > 1)
	{
		t1 = 1 - t1;
		t2 = 1 - t2;
	}

	Vector p = a + t1 * ba + t2 * ca;
	return LightPoint(p, normal, probability, le);
}

void Lights::Triangle::EmitPhotons(int nphotons, Photon photons[]) const
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

Luminance Lights::Triangle::Le() const 
{
	return le;
}