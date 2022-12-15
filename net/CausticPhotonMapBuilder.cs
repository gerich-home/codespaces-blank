using System;
using Engine;

namespace PhotonMapBuilders
{
	public class CausticPhotonMapBuilder : IPhotonMapBuilder
	{
		public const int NPHOTONS = 100;
		public const double ABSOPTION = 0.3;

		public PhotonMap BuildPhotonMap(Random rnd, IShape scene, IShape diffuse, IShape glossy, ILightSource lights)
		{
			PhotonMap pm = new PhotonMap(NPHOTONS);
			Photon[] emitted_photons = lights.EmitPhotons(rnd, NPHOTONS);

			for(int i = 0; i < NPHOTONS; i++)
			{
				bool isDiffuse = false;
				Photon current_photon = emitted_photons[i];
				
				HitPoint ghp1 = glossy.Intersection(current_photon.point, current_photon.direction);
				
				if(ghp1 == null)
				{
					continue;
				}
				
				HitPoint dhp1 = diffuse.Intersection(current_photon.point, current_photon.direction);
				
				if(dhp1 != null)
				{
					if(dhp1.t < ghp1.t)
					{
						continue;
					}
				}

				bool isNotFull = true;

				while(isNotFull)
				{
					isDiffuse = false;

					HitPoint hp;

					HitPoint ghp = glossy.Intersection(current_photon.point, current_photon.direction);
					HitPoint dhp = diffuse.Intersection(current_photon.point, current_photon.direction);
				
					if(dhp != null)
					{
						if(ghp != null)
						{
							if(dhp.t <= ghp.t)
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

					RandomDirection rndd = hp.material.SampleDirection(rnd, current_photon.direction, hp.normal, ksi);
				
					if(rndd.factor.r == 0 && rndd.factor.g == 0 && rndd.factor.b == 0)
					{
						break;
					}
					

					current_photon = current_photon with {
						point = current_photon.point + hp.t * current_photon.direction,
						direction = rndd.direction,
						energy = current_photon.energy * rndd.factor / (1 - ABSOPTION)
					};
					
					if(isDiffuse)
					{
						current_photon = current_photon with {
							normal = hp.normal
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
}