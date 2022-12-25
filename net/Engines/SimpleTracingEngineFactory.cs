using Engine;

namespace Engines;

public record class SimpleTracingEngineFactory(
	int reflectRaysCount,
	int shadowRaysCount,
	double absorptionProbability
): IEngineFactory {
	public IEngine CreateEngine(Random rnd, SceneSetup sceneSetup) =>
		new SimpleTracingEngine(
			rnd,
			sceneSetup,
			reflectRaysCount,
			shadowRaysCount,
			absorptionProbability,
			1.0 / (reflectRaysCount * (1 - absorptionProbability))
		);
}
