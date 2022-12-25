using Engine;

namespace Engines;

public record class SimpleTracingEngine(
	Random rnd,
	SceneSetup sceneSetup,
	int reflectRaysCount,
	int shadowRaysCount,
	double absorptionProbability,
	double reflectFactor
): IEngine {
	public Luminance L(in Ray ray)
	{
		var rootHp = sceneSetup.scene.Intersection(null, ray);
	
		if (rootHp == null)
		{
			return Luminance.Zero;
		}

		var root = new Node
		{
			hp = rootHp,
			factor = Luminance.Unit,
			indirectLuminance = Luminance.Zero
		};

		var q = new Queue<Node>();
		var s = new Stack<Node>();
		q.Enqueue(root);

		while(q.Count > 0)
        {
			var node = q.Dequeue();
			var hp = node.hp;

            double ksi = rnd.NextDouble();

            if (ksi < absorptionProbability)
            {
                continue;
            }

			for(int i = 0; i < reflectRaysCount; i++)
			{
				var rndd = hp.SampleDirection(rnd.NextDouble());

				if (rndd.factor.IsZero)
				{
					continue;
				}

				var nextHp = sceneSetup.scene.Intersection(hp.shape, hp.RayAlong(rndd.direction));

				if (nextHp == null)
				{
					continue;
				}
				
				var nextNode = new Node
				{
					parent = node,
					hp = nextHp,
					factor = rndd.factor * reflectFactor
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

		return ComputeDirectLuminance(rootHp) + root.indirectLuminance;
	}

    private Luminance ComputeDirectLuminance(HitPoint hp)
    {
		var result = Enumerable
			.Range(0, shadowRaysCount)
			.Aggregate(Luminance.Zero, (l1, i) => l1 + ComputeDirectLuminanceForSingleRay(hp));
			
		return result / shadowRaysCount;
    }
	
    private Luminance ComputeDirectLuminanceForSingleRay(HitPoint hp)
    {
		var lp = sceneSetup.lights.SampleLightPoint(hp);
		if (lp.factor == 0)
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
		if (l < double.Epsilon)
		{
			return Luminance.Zero;
		}

		double linv = 1 / l;
		directionToLight *= linv;

		var barrierHp = sceneSetup.scene.Intersection(hp.shape, hp.RayAlong(directionToLight));

		if (barrierHp != null && (barrierHp.T <= l - double.Epsilon))
		{
			return Luminance.Zero;
		}

		linv *= linv;
		linv *= linv;

		return (-cos_dir_normal * cos_dir_lnormal * lp.factor * linv) * lp.Le * hp.BRDF(directionToLight);
    }
}

class Node
{
	public Node parent;
	public HitPoint hp;
	public Luminance indirectLuminance = Luminance.Zero;
	public Luminance factor;
}
