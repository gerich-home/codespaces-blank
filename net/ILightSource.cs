using System;

namespace Engine
{
	public interface ILightSource
	{
		LightPoint SampleLightPoint(Random rnd, Vector point);
		void EmitPhotons(Random rnd, int nphotons, Photon[] photons);
		Luminance Le();
	}
}

