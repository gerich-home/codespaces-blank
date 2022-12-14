namespace Engine
{
	public interface IShape
	{
		HitPoint Intersection(Vector start, Vector direction);
	}
}
