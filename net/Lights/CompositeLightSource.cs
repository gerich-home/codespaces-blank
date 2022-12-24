using Engine;

namespace Lights;

public class CompositeLightSource : ILightSource
{
	public readonly (ILightSource light, double energy)[] lights;
	public readonly Random rnd;

	public static ILightSource Create(Random rnd, ILightSource[] lights)
	{
		if (lights.Length == 1)
		{
			return lights[0];
		}

		return new CompositeLightSource(rnd, lights);
	}

	private CompositeLightSource(Random rnd, IEnumerable<ILightSource> lights)
	{
		this.rnd = rnd;
		this.lights = lights.Select(light => (light, light.Le.Energy)).ToArray();
	}

	public bool CanSendLightTo(HitPoint hitPoint) =>
		lights.Any(l => l.light.CanSendLightTo(hitPoint));

	public LightPoint SampleLightPoint(HitPoint hitPoint)
	{
		var visibleLights = lights.Where(l => l.light.CanSendLightTo(hitPoint)).ToList();
		
		var totalEnergy = visibleLights.Sum(l => l.light.Le.Energy);

		double ksi = rnd.NextDouble(totalEnergy);

		foreach (var (light, energy) in visibleLights)
		{
			if(ksi < energy)
			{
				var lp = light.SampleLightPoint(hitPoint);
				return lp with {
					factor = lp.factor * totalEnergy / energy
				};
			}
			else
			{
				ksi -= energy;
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
