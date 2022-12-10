#pragma once


namespace Lights
{
	using namespace Engine;

	class Sphere : public ILightSource
	{
		Sphere(Vector center, double r, Luminance le);

		LightPoint SampleLightPoint(Vector point) const;
		void EmitPhotons(int nphotons, Photon photons[]) const;
		Luminance Le() const;

	private:
		const double r;
		const double probability;
		const Vector center;
		const Luminance le;
	};
}
