using Engine;

namespace Lights;

public class CompositeLightSource : ILightSource
{
	public readonly ILightSource[] lights;
	public readonly double[] probabilities;

	public CompositeLightSource(ILightSource[] lights)
	{
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

	public LightPoint SampleLightPoint(Random rnd, Vector point)
	{
		double ksi = rnd.NextDouble();

		for(int i = 0; i < lights.Length; i++)
		{
			if(ksi < probabilities[i])
			{
				LightPoint lp = lights[i].SampleLightPoint(rnd, point);
				return new LightPoint(lp.point, lp.normal, probabilities[i] * lp.probability, lp.Le);
			}
			else
			{
				ksi -= probabilities[i];
			}
		}

		throw new Exception();
	}

	public Photon[] EmitPhotons(Random rnd, int nphotons)
	{
		double[] energy = new double[lights.Length];
		
		Luminance totalLe = new Luminance(0, 0, 0);
		for(int i = 0; i < lights.Length; i++)
		{
			Luminance lei = lights[i].Le();
			totalLe += lei;
			energy[i] = (lei.r + lei.g + lei.b) / 3;
		}

		double factor = nphotons * 3 / (totalLe.r + totalLe.g + totalLe.b);

		int[] nphotonsPerLight = new int[lights.Length];
		int remainedPhotons = 0;

		for(int i = 0; i < lights.Length; i++)
		{
			nphotonsPerLight[i] = (int)(factor * energy[i]);
			remainedPhotons += nphotonsPerLight[i];
		}

		remainedPhotons = nphotons - remainedPhotons;
		
		for(int i = 0; i < remainedPhotons; i++)
		{
			nphotonsPerLight[i]++;
		}

		return lights
			.SelectMany((light, i) => light.EmitPhotons(rnd, nphotonsPerLight[i]))
			.ToArray();
	}

	public Luminance Le()
	{
		Luminance Le = new Luminance(0, 0, 0);

		for(int i = 0; i < lights.Length; i++)
		{
			Le += lights[i].Le();
		}
		
		return Le;
	}
}
