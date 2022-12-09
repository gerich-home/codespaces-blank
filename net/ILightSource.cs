namespace Engine
{
	interface ILightSource
	{
		LightPoint SampleLightPoint(Vector point);
		void EmitPhotons(int nphotons, Photon[] photons);
		Luminance Le();
	}
}

