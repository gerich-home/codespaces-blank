namespace Engine;

public interface IPhotonMapBuilder
{
	PhotonMap BuildPhotonMap(IBody scene, IBody diffuse, IBody glossy, ILightSource lights);
}
