using Engine;

namespace Lights;

public class SphereLight : ILightSource
{
	public readonly double r;
	public readonly double r2;
	public readonly double factor;
	public readonly Vector center;
	public readonly Luminance le;
	public readonly Random rnd;
	public static readonly double TwoPi = 2 * Math.PI;

	public SphereLight(Random rnd, Vector center, double r, Luminance le)
	{
		this.rnd = rnd;
		this.center = center;
		this.r = r;
		this.r2 = r * r;
		this.factor = 4 * Math.PI * r * r;
		this.le = le;
	}

	public bool CanSendLightTo(HitPoint hitPoint)
	{
		var hc = hitPoint.Point - center;

		return hitPoint.Normal.DotProduct(hc) > -r;
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
}
