using Engine;

namespace Lights;

public class Triangle : ILightSource
{
	public readonly double factor;
	public readonly Vector a;
	public readonly Vector b;
	public readonly Vector c;
	public readonly Vector normal;
	public readonly Luminance le;
	public readonly Luminance energy;
	public readonly Random rnd;
	public readonly Vector ba;
	public readonly Vector ca;
	public readonly AABB aabb;

	public Triangle(Random rnd, Vector a, Vector b, Vector c, Luminance le)
	{
		this.rnd = rnd;
		this.a = a;
		this.b = b;
		this.c = c;
		this.ba = b - a;
		this.ca = c - a;
		this.normal = ba.CrossProduct(ca).Normalized;
		this.factor = (ba).CrossProduct(ca).Length / 2;
		this.le = le;
		this.energy = le * factor;
		aabb = AABB.FromEdgePoints(
			a,
			b, 
			c 
		);
	}

	public ref readonly AABB AABB => ref aabb;

	public bool CanSendLightTo(HitPoint hitPoint)
	{
		var ha = hitPoint.Point - a;

		return normal.DotProduct(ha) > 0 && (
			hitPoint.Normal.DotProduct(ha) < 0 ||
			hitPoint.Normal.DotProduct(hitPoint.Point - b) < 0 ||
			hitPoint.Normal.DotProduct(hitPoint.Point - c) < 0
		);
	}

	public LightPoint SampleLightPoint(HitPoint hitPoint)
	{
		double t1 = rnd.NextDouble();
		double t2 = rnd.NextDouble();

		if(t1 + t2 > 1)
		{
			t1 = 1 - t1;
			t2 = 1 - t2;
		}

		Vector p = a + t1 * ba + t2 * ca;
		return new LightPoint(p, normal, factor, le);
	}

	public Photon[] EmitPhotons(int nphotons)
	{
		Photon[] photons = new Photon[nphotons];
		Luminance energy = le / nphotons;
		for(int i = 0; i < nphotons; i++)
		{
			double t1 = rnd.NextDouble();
			double t2 = rnd.NextDouble();

            var direction = rnd.NextSemisphereDirectionUniform()
				.Transform(normal);

			photons[i] = new Photon((a + t1 * ba + t2 * ca).RayAlong(direction), normal, energy);
		}
		return photons;
	}

    public Luminance Le => le;
    public Luminance Energy => energy;
}
