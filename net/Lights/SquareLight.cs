using Engine;
using Shapes;

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
	public readonly AABB aabb;
	public readonly double factor;
	public readonly Luminance le;
	public readonly Luminance energy;
	public readonly Random rnd;
	public readonly Square shape;

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
		this.factor = (ba).CrossProduct(ca).Length;
		this.le = le;
		this.energy = le * factor;
		this.shape = new Square(a, b, c);
		aabb = shape.AABB;
	}

	public ref readonly AABB AABB => ref aabb;

	public bool CanSendLightTo(BodyHitPoint hitPoint)
	{
		var ha = hitPoint.Point - a;

		return normal.DotProduct(ha) > 0 && (
			hitPoint.Normal.DotProduct(ha) < 0 ||
			hitPoint.Normal.DotProduct(hitPoint.Point - d) < 0 ||
			hitPoint.Normal.DotProduct(hitPoint.Point - b) < 0 ||
			hitPoint.Normal.DotProduct(hitPoint.Point - c) < 0
		);
	}

	public LightPoint SampleLightPoint(BodyHitPoint hitPoint)
	{
		double t1 = rnd.NextDouble();
		double t2 = rnd.NextDouble();

		return new LightPoint(a + t1 * ba + t2 * ca, normal, factor, le);
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
	
	public LightHitPoint Intersection(in Ray ray)
	{
		var shapeHitPoint = shape.Intersection(ray);

		if(shapeHitPoint == null) 
		{
			return null;
		}

		return new LightHitPoint(shapeHitPoint, Le);
	}
}
