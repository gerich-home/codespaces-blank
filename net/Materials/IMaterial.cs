namespace Engine;

public interface IMaterial
{
	Luminance BRDF(HitPoint hitPoint, Vector ndirection);
	RandomDirection SampleDirection(HitPoint hitPoint, double ksi);
}
