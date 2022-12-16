using Engine;

namespace Engines;

public record class PhotonMapTracingEngine(
	PhotonMap globalMap,
	PhotonMap causticsMap,
	SceneSetup sceneSetup
): IEngine {
	const int SHADOW_RAYS = 10;
	const double ABSOPTION = 0.01;

	public Luminance L(Ray ray)
	{
		HitPoint hp = sceneSetup.scene.Intersection(ray);
	
		if (hp == null)
		{
			return Luminance.Zero;
		}

		Luminance result = Luminance.Zero;
		Luminance factor = new Luminance(1, 1, 1);
		Vector current_point = hp.hitPoint;
		Vector current_direction = hp.ray.direction;
		HitPoint current_hp = hp;

		while(true)
		{
			Luminance direct = Luminance.Zero;

			for(int i = 0; i < SHADOW_RAYS; i++)
			{	
				LightPoint lp = sceneSetup.lights.SampleLightPoint();
				Vector ndirection = lp.point - current_point;

				double cos_dir_normal = current_hp.normal.DotProduct(ndirection);

				if(cos_dir_normal < 0)
				{
					continue;
				}

				double cos_dir_lnormal = -(ndirection.DotProduct(lp.normal));
				if(cos_dir_lnormal < 0)
				{
					continue;
				}

				double l = ndirection.Length;
				if(l * l < double.Epsilon)
				{
					continue;
				}

				double linv = 1 / l;
				ndirection *= linv;
				cos_dir_normal *= linv;
				cos_dir_lnormal *= linv;

				HitPoint nhp = sceneSetup.scene.Intersection(current_point.RayAlong(ndirection));

				if(nhp != null)
				{
					if(nhp.t <= l - double.Epsilon)
					{
						continue;
					}
				}

				direct += lp.Le * current_hp.material.BRDF(current_direction, ndirection, current_hp.normal) * (cos_dir_normal * cos_dir_lnormal / (lp.probability * l * l));
			}
			direct /= SHADOW_RAYS;
			
			result += factor * direct;
			
			//Compute indirect luminancy
			
			RandomDirection rndd = current_hp.material.SampleDirection(current_direction, current_hp.normal, ABSOPTION);
				
			// the whole file was commented before
			//if(rndd.hp == null)
			//{
			//	break;
			//}

			if(rndd.factor.r == 0 && rndd.factor.g == 0 && rndd.factor.b == 0)
			{
				break;
			}

			factor *= rndd.factor;
			
			current_direction = rndd.direction;
			// the whole file was commented before
			//current_hp = rndd.hp;
			//current_point += current_direction * current_hp.t;
		}

		return result;
	}
}
