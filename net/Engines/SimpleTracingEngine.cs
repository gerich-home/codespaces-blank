using Engine;

namespace Engines;

public record class SimpleTracingEngine(
	Random rnd,
	SceneSetup sceneSetup
): IEngine {
	public const int SHADOW_RAYS = 10;
	public const double ABSOPTION = 0.3;

	public Luminance L(in Ray ray) =>
		LuminanceComponents(ray).Aggregate(Luminance.Zero, (l1, l2) => l1 + l2);

	private IEnumerable<Luminance> LuminanceComponents(Ray ray)
	{
		HitPoint hp;
		
		hp = sceneSetup.scene.Intersection(ray);
	
		if (hp == null)
		{
			yield break;
		}

		Luminance factor = new Luminance(1, 1, 1);

		while(true)
        {
			yield return factor * ComputeDirectLuminance(hp);

            double ksi = rnd.NextDouble();

            if (ksi < ABSOPTION)
            {
                yield break;
            }

            ksi = (ksi - ABSOPTION) / (1 - ABSOPTION);

            var rndd = hp.SampleDirection(ksi);

            if (rndd.factor.IsZero)
            {
                yield break;
            }

            hp = sceneSetup.scene.Intersection(hp.RayAlong(rndd.direction));

            if (hp == null)
            {
                yield break;
            }

            factor *= rndd.factor / (1 - ABSOPTION);
        }
	}

    private Luminance ComputeDirectLuminance(HitPoint hp)
    {
        Luminance result = Luminance.Zero;

        for (int i = 0; i < SHADOW_RAYS; i++)
        {
			result += ComputeDirectLuminanceForSingleRay(hp);
        }

        return result / SHADOW_RAYS;
    }
	
    private Luminance ComputeDirectLuminanceForSingleRay(HitPoint hp)
    {
		var lp = sceneSetup.lights.SampleLightPoint();
		var ndirection = lp.point - hp.hitPoint;

		double cos_dir_normal = hp.normal.DotProduct(ndirection);

		if (cos_dir_normal <= 0)
		{
			return Luminance.Zero;
		}

		double cos_dir_lnormal = -(ndirection.DotProduct(lp.normal));
		if (cos_dir_lnormal <= 0)
		{
			return Luminance.Zero;
		}

		double l = ndirection.Length;
		if (l * l < double.Epsilon)
		{
			return Luminance.Zero;
		}

		double linv = 1 / l;
		ndirection *= linv;
		cos_dir_normal *= linv;
		cos_dir_lnormal *= linv;

		var barrierHp = sceneSetup.scene.Intersection(hp.RayAlong(ndirection));

		if (barrierHp != null && (barrierHp.t <= l - double.Epsilon))
		{
			return Luminance.Zero;
		}

		return lp.Le * hp.BRDF(ndirection) * (cos_dir_normal * cos_dir_lnormal / (lp.probability * l * l));
    }
}
