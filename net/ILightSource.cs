namespace Engine;

public interface ILightSource
{
	LightPoint SampleLightPoint(Random rnd, Vector point);
	Photon[] EmitPhotons(Random rnd, int nphotons);
	Luminance Le();
}
