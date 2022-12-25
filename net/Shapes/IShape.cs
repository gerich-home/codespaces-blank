namespace Engine;

public interface IShape
{
	HitPoint Intersection(IShape except, in Ray ray);
	ref readonly AABB AABB { get; }
}
