using Engine;

namespace Engines;

public record class SimpleTracingEngine(
	Random rnd,
	SceneSetup sceneSetup
): IEngine {
	public const int REFLECT_RAYS = 3;
	public const int SHADOW_RAYS = 10;
	public const double ABSOPTION = 0.7;

	public Luminance L(in Ray ray)
	{
		var hp = sceneSetup.scene.Intersection(ray);
	
		if (hp == null)
		{
			return Luminance.Zero;
		}

		var root = new Node
		{
			hp = hp,
			factor = new Luminance(1, 1, 1),
			indirectLuminance = Luminance.Zero
		};

		var q = new Queue<Node>();
		var s = new Stack<Node>();
		q.Enqueue(root);

		while(q.Count > 0)
        {
			var node = q.Dequeue();

            double ksi = rnd.NextDouble();

            if (ksi < ABSOPTION)
            {
                continue;
            }

			for(int i = 0; i < REFLECT_RAYS; i++)
			{
				var rndd = hp.SampleDirection(rnd.NextDouble());

				if (rndd.factor.IsZero)
				{
					continue;
				}

				var nextHp = sceneSetup.scene.Intersection(hp.RayAlong(rndd.direction));

				if (nextHp == null)
				{
					continue;
				}
				
				var nextNode = new Node
				{
					parent = node,
					hp = nextHp,
					factor = rndd.factor / (REFLECT_RAYS * (1 - ABSOPTION))
				};
            	q.Enqueue(nextNode);
				s.Push(nextNode);
			}
        }

		while(s.Count > 0)
		{
			var node = s.Pop();
			var directLuminance = ComputeDirectLuminance(node.hp);
			node.parent.indirectLuminance += (directLuminance + node.indirectLuminance) * node.factor;
		}

		return ComputeDirectLuminance(root.hp) + root.indirectLuminance;		
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
		var lp = sceneSetup.lights.SampleLightPoint(hp);
		if (lp.probability == 0)
		{
			return Luminance.Zero;
		}

		var directionToLight = lp.point - hp.Point;

		double cos_dir_normal = hp.Normal.DotProduct(directionToLight);

		if (cos_dir_normal <= 0)
		{
			return Luminance.Zero;
		}

		double cos_dir_lnormal = directionToLight.DotProduct(lp.normal);
		if (cos_dir_lnormal >= 0)
		{
			return Luminance.Zero;
		}

		double l = directionToLight.Length;
		if (l * l < double.Epsilon)
		{
			return Luminance.Zero;
		}

		double linv = 1 / l;
		directionToLight *= linv;
		cos_dir_normal *= linv;
		cos_dir_lnormal *= linv;

		var barrierHp = sceneSetup.scene.Intersection(hp.RayAlong(directionToLight));

		if (barrierHp != null && (barrierHp.T <= l - double.Epsilon))
		{
			return Luminance.Zero;
		}

		return lp.Le * hp.BRDF(directionToLight) * (-cos_dir_normal * cos_dir_lnormal / (lp.probability * l * l));
    }
}

class Node
{
	public Node parent;
	public HitPoint hp;
	public Luminance indirectLuminance = Luminance.Zero;
	public Luminance factor;
}
