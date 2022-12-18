namespace Engine;

public interface IMaterial
{
	Luminance BRDF(HitPoint hitPoint, in Vector ndirection);
	RandomDirection SampleDirection(HitPoint hitPoint, double ksi);
}
