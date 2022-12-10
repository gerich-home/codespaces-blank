#pragma once


namespace PhotonMapBuilders
{
	using namespace Engine;

	class CausticPhotonMapBuilder : public IPhotonMapBuilder
	{
		PhotonMap BuildPhotonMap(IShape scene, IShape diffuse, IShape glossy, ILightSource lights) const;
	};
}