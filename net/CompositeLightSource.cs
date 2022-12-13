using Engine;

namespace Lights
{
	class CompositeLightSource : ILightSource
	{
		public readonly ILightSource lights;
		public readonly double[] probabilities;
		public readonly int nlights;

		CompositeLightSource(int nlights, ILightSource lights[])
		{
			this.nlights = nlights;
			this.lights = new ILightSource[nlights];
			this.probabilities = new double[nlights];
			
			double p = 1 / (double)nlights;

			for(int i = 0; i < nlights; i++)
			{
				this.lights[i] = lights[i];
				this.probabilities[i] = p;
			}
		}

		CompositeLightSource(int nlights, ILightSource lights[], double probabilities[])
		{
			this.nlights = nlights;
			this.lights = new ILightSource[nlights];
			this.probabilities = new double[nlights];
			
			double sum = 0;

			for(int i = 0; i < nlights; i++)
			{
				this.lights[i] = lights[i];
				this.probabilities[i] = probabilities[i];
				sum += probabilities[i];
			}
			
			sum = 1 / sum;
			for(int i = 0; i < nlights; i++)
			{
				this.probabilities[i] *= sum;
			}
		}

		LightPoint SampleLightPoint(Vector point)
		{
			double ksi = (double) rand() / (RAND_MAX + 1);

			for(int i = 0; i < nlights; i++)
			{
				if(ksi < probabilities[i])
				{
					LightPoint lp = lights[i].SampleLightPoint(point);
					return new LightPoint(lp.point, lp.normal, probabilities[i] * lp.probability, lp.Le);
				}
				else
				{
					ksi -= probabilities[i];
				}
			}
		}

		void EmitPhotons(int nphotons, Photon photons[])
		{
			double[] energy = new double[nlights];
			
			Luminance totalLe;
			for(int i = 0; i < nlights; i++)
			{
				readonly Luminance lei = lights[i].Le();
				totalLe += lei;
				energy[i] = (lei.r() + lei.g() + lei.b()) / 3;
			}

			double factor = nphotons * 3 / (totalLe.r() + totalLe.g() + totalLe.b());

			int[] nphotonsPerLight = new int[nlights];
			int remainedPhotons = 0;

			for(int i = 0; i < nlights; i++)
			{
				nphotonsPerLight[i] = factor * energy[i];
				remainedPhotons += nphotonsPerLight[i];
			}

			remainedPhotons = nphotons - remainedPhotons;
			
			for(int i = 0; i < remainedPhotons; i++)
			{
				nphotonsPerLight[i]++;
			}

			int offset = 0;

			for(int i = 0; i < nlights; i++)
			{
				lights[i].EmitPhotons(nphotonsPerLight[i], photons + offset);
				offset += nphotonsPerLight[i];
			}
		}

		Luminance Le()
		{
			Luminance Le;

			for(int i = 0; i < nlights; i++)
			{
				Le += lights[i].Le();
			}
			
			return Le;
		}
	};
}