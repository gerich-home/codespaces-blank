using Engine;

namespace Lights;

public class SphereLight : ILightSource
{
	public readonly double r;
	public readonly double probability;
	public readonly Vector center;
	public readonly Luminance le;
	public readonly Random rnd;
	public static readonly double TwoPi = 2 * Math.PI;

	public SphereLight(Random rnd, Vector center, double r, Luminance le)
	{
		this.rnd = rnd;
		this.center = center;
		this.r = r;
		this.probability = 1 / (4 * Math.PI * r * r);
		this.le = le;
	}

	public bool CanSendLightTo(HitPoint hitPoint)
	{
		var hc = hitPoint.Point - center;

		return hitPoint.Normal.DotProduct(hc) > -r;
	}

	public LightPoint SampleLightPoint(HitPoint hitPoint)
	{	
		var (cosa, sina) = rnd.NextCosDistribution();
		var b = rnd.NextDouble(TwoPi);
		var (sinb, cosb) = Math.SinCos(b);

		var normal = new Vector(
			cosa * cosb,
			cosa * sinb,
			sina
		);

		return new LightPoint(
			center + r * normal,
			normal,
			probability,
			le
		);
	}

	public Photon[] EmitPhotons(int nphotons)
	{
		Photon[] photons = new Photon[nphotons];
		Luminance energy = le / nphotons;
		for(int i = 0; i < nphotons; i++)
        {
            var (cosa, sina) = rnd.NextSinDistribution();
            var b = rnd.NextDouble(TwoPi);
			var (sinb, cosb) = Math.SinCos(b);

            var normal = new Vector(
				cosa * cosb,
				cosa * sinb,
				sina
			);

            (cosa, sina) = rnd.NextCosDistribution();
            b = rnd.NextDouble(TwoPi);
			(sinb, cosb) = Math.SinCos(b);

            var direction = new Vector(
				sina * cosb,
				sina * sinb,
				cosa
			).Transform(normal);

            photons[i] = new Photon((center + r * normal).RayAlong(direction), normal, energy);
        }

        return photons;
	}

    public Luminance Le => le;
}
