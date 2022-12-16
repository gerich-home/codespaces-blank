using Engine;

namespace Lights;

public class CompositeLightSource : ILightSource
{
	public readonly ILightSource[] lights;
	public readonly double[] probabilities;
	public readonly Random rnd;

	public CompositeLightSource(Random rnd, ILightSource[] lights)
	{
		this.rnd = rnd;
		this.lights = lights;
		this.probabilities = new double[lights.Length];
		
		double p = 1.0 / lights.Length;

		for(int i = 0; i < lights.Length; i++)
		{
			this.probabilities[i] = p;
		}
	}

	public CompositeLightSource(ILightSource[] lights, double[] probabilities)
	{
		this.lights = lights;
		this.probabilities = probabilities;
		
		double sum = 0;

		for(int i = 0; i < lights.Length; i++)
		{
			sum += probabilities[i];
		}
		
		sum = 1 / sum;
		for(int i = 0; i < lights.Length; i++)
		{
			this.probabilities[i] *= sum;
		}
	}

	public LightPoint SampleLightPoint()
	{
		double ksi = rnd.NextDouble();

		for(int i = 0; i < lights.Length; i++)
		{
			if(ksi < probabilities[i])
			{
				LightPoint lp = lights[i].SampleLightPoint();
				return new LightPoint(lp.point, lp.normal, probabilities[i] * lp.probability, lp.Le);
			}
			else
			{
				ksi -= probabilities[i];
			}
		}

		throw new Exception();
	}

	public Photon[] EmitPhotons(int nphotons)
	{
		double factor = nphotons / Le.Energy;

		int[] nphotonsPerLight = lights
			.Select(light => (int)(factor * light.Le.Energy))
			.ToArray();

		int remainingPhotons = nphotons - nphotonsPerLight.Sum();
		
		for(int i = 0; i < remainingPhotons; i++)
		{
			nphotonsPerLight[i]++;
		}

		return lights
			.SelectMany((light, i) => light.EmitPhotons(nphotonsPerLight[i]))
			.ToArray();
	}

    public Luminance Le =>
		lights
			.Select(ls => ls.Le)
			.Aggregate(Luminance.Zero, (a, b) => a + b);
}
