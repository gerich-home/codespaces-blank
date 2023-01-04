namespace Engine;

public interface IShape
{
	HitPoint Intersection(in Ray ray);
	ref readonly AABB AABB { get; }
}
