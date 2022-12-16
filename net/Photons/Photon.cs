namespace Engine;

public record class Photon(
	Ray ray,
	Vector normal,
	Luminance energy
);
