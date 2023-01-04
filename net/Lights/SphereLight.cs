using Engine;

namespace Lights;

public class SphereLight : ILightSource
{
	public readonly double r;
	public readonly double r2;
	public readonly double factor;
	public readonly AABB aabb;
	public readonly Vector center;
	public readonly Luminance le;
	public readonly Luminance energy;
	public readonly Random rnd;

	public SphereLight(Random rnd, Vector center, double r, Luminance le)
	{
		this.rnd = rnd;
		this.center = center;
		this.r = r;
		this.r2 = r * r;
		this.factor = 2 * Math.PI * r * r;
		this.le = le;
		this.energy = le * factor;
		aabb = new AABB(
			center - Vector.Unit * r,
			center + Vector.Unit * r
		);
	}

	public ref readonly AABB AABB => ref aabb;

	public bool CanSendLightTo(HitPoint hitPoint)
	{
		var directionToLight = center - hitPoint.Point;

		return directionToLight.DotProduct(hitPoint.Normal) > -r;
	}

	public LightPoint SampleLightPoint(HitPoint hitPoint)
    {
		var directionFromLight = hitPoint.Point - center;
        var normal = rnd.NextSemisphereDirectionCos(r2 / directionFromLight.Norm)
			.Transform(directionFromLight.Normalized);

        return new LightPoint(
            center + r * normal,
            normal,
            factor,
            le
        );
    }

    public Photon[] EmitPhotons(int nphotons)
	{
		Photon[] photons = new Photon[nphotons];
		Luminance energy = le / nphotons;
		for(int i = 0; i < nphotons; i++)
        {
            var normal = rnd.NextSemisphereDirectionUniform();
            var direction = rnd.NextSemisphereDirectionUniform()
				.Transform(normal);

            photons[i] = new Photon((center + r * normal).RayAlong(direction), normal, energy);
        }

        return photons;
	}

    public Luminance Le => le;
    public Luminance Energy => energy;
}
