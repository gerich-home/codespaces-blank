namespace Engine;

public interface ILightSource
{
	LightPoint SampleLightPoint();
	Photon[] EmitPhotons(int nphotons);
    Luminance Le { get; }
}
