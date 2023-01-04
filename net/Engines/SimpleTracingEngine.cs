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

	public Luminance L(in Ray ray)
	{
		#if DEBUG_DEPTH
		// output luminace of that depth only
		const int DEPTH = 2;
		#endif

		var rootHp = sceneSetup.scene.Intersection(ray);
	
		if (rootHp == null)
		{
			return Luminance.Zero;
		}

		var resultNode = new Node
		{
			indirectLuminance = Luminance.Zero,
		};

		var root = new Node
		{
			parent = resultNode,
			hp = rootHp,
			factor = Luminance.Unit,
			indirectLuminance = Luminance.Zero,
			#if DEBUG_DEPTH
			depth = 0
			#endif
		};

		var q = new Queue<Node>();
		var s = new Stack<Node>();
		q.Enqueue(root);
		s.Push(root);

		while(q.Count > 0)
        {
			var node = q.Dequeue();

			#if DEBUG_DEPTH
			if (node.depth == DEPTH)
			{
				continue;
			}
			#endif

			var hp = node.hp;
			var isPerfect = hp.material.IsPerfect;

			if(!isPerfect)
			{
				var ksi = rnd.NextDouble();

				if (ksi < absorptionProbability)
				{
					continue;
				}
			}

			var actualReflectRaysCount = isPerfect ? 1 : reflectRaysCount;
			var factor = isPerfect ? 1 : reflectFactor;

			for(int i = 0; i < actualReflectRaysCount; i++)
			{
				var rndd = hp.SampleDirection();

				if (rndd.factor.IsZero)
				{
					continue;
				}

				var nextHp = sceneSetup.scene.Intersection(hp.RayAlong(rndd.directionToLight, tolerance));

				if (nextHp == null)
				{
					continue;
				}
				
				var nextNode = new Node
				{
					parent = node,
					hp = nextHp,
					factor = rndd.factor * factor,

					#if DEBUG_DEPTH
					depth = node.depth + 1
					#endif
				};
				q.Enqueue(nextNode);
				s.Push(nextNode);
			}
        }

		while(s.Count > 0)
		{
			var node = s.Pop();
					
			#if DEBUG_DEPTH
			var directLuminance = node.depth == DEPTH ? ComputeDirectLuminance(node.hp) : Luminance.Zero;
			#else
			var directLuminance = ComputeDirectLuminance(node.hp);
			#endif
			node.parent.indirectLuminance += (directLuminance + node.indirectLuminance) * node.factor;
		}

		return resultNode.indirectLuminance;
	}

    private Luminance ComputeDirectLuminance(HitPoint hp)
    {
		if(hp.material.IsPerfect)
		{
			return Luminance.Zero;
		}

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
	public HitPoint hp;
	public Luminance indirectLuminance = Luminance.Zero;
	public Luminance factor;

	#if DEBUG_DEPTH
	public int depth;
	#endif
}
