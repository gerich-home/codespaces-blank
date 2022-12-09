namespace Engine
{
	interface IEngine
	{
		Luminance L(HitPoint hp, Vector point, Vector direction, IShape scene, IShape diffuse, IShape glossy, ILightSource lights);
	}
}