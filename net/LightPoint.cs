namespace Engine;

public record class LightPoint(
	Vector point,
	Vector normal,
	double probability,
	Luminance Le
);
