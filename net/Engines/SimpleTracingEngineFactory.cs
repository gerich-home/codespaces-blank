using Engine;

namespace Engines;

public record class SimpleTracingEngineFactory: IEngineFactory {
	public IEngine CreateEngine(Random rnd, SceneSetup sceneSetup) =>
		new SimpleTracingEngine(
			rnd,
			sceneSetup
		);
}
