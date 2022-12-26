namespace Engine;

public interface ILightSource
{
	bool CanSendLightTo(HitPoint hitPoint);
	LightPoint SampleLightPoint(HitPoint hitPoint);
	Photon[] EmitPhotons(int nphotons);
    Luminance Le { get; }
	ref readonly AABB AABB { get; }
}
