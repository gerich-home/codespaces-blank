using Engine;

namespace Lights;

public class CompositeLightSource : ILightSource
{
	public readonly (ILightSource light, double probability)[] lights;
	public readonly Random rnd;

	public static ILightSource Create(Random rnd, ILightSource[] lights)
	{
		if (lights.Length == 1)
		{
			return lights[0];
		}

		return new CompositeLightSource(rnd, CalculateLightsWithProbabilities(lights));
	}

	private static (ILightSource light, double probability)[] CalculateLightsWithProbabilities(ILightSource[] lights)
	{
		var k = 1.0 / lights.Sum(light => light.Le.Energy);

		return lights.Select(light => (light, light.Le.Energy * k)).ToArray();
	}

	private CompositeLightSource(Random rnd, (ILightSource light, double probability)[] lights)
	{
		this.rnd = rnd;
		this.lights = lights;
	}

	public bool CanSendLightTo(HitPoint hitPoint) =>
		lights.Any(l => l.light.CanSendLightTo(hitPoint));

	public LightPoint SampleLightPoint(HitPoint hitPoint)
	{
		var visibleLights = lights.Where(l => l.light.CanSendLightTo(hitPoint)).ToList();
		
		var totalP = visibleLights.Sum(l => l.probability);

		double ksi = rnd.NextDouble(totalP);

		foreach (var (light, p) in visibleLights)
		{
			if(ksi < p)
			{
				var lp = light.SampleLightPoint(hitPoint);
				return new LightPoint(lp.point, lp.normal, p * lp.probability / totalP, lp.Le);
			}
			else
			{
				ksi -= p;
			}
		}

		return new LightPoint();
	}

	public Photon[] EmitPhotons(int nphotons)
	{
		double factor = nphotons / Le.Energy;

		int[] nphotonsPerLight = lights
			.Select(l => (int)(factor * l.light.Le.Energy))
			.ToArray();

		int remainingPhotons = nphotons - nphotonsPerLight.Sum();
		
		for(int i = 0; i < remainingPhotons; i++)
		{
			nphotonsPerLight[i]++;
		}

		return lights
			.SelectMany((l, i) => l.light.EmitPhotons(nphotonsPerLight[i]))
			.ToArray();
	}

    public Luminance Le =>
		lights
			.Select(l => l.light.Le)
			.Aggregate(Luminance.Zero, (a, b) => a + b);
}
