using Engine;

namespace Engines;

public record class PhotonMapTracingEngine(
	PhotonMap globalMap,
	PhotonMap causticsMap,
	SceneSetup sceneSetup
): IEngine {
	const int SHADOW_RAYS = 10;
	const double ABSOPTION = 0.01;

	public Luminance L(in Ray ray)
	{
		HitPoint hp = sceneSetup.scene.Intersection(null, ray);
	
		if (hp == null)
		{
			return Luminance.Zero;
		}

		Luminance result = Luminance.Zero;
		Luminance factor = new Luminance(1, 1, 1);
		Vector current_point = hp.Point;
		Vector current_direction = hp.IncomingDirection;
		HitPoint current_hp = hp;

		while(true)
		{
			Luminance direct = Luminance.Zero;

			for(int i = 0; i < SHADOW_RAYS; i++)
			{	
				LightPoint lp = sceneSetup.lights.SampleLightPoint(hp);
				Vector directionToLight = lp.point - current_point;

				double cos_dir_normal = current_hp.Normal.DotProduct(directionToLight);

				if(cos_dir_normal < 0)
				{
					continue;
				}

				double cos_dir_lnormal = -(directionToLight.DotProduct(lp.normal));
				if(cos_dir_lnormal < 0)
				{
					continue;
				}

				double l = directionToLight.Length;
				if(l * l < double.Epsilon)
				{
					continue;
				}

				double linv = 1 / l;
				directionToLight *= linv;
				cos_dir_normal *= linv;
				cos_dir_lnormal *= linv;

				HitPoint nhp = sceneSetup.scene.Intersection(hp.shape, current_point.RayAlong(directionToLight));

				if(nhp != null)
				{
					if(nhp.T <= l - double.Epsilon)
					{
						continue;
					}
				}

				direct += lp.factor * lp.Le * current_hp.BRDF(directionToLight) * (cos_dir_normal * cos_dir_lnormal / (l * l));
			}
			direct /= SHADOW_RAYS;
			
			result += factor * direct;
			
			//Compute indirect luminancy
			
			RandomDirection rndd = current_hp.SampleDirection(ABSOPTION);
				
			// the whole file was commented before
			//if(rndd.hp == null)
			//{
			//	break;
			//}

			if(rndd.factor.IsZero)
			{
				break;
			}

			factor *= rndd.factor;
			
			current_direction = rndd.directionToLight;
			// the whole file was commented before
			//current_hp = rndd.hp;
			//current_point += current_direction * current_hp.t;
		}

		return result;
	}
}
