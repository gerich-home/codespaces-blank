namespace Engine;

public readonly record struct LightPoint(
	Vector point,
	Vector normal,
	double factor, // 1 / prob(point)
	Luminance Le
);
