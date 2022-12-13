using Engine;

namespace PhotonMapBuilders
{
	class CausticPhotonMapBuilder : IPhotonMapBuilder
	{
		const int NPHOTONS = 100;
		const double ABSOPTION = 0.3;

		PhotonMap BuildPhotonMap(IShape scene, IShape diffuse, IShape glossy, ILightSource lights)
		{
			PhotonMap pm = PhotonMap(NPHOTONS);
			Photon[] emitted_photons = new Photon[NPHOTONS];

			lights.EmitPhotons(NPHOTONS, emitted_photons);

			for(int i = 0; i < NPHOTONS; i++)
			{
				bool isDiffuse = false;
				Photon current_photon = emitted_photons[i];
				
				HitPoint ghp = glossy.Intersection(current_photon.point, current_photon.direction);
				
				if(!ghp)
				{
					continue;
				}
				
				HitPoint dhp = diffuse.Intersection(current_photon.point, current_photon.direction);
				
				if(dhp)
				{
					if(dhp.t < ghp.t)
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
				
					if(dhp)
					{
						if(ghp)
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
						if(ghp)
						{
							hp = ghp;
						}
						else
						{
							break;
						}
					}

					double ksi = (double) rand() / (RAND_MAX + 1);

					if(ksi < ABSOPTION)
					{
						break;
					}

					ksi = (ksi - ABSOPTION) / (1 - ABSOPTION);

					RandomDirection rndd = hp.material.SampleDirection(current_photon.direction, hp.normal, ksi);
				
					if(rndd.factor.colors[L_R] == 0 && rndd.factor.colors[L_G] == 0 && rndd.factor.colors[L_B] == 0)
					{
						break;
					}
					

					current_photon.point += hp.t * current_photon.direction;
					current_photon.direction = rndd.direction;
					current_photon.energy *= rndd.factor / (1 - ABSOPTION);
					
					if(isDiffuse)
					{
						current_photon.normal = hp.normal;
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