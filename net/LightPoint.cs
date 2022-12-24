namespace Engine;

public readonly record struct LightPoint(
	Vector point,
	Vector normal,
	double factor,
	Luminance Le
);
