namespace Engine;

public interface IMaterial
{
	Luminance BRDF(HitPoint hitPoint, in Vector directionToLight);

	// result.factor == BRDF(hitPoint, result.directionToLight) * cos(hitPoint.normal, result.directionToLight) / prob(result.directionToLight)
	// result.directionToLight.Length == 1 
	// cos(hitPoint.normal, result.directionToLight) > 0
	RandomDirection SampleDirection(HitPoint hitPoint, double ksi);
}
