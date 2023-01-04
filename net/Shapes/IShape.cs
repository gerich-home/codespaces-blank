namespace Engine;

public interface IShape
{
	ShapeHitPoint Intersection(in Ray ray);
	ref readonly AABB AABB { get; }
}

public interface ITexturableShape : IShape
{
	TexturableShapeHitPoint TexturableIntersection(in Ray ray);
}
