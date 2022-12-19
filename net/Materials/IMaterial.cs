namespace Engine;

public interface IMaterial
{
	Luminance BRDF(HitPoint hitPoint, in Vector directionToLight);
	RandomDirection SampleDirection(HitPoint hitPoint, double ksi);
}
