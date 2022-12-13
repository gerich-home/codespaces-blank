
#define _USE_MATH_DEFINES

using namespace Engine;

Lights.Square.Square(Vector a, Vector b, Vector c, Luminance le):
	a(a),
	ba(b - a),
	ca(c - a),
	normal((b - a).CrossProduct(c - a).Normalized),
	probability(1 / (b - a).CrossProduct(c - a).Length),
	le(le)
{
}

readonly LightPoint Lights.Square.SampleLightPoint(Vector point)
{
	double t1 = rnd.NextDouble();
	double t2 = rnd.NextDouble();

	return new LightPoint(a + t1 * ba + t2 * ca, normal, probability, le);
}

void Lights.Square.EmitPhotons(int nphotons, Photon photons[])
{
	Luminance energy = le / nphotons;
	for(int i = 0; i < nphotons; i++)
	{
		double t1 = rnd.NextDouble();
		double t2 = rnd.NextDouble();
		
		double cosa = rnd.NextDouble();
		double sina = Math.Sqrt(1 - cosa * cosa);
		double b = 2 * Math.PI * rnd.NextDouble();

		Vector direction = new Vector(sina * Math.Cos(b), sina * Math.Sin(b), cosa).Transform(normal);

		photons[i] = Photon(a + t1 * ba + t2 * ca, normal, direction, energy);
	}
}

Luminance Lights.Square.Le() 
{
	return le;
}