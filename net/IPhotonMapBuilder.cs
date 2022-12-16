namespace Engine;

public interface IPhotonMapBuilder
{
	PhotonMap BuildPhotonMap(IShape scene, IShape diffuse, IShape glossy, ILightSource lights);
}
