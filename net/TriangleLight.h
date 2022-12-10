#pragma once


namespace Lights
{
	using namespace Engine;

	class Triangle: public ILightSource
	{
		Triangle(const Vector a, const Vector b, const Vector c, Luminance le);

		LightPoint SampleLightPoint(Vector point) const;
		void EmitPhotons(int nphotons, Photon photons[]) const;
		Luminance Le() const;

	private:
		const double probability;
		const Vector ba;
		const Vector ca;
		const Vector a;
		const Vector normal;
		const Luminance le;
	};
}