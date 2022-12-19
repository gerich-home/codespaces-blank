using Engine;

namespace Lights;

public class SquareLight : ILightSource
{
	public readonly Vector a;
	public readonly Vector b;
	public readonly Vector c;
	public readonly Vector d;
	public readonly Vector ba;
	public readonly Vector ca;
	public readonly Vector normal;
	public readonly double probability;
	public readonly Luminance le;
	public readonly Random rnd;
	public static readonly double TwoPi = 2 * Math.PI;

	public SquareLight(Random rnd, Vector a, Vector b, Vector c, Luminance le)
	{
		this.rnd = rnd;
		this.a = a;
		this.b = b;
		this.c = c;
		this.ba = b - a;
		this.ca = c - a;
		this.d = b + ca;
		this.normal = ba.CrossProduct(ca).Normalized;
		this.probability = 1 / (ba).CrossProduct(ca).Length;
		this.le = le;
	}

	public bool CanSendLightTo(HitPoint hitPoint)
	{
		var ha = hitPoint.Point - a;

		return normal.DotProduct(ha) > 0 && (
			hitPoint.Normal.DotProduct(ha) < 0 ||
			hitPoint.Normal.DotProduct(hitPoint.Point - d) < 0 ||
			hitPoint.Normal.DotProduct(hitPoint.Point - b) < 0 ||
			hitPoint.Normal.DotProduct(hitPoint.Point - c) < 0
		);
	}

	public LightPoint SampleLightPoint(HitPoint hitPoint)
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
			
			var (cosa, sina) = rnd.NextCosDistribution();
			var b = rnd.NextDouble(TwoPi);
			var (sinb, cosb) = Math.SinCos(b);

            var direction = new Vector(
				sina * cosb,
				sina * sinb,
				cosa
			).Transform(normal);

			photons[i] = new Photon((a + t1 * ba + t2 * ca).RayAlong(direction), normal, energy);
		}

		return photons; 
	}

    public Luminance Le => le;
}
