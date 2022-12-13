namespace Engine
{
	class LightPoint
	{	
		public readonly Vector point;
		public readonly Vector normal;
		public readonly double probability;
		public readonly Luminance Le;

		LightPoint(Vector point, Vector normal, double probability, Luminance Le)
		{
			this.point = point;
			this.normal = normal;
			this.probability = probability;
			this.Le = Le;
		}
	};
}