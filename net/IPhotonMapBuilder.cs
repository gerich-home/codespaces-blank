namespace Engine;

public interface IPhotonMapBuilder
{
	PhotonMap BuildPhotonMap(Random rnd, IShape scene, IShape diffuse, IShape glossy, ILightSource lights);
}
