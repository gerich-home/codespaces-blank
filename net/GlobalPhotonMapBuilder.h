#pragma once

#include "IPhotonMapBuilder.h"

namespace PhotonMapBuilders
{
	using namespace Engine;
	
	class GlobalPhotonMapBuilder : public IPhotonMapBuilder
	{
	public:
		virtual const PhotonMap BuildPhotonMap(const IShape& scene, const IShape& diffuse, const IShape& glossy, const ILightSource& lights) const;
	};
}