namespace Engine;

public interface ILightSource
{
	LightPoint SampleLightPoint(Random rnd);
	Photon[] EmitPhotons(Random rnd, int nphotons);
    Luminance Le { get; }
}
