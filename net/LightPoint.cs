namespace Engine
{
	class LightPoint
	{	
		readonly Vector point;
		readonly Vector normal;
		readonly double probability;
		readonly Luminance Le;

		LightPoint(Vector point, Vector normal, double probability, Luminance Le)
		{
			this.point = point;
			this.normal = normal;
			this.probability = probability;
			this.Le = Le;
		}
	};
}