namespace Engine;

public interface IMaterial
{
	Luminance BRDF(HitPoint hitPoint, in Vector outgoingDirection);

	// result.factor == BRDF(hitPoint) * cos(hitPoint.normal, result.direction) / prob(result.direction)
	// result.direction.Length == 1 
	// cos(hitPoint.normal, result.direction) > 0
	RandomDirection SampleDirection(HitPoint hitPoint, double ksi);
}
