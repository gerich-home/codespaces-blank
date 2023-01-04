namespace Engine;

public interface IMaterial
{
	// cos(hitPoint.normal, directionToLight) > 0
	Luminance BRDF(HitPoint hitPoint, in Vector directionToLight);

	// result.factor == BRDF(hitPoint, result.directionToLight) * cos(hitPoint.normal, result.directionToLight) / prob(result.directionToLight)
	// result.directionToLight.Length == 1 
	// cos(hitPoint.normal, result.directionToLight) > 0
	RandomDirection SampleDirection(HitPoint hitPoint);

	// BRDF = delta(some direction), i.e. probablity(BRDF > 0) == 0
	bool IsPerfect { get; }
}
