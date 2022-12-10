#pragma once


namespace Engine
{
			
	interface IPhotonMapBuilder
	{
		PhotonMap BuildPhotonMap(IShape scene, IShape diffuse, IShape glossy, ILightSource lights);
	};
}