using Engine;

namespace PhotonMapBuilders;

public class CausticPhotonMapBuilder : IPhotonMapBuilder
{
	public const int NPHOTONS = 100;
	public const double ABSOPTION = 0.3;
	public readonly Random rnd;

	public CausticPhotonMapBuilder(Random rnd)
	{
		this.rnd = rnd;
	}

	public PhotonMap BuildPhotonMap(IShape scene, IShape diffuse, IShape glossy, ILightSource lights)
	{
		PhotonMap pm = new PhotonMap(NPHOTONS);
		Photon[] emitted_photons = lights.EmitPhotons(NPHOTONS);

		for(int i = 0; i < NPHOTONS; i++)
		{
			bool isDiffuse = false;
			Photon current_photon = emitted_photons[i];
			
			HitPoint ghp1 = glossy.Intersection(null, current_photon.ray);
			
			if(ghp1 == null)
			{
				continue;
			}
			
			HitPoint dhp1 = diffuse.Intersection(null, current_photon.ray);
			
			if(dhp1 != null)
			{
				if(dhp1.T < ghp1.T)
				{
					continue;
				}
			}

			bool isNotFull = true;

			while(isNotFull)
			{
				isDiffuse = false;

				HitPoint hp;

				HitPoint ghp = glossy.Intersection(null, current_photon.ray);
				HitPoint dhp = diffuse.Intersection(null, current_photon.ray);
			
				if(dhp != null)
				{
					if(ghp != null)
					{
						if(dhp.T <= ghp.T)
						{
							isDiffuse = true;
							hp = dhp;
						}
						else
						{
							hp = ghp;
						}
					}
					else
					{
						isDiffuse = true;
						hp = dhp;
					}
				}
				else
				{
					if(ghp != null)
					{
						hp = ghp;
					}
					else
					{
						break;
					}
				}

				double ksi = rnd.NextDouble();

				if(ksi < ABSOPTION)
				{
					break;
				}

				ksi = (ksi - ABSOPTION) / (1 - ABSOPTION);

				//RandomDirection rndd = hp.material.SampleDirection(current_photon.ray.direction, hp.normal, ksi);
				RandomDirection rndd = hp.SampleDirection(ksi);
			
				if(rndd.factor.IsZero)
				{
					break;
				}
				

				current_photon = current_photon with {
					ray = current_photon.ray with {
						start = current_photon.ray.PointAt(hp.T),
						direction = rndd.directionToLight,
					},
					energy = current_photon.energy * rndd.factor / (1 - ABSOPTION)
				};
				
				if(isDiffuse)
				{
					current_photon = current_photon with {
						normal = hp.Normal
					};
					if(!pm.Add(current_photon))
					{
						isNotFull = false;
						break;
					}
				}
				
			}
			
		}

		pm.Build();
		return pm;
	}
}
