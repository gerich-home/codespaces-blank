namespace Engine
{
	interface IMaterial
	{
		Luminance BRDF(Vector direction, Vector ndirection, Vector normal);
		RandomDirection SampleDirection(Vector direction, Vector normal, double ksi);
	}
}