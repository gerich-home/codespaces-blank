using Engine;

namespace Engines;

public record class PhotonMapTracingEngineFactory(
	PhotonMap globalMap,
	PhotonMap causticsMap
): IEngineFactory {
	public IEngine CreateEngine(Random rnd, SceneSetup sceneSetup) =>
		new PhotonMapTracingEngine(
			globalMap,
			causticsMap,
			sceneSetup
		);
}
