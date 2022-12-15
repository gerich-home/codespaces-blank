namespace Engine
{
	public record class Photon(
		Vector point,
		Vector normal,
		Vector direction,
		Luminance energy
	);
}