#pragma once


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
		Luminance L(HitPoint hp, Vector point, Vector direction, IShape scene, IShape diffuse, IShape glossy, ILightSource lights) const;
	};
}