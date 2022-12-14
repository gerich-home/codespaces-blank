namespace Engine
{
	public interface IMaterial
	{
		Luminance BRDF(Vector direction, Vector ndirection, Vector normal);
		RandomDirection SampleDirection(Random rnd, Vector direction, Vector normal, double ksi);
	}
}