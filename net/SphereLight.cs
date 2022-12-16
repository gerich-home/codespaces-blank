using Engine;

namespace Lights;

public class SphereLight : ILightSource
{
	public readonly double r;
	public readonly double probability;
	public readonly Vector center;
	public readonly Luminance le;
	public readonly Random rnd;

	public SphereLight(Random rnd, Vector center, double r, Luminance le)
	{
		this.rnd = rnd;
		this.center = center;
		this.r = r;
		this.probability = 1 / (4 * Math.PI * r * r);
		this.le = le;
	}

	public LightPoint SampleLightPoint()
	{	
		double cosa = rnd.NextDouble();
		double sina = Math.Sqrt(1 - cosa * cosa);
		double b = 2 * Math.PI * rnd.NextDouble();

		Vector normal = new Vector(cosa * Math.Cos(b), cosa * Math.Sin(b), sina);

		return new LightPoint(center + r * normal, normal, probability, le);
	}

	public Photon[] EmitPhotons(int nphotons)
	{
		Photon[] photons = new Photon[nphotons];
		Luminance energy = le / nphotons;
		for(int i = 0; i < nphotons; i++)
		{		
			double sina = rnd.NextDouble();
			double cosa = Math.Sqrt(1 - sina * sina);
			double b = 2 * Math.PI * rnd.NextDouble();

			Vector normal = new Vector(cosa * Math.Cos(b), cosa * Math.Sin(b), sina);
			
			cosa = rnd.NextDouble();
			sina = Math.Sqrt(1 - cosa * cosa);
			b = 2 * Math.PI * rnd.NextDouble();

			Vector direction = new Vector(sina * Math.Cos(b), sina * Math.Sin(b), cosa).Transform(normal);

			photons[i] = new Photon(center + r * normal, normal, direction, energy);
		}

		return photons;
	}

    public Luminance Le => le;
}
