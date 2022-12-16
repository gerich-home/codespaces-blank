using Engine;

namespace Engines;

public class SimpleTracing : IEngine
{
	public const int SHADOW_RAYS = 10;
	public const double ABSOPTION = 0.3;

	public Luminance L(Random rnd, HitPoint hp, Vector point, Vector direction, IShape scene, IShape diffuse, IShape glossy, ILightSource lights)
	{
		Luminance result = Luminance.Zero;
		Luminance factor = new Luminance(1, 1, 1);
		Vector current_point = point;
		Vector current_direction = direction;
		HitPoint current_hp = hp;

		while(true)
		{
			Luminance direct = Luminance.Zero;

			for(int i = 0; i < SHADOW_RAYS; i++)
			{	
				LightPoint lp = lights.SampleLightPoint(rnd);
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

				HitPoint nhp = scene.Intersection(current_point, ndirection);

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
			//break;
			//Compute indirect luminancy
			
			double ksi = rnd.NextDouble();

			if(ksi < ABSOPTION)
			{
				break;
			}

			ksi = (ksi - ABSOPTION) / (1 - ABSOPTION);

			RandomDirection rndd = current_hp.material.SampleDirection(rnd, current_direction, current_hp.normal, ksi);
			
			if(rndd.factor.r == 0 && rndd.factor.g == 0 && rndd.factor.b == 0)
			{
				break;
			}

			HitPoint nhp1 = scene.Intersection(current_point, rndd.direction);
			
			if(nhp1 == null)
			{
				break;
			}

			factor *= rndd.factor / (1 - ABSOPTION);
			
			current_direction = rndd.direction;
			current_hp = nhp1;
			current_point += current_direction * current_hp.t;
		}

		return result;
	}
}
