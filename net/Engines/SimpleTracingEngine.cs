//#define DEBUG_DEPTH

using Engine;

namespace Engines;

public record class SimpleTracingEngine(
	Random rnd,
	SceneSetup sceneSetup,
	int reflectRaysCount,
	int shadowRaysCount,
	double absorptionProbability
): IEngine {
	const double tolerance = 1E-8;
	
    readonly List<Node> q = InitNodesStorage();
	private static List<Node> InitNodesStorage()
	{
		var result = new List<Node>(300);
		result.Add(new Node());
		return result;
	}

	public Luminance L(in Ray cameraRay)
    {
#if DEBUG_DEPTH
		// output luminance of that depth only
		const int DEPTH = 1;
#endif

        var root = new Node
        {
			ray = cameraRay,
            factor = Luminance.Unit,
			isDirect = true,
#if DEBUG_DEPTH
			depth = 0
#endif
        };

        q[0] = root;
		int nodesCount = 1;

        for(var index = 0; index < nodesCount; index++)
        {
            var node = q[index];

			ref var ray = ref node.ray;
			var hp = sceneSetup.scene.Intersection(ray);
			
#if DEBUG_DEPTH
			if(node.depth == DEPTH)
#endif
			{
				var ll = node.isDirect ? LightLuminance(hp, sceneSetup.lights.Intersection(ray)) : Luminance.Zero;
				var dl = ComputeDirectLuminance(hp);
				node.luminance = ll + dl;
			}

#if DEBUG_DEPTH
			if (node.depth == DEPTH)
			{
				continue;
			}
#endif

			if(hp == null)
			{
				continue;
			}

            var isPerfect = hp.Material.IsPerfect;

			var actualAbsorptionProbability = absorptionProbability + (1 - absorptionProbability) * (1 - (1.0 / (1 + node.depth / 10.0)));
            if (!isPerfect)
            {
                var ksi = rnd.NextDouble();

                if (ksi < actualAbsorptionProbability)
                {
                    continue;
                }
            }

            var actualReflectRaysCount = isPerfect ? 1 : reflectRaysCount;
            var factor = isPerfect ? 1 : 1 / (reflectRaysCount * (1 - actualAbsorptionProbability));

            for (int i = 0; i < actualReflectRaysCount; i++)
            {
                var rndd = hp.SampleDirection();

                if (rndd.factor.IsZero)
                {
                    continue;
                }

                var nextNode = new Node
                {
                    ray = hp.RayAlong(rndd.directionToLight, tolerance),
                    factor = rndd.factor * factor,
					depth = node.depth + 1,
					parent = node,
					hitPoint = hp,
					isDirect = hp.Material.IsPerfect
                };

				nodesCount++;
				if(q.Count < nodesCount)
				{
					q.Add(nextNode);
				}
				else
				{
					q[nodesCount - 1] = nextNode;
				}
            }
        }

        for(var index = nodesCount - 1; index > 0; index--)
        {
            var node = q[index];
            node.parent.luminance += node.luminance * node.factor;
        }

        return root.luminance;
    }

    private static Luminance LightLuminance(BodyHitPoint? sceneHp, LightHitPoint lightHp)
    {
        if (lightHp == null || sceneHp != null && sceneHp.T < lightHp.T)
		{
			return Luminance.Zero;
        }

		return (-lightHp.ShapeHitPoint.IncomingDirection.DotProduct(lightHp.ShapeHitPoint.Normal)) * lightHp.Le;
    }

    private Luminance ComputeDirectLuminance(BodyHitPoint hp)
    {
		if(hp == null || hp.Material.IsPerfect)
		{
			return Luminance.Zero;
		}

		var result = Enumerable
			.Range(0, shadowRaysCount)
			.Aggregate(Luminance.Zero, (l1, i) => l1 + ComputeDirectLuminanceForSingleRay(hp));
			
		return result / shadowRaysCount;
    }
	
    private Luminance ComputeDirectLuminanceForSingleRay(BodyHitPoint hp)
    {
		var lp = sceneSetup.lights.SampleLightPoint(hp);
		if (lp.factor == 0)
		{
			return Luminance.Zero;
		}

		var directionToLight = lp.point - hp.Point;

		var cos_dir_normal = directionToLight.DotProduct(hp.Normal);

		if (cos_dir_normal <= 0)
		{
			return Luminance.Zero;
		}

		var cos_dir_lnormal = directionToLight.DotProduct(lp.normal);
		if (cos_dir_lnormal >= 0)
		{
			return Luminance.Zero;
		}

		var l = directionToLight.Length;
		if (l < double.Epsilon)
		{
			return Luminance.Zero;
		}

		var linv = 1 / l;
		directionToLight *= linv;

		var barrierHp = sceneSetup.scene.Intersection(hp.RayAlong(directionToLight, tolerance));

		if (barrierHp != null && (barrierHp.T <= l - tolerance))
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
	public Ray ray;
	public Luminance luminance;
	public Luminance factor;
	public int depth;
	public Node parent;
	public BodyHitPoint hitPoint;
	public bool isDirect;
}
