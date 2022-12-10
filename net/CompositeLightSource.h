#pragma once


namespace Lights
{
	using namespace Engine;

	class CompositeLightSource : public ILightSource
	{
		CompositeLightSource(int nlights, ILightSource lights[]);
		CompositeLightSource(int nlights, ILightSource lights[], double probabilities[]);
		~CompositeLightSource();

		LightPoint SampleLightPoint(Vector point) const;
		void EmitPhotons(int nphotons, Photon photons[]) const;
		Luminance Le() const;

	private:
		ILightSource* lights;
		double* probabilities;
		int nlights;
	};
}