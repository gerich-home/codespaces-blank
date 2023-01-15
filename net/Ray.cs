namespace Engine;

public readonly record struct Ray(
	Vector start,
	Vector direction
) {
	public Vector PointAt(double t) =>
		start + t * direction;

	public Vector InvDirection =>
		new Vector(
			1 / direction.x,
			1 / direction.y,
			1 / direction.z
		);
}
