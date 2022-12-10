
#define _USE_MATH_DEFINES

using namespace Engine;

Lights::Sphere::Sphere(Vector center, double r, Luminance le) :
	center(center),
	r(r),
	probability(1 / (4 * M_PI * r * r)),
	le(le)
{
}

const LightPoint Lights::Sphere::SampleLightPoint(Vector point) const
{	
	double cosa = (double) rand() / RAND_MAX;
	double sina = sqrt(1 - cosa * cosa);
	double b = 2 * M_PI * (double) rand() / RAND_MAX;

	const Vector normal = Vector(cosa * cos(b), cosa * sin(b), sina);

	return LightPoint(center + r * normal, normal, probability, le);
}

void Lights::Sphere::EmitPhotons(int nphotons, Photon photons[]) const
{
	Luminance energy = le / nphotons;
	for(int i = 0; i < nphotons; i++)
	{		
		double sina = (double) rand() / RAND_MAX;
		double cosa = sqrt(1 - sina * sina);
		double b = 2 * M_PI * (double) rand() / RAND_MAX;

		const Vector normal = Vector(cosa * cos(b), cosa * sin(b), sina);
		
		cosa = (double) rand() / RAND_MAX;
		sina = sqrt(1 - cosa * cosa);
		b = 2 * M_PI * (double) rand() / RAND_MAX;

		Vector direction = Vector(sina * cos(b), sina * sin(b), cosa).Transform(normal);

		photons[i] = Photon(center + r * normal, normal, direction, energy);
	}
}

Luminance Lights::Sphere::Le() const 
{
	return le;
}
