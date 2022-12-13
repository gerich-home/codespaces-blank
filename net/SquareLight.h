#pragma once


namespace Lights
{
	using namespace Engine;

	class Square: public ILightSource
	{
		Square(Vector a, Vector b, Vector c, Luminance le);

		LightPoint SampleLightPoint(Vector point);
		void EmitPhotons(int nphotons, Photon photons[]);
		Luminance Le();

	private:
		readonly Vector a;
		readonly Vector ba;
		readonly Vector ca;
		readonly Vector normal;
		readonly double probability;
		readonly Luminance le;
	};
}