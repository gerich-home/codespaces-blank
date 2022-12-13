
#define _USE_MATH_DEFINES

using namespace Engine;

Lights.Sphere.Sphere(Vector center, double r, Luminance le) :
	center(center),
	r(r),
	probability(1 / (4 * Math.PI * r * r)),
	le(le)
{
}

readonly LightPoint Lights.Sphere.SampleLightPoint(Vector point)
{	
	double cosa = rnd.NextDouble();
	double sina = Math.Sqrt(1 - cosa * cosa);
	double b = 2 * Math.PI * rnd.NextDouble();

	readonly Vector normal = new Vector(cosa * Math.Cos(b), cosa * Math.Sin(b), sina);

	return new LightPoint(center + r * normal, normal, probability, le);
}

void Lights.Sphere.EmitPhotons(int nphotons, Photon photons[])
{
	Luminance energy = le / nphotons;
	for(int i = 0; i < nphotons; i++)
	{		
		double sina = rnd.NextDouble();
		double cosa = Math.Sqrt(1 - sina * sina);
		double b = 2 * Math.PI * rnd.NextDouble();

		readonly Vector normal = new Vector(cosa * Math.Cos(b), cosa * Math.Sin(b), sina);
		
		cosa = rnd.NextDouble();
		sina = Math.Sqrt(1 - cosa * cosa);
		b = 2 * Math.PI * rnd.NextDouble();

		Vector direction = new Vector(sina * Math.Cos(b), sina * Math.Sin(b), cosa).Transform(normal);

		photons[i] = Photon(center + r * normal, normal, direction, energy);
	}
}

Luminance Lights.Sphere.Le() 
{
	return le;
}
