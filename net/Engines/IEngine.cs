namespace Engine;

public interface IEngineFactory
{
	IEngine CreateEngine(Random rnd, SceneSetup sceneSetup);
}

public interface IEngine
{
	Luminance L(Ray ray);
}
