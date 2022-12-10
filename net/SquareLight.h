#pragma once


namespace Lights
{
	using namespace Engine;

	class Square: public ILightSource
	{
		Square(const Vector a, const Vector b, const Vector c, Luminance le);

		LightPoint SampleLightPoint(Vector point) const;
		void EmitPhotons(int nphotons, Photon photons[]) const;
		Luminance Le() const;

	private:
		const Vector a;
		const Vector ba;
		const Vector ca;
		const Vector normal;
		const double probability;
		const Luminance le;
	};
}