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
		var (cosa, sina) = rnd.NextCosDistribution();
		double b = rnd.NextDouble(2 * Math.PI);

		Vector normal = new Vector(cosa * Math.Cos(b), cosa * Math.Sin(b), sina);

		return new LightPoint(center + r * normal, normal, probability, le);
	}

	public Photon[] EmitPhotons(int nphotons)
	{
		Photon[] photons = new Photon[nphotons];
		Luminance energy = le / nphotons;
		for(int i = 0; i < nphotons; i++)
        {
            var (cosa, sina) = rnd.NextSinDistribution();
            double b = rnd.NextDouble(2 * Math.PI);

            Vector normal = new Vector(cosa * Math.Cos(b), cosa * Math.Sin(b), sina);
            (cosa, sina) = rnd.NextCosDistribution();
            b = rnd.NextDouble(2 * Math.PI);

            Vector direction = new Vector(sina * Math.Cos(b), sina * Math.Sin(b), cosa).Transform(normal);

            photons[i] = new Photon((center + r * normal).RayAlong(direction), normal, energy);
        }

        return photons;
	}

    public Luminance Le => le;
}
