namespace Engine;

public readonly record struct Ray(
	Vector start,
	Vector direction
) {
	public Vector PointAt(double t) =>
		start + t * direction;
}
