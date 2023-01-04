namespace Engine;

public interface IBody
{
	BodyHitPoint Intersection(in Ray ray);
	ref readonly AABB AABB { get; }
}
