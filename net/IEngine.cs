namespace Engine;

public interface IEngine
{
	Luminance L(Random rnd, HitPoint hp, Vector point, Vector direction, IShape scene, IShape diffuse, IShape glossy, ILightSource lights);
}
