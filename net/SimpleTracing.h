#pragma once

#include "IEngine.h"

class Engine::Vector;
class Engine::Luminance;
class Engine::IEngine;
class Engine::ILightSource;
class Engine::IShape;

namespace Engines
{
	using namespace Engine;

	class SimpleTracing: public IEngine
	{
	public:
		virtual Luminance L(const HitPoint& hp, const Vector& point, const Vector& direction, const IShape& scene, const IShape& diffuse, const IShape& glossy, const ILightSource& lights) const;
	};
}