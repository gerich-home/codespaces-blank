namespace Engine;

public readonly record struct LightPoint(
	Vector point,
	Vector normal,
	double probability,
	Luminance Le
);
