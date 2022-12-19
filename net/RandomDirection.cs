namespace Engine;

public readonly record struct RandomDirection(
    Luminance factor,
    Vector direction
) {
	public static RandomDirection operator *(in RandomDirection a, double factor) =>
		a with {factor = a.factor * factor};

	public static RandomDirection operator *(double factor, in RandomDirection a) =>
		a with {factor = a.factor * factor};

	public static RandomDirection operator /(in RandomDirection a, double factor) =>
		a with {factor = a.factor / factor};
}
