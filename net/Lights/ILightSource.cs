namespace Engine;

public interface ILightSource
{
	bool CanSendLightTo(BodyHitPoint hitPoint);
	LightPoint SampleLightPoint(BodyHitPoint hitPoint);
	Photon[] EmitPhotons(int nphotons);
    Luminance Le { get; }
    Luminance Energy { get; }
	ref readonly AABB AABB { get; }
	LightHitPoint Intersection(in Ray ray);
}
