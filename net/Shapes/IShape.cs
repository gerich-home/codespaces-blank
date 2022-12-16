namespace Engine;

public interface IShape
{
	HitPoint Intersection(Ray ray);
}
