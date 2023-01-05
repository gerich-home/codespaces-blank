// #define DEBUG_DEPTH

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
	const double tolerance = 1E-8;

	public Luminance L(in Ray cameraRay)
    {
#if DEBUG_DEPTH
		// output luminace of that depth only
		const int DEPTH = 2;
#endif

        var resultNode = new Node();

        var root = new Node
        {
            parent = resultNode,
			ray = cameraRay,
            factor = Luminance.Unit,
#if DEBUG_DEPTH
			depth = 0
#endif
        };

        var q = new Queue<Node>();
        var s = new Stack<Node>();
        q.Enqueue(root);

        while (q.Count > 0)
        {
            var node = q.Dequeue();
	        s.Push(node);

#if DEBUG_DEPTH
			if (node.depth == DEPTH)
			{
				continue;
			}
#endif

			ref var ray = ref node.ray;
			var hp = sceneSetup.scene.Intersection(ray);
			var lightHp = sceneSetup.lights.Intersection(ray);
			

#if DEBUG_DEPTH
			var directLuminance = node.depth == DEPTH ? ComputeDirectLuminance(hp) : Luminance.Zero;
#else
            var directLuminance = ComputeDirectLuminance(hp);
#endif

			node.luminance = directLuminance + LightLuminance(hp, lightHp);

			if(hp == null)
			{
				continue;
			}

            var isPerfect = hp.Material.IsPerfect;

            if (!isPerfect)
            {
                var ksi = rnd.NextDouble();

                if (ksi < absorptionProbability)
                {
                    continue;
                }
            }

            var actualReflectRaysCount = isPerfect ? 1 : reflectRaysCount;
            var factor = isPerfect ? 1 : reflectFactor;

            for (int i = 0; i < actualReflectRaysCount; i++)
            {
                var rndd = hp.SampleDirection();

                if (rndd.factor.IsZero)
                {
                    continue;
                }

                var nextNode = new Node
                {
                    parent = node,
                    ray = hp.RayAlong(rndd.directionToLight, tolerance),
                    factor = rndd.factor * factor,
#if DEBUG_DEPTH
					depth = node.depth + 1
#endif
                };
				q.Enqueue(nextNode);
            }
        }

        while (s.Count > 0)
        {
            var node = s.Pop();
            node.parent.luminance += node.luminance * node.factor;
        }

        return resultNode.luminance;
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
	public Ray ray;
	public Luminance luminance = Luminance.Zero;
	public Luminance factor;

	#if DEBUG_DEPTH
	public int depth;
	#endif
}
