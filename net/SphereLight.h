#pragma once


namespace Lights
{
	using namespace Engine;

	class Sphere : public ILightSource
	{
		Sphere(Vector center, double r, Luminance le);

		LightPoint SampleLightPoint(Vector point);
		void EmitPhotons(int nphotons, Photon photons[]);
		Luminance Le();

	private:
		readonly double r;
		readonly double probability;
		readonly Vector center;
		readonly Luminance le;
	};
}
