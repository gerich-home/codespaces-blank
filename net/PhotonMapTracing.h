#pragma once


	
class Engine.Vector;
class Engine.Luminance;
class Engine.IEngine;
class Engine.ILightSource;
class Engine.IShape;
class Engine.PhotonMap;

namespace Engines
{
	using namespace Engine;

	class PhotonMapTracing: public IEngine
	{
		PhotonMapTracing(PhotonMap globalMap, PhotonMap causticsMap) :
			globalMap(globalMap),
			causticsMap(causticsMap)
		{
		}

		Luminance L(HitPoint hp, Vector point, Vector direction, IShape scene, IShape diffuse, IShape glossy, ILightSource lights);

	private:
		PhotonMap globalMap;
		PhotonMap causticsMap;
	};
}