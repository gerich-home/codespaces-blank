using Engine;

namespace Lights;

public class SquareLight : ILightSource
{
	public readonly Vector a;
	public readonly Vector ba;
	public readonly Vector ca;
	public readonly Vector normal;
	public readonly double probability;
	public readonly Luminance le;
	public readonly Random rnd;

	public SquareLight(Random rnd, Vector a, Vector b, Vector c, Luminance le)
	{
		this.rnd = rnd;
		this.a = a;
		this.ba = b - a;
		this.ca = c - a;
		this.normal = (b - a).CrossProduct(c - a).Normalized;
		this.probability = 1 / (b - a).CrossProduct(c - a).Length;
		this.le = le;
	}

	public LightPoint SampleLightPoint()
	{
		double t1 = rnd.NextDouble();
		double t2 = rnd.NextDouble();

		return new LightPoint(a + t1 * ba + t2 * ca, normal, probability, le);
	}

	public Photon[] EmitPhotons(int nphotons)
	{
		Photon[] photons = new Photon[nphotons];
		Luminance energy = le / nphotons;
		for(int i = 0; i < nphotons; i++)
		{
			double t1 = rnd.NextDouble();
			double t2 = rnd.NextDouble();
			
			double cosa = rnd.NextDouble();
			double sina = Math.Sqrt(1 - cosa * cosa);
			double b = 2 * Math.PI * rnd.NextDouble();

			Vector direction = new Vector(sina * Math.Cos(b), sina * Math.Sin(b), cosa).Transform(normal);

			photons[i] = new Photon(a + t1 * ba + t2 * ca, normal, direction, energy);
		}

		return photons; 
	}

    public Luminance Le => le;
}
