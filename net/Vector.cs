using System.Text;

namespace Engine;

public readonly record struct Vector(double x, double y, double z)
{
	public Ray RayAlong(in Vector direction) =>
		new Ray(this, direction);

	public static Vector Zero => new Vector(0, 0, 0);
	public static Vector Unit => new Vector(1, 1, 1);
	public static Vector UnitX => new Vector(1, 0, 0);
	public static Vector UnitY => new Vector(0, 1, 0);
	public static Vector UnitZ => new Vector(0, 0, 1);

	public Vector Normalized => this / Length;

	public double Norm => x * x + y * y + z * z;

	public double Length => Math.Sqrt(Norm);

	public static Vector operator +(in Vector a, in Vector b) =>
		new Vector(a.x + b.x, a.y + b.y, a.z + b.z);
		
	public static Vector operator -(in Vector a, in Vector b) =>
		new Vector(a.x - b.x, a.y - b.y, a.z - b.z);
		
	public static Vector operator -(in Vector a) =>
		new Vector(-a.x, -a.y, -a.z);
		
	public static Vector operator *(in Vector a, double factor) =>
		new Vector(a.x * factor, a.y * factor, a.z * factor);

	public static Vector operator *(double factor, in Vector a) =>
		new Vector(a.x * factor, a.y * factor, a.z * factor);

	public static Vector operator /(in Vector a, double factor) =>
		a * (1 / factor);

	public Vector CrossProduct(in Vector vector) =>
		new Vector(
			y * vector.z - vector.y * z,
			z * vector.x - vector.z * x,
			x * vector.y - vector.x * y
		);

	public double DotProduct(in Vector vector) =>
		vector.x * x + vector.y * y + vector.z * z;

	public Vector Transform(in Vector axis)
	{
		const double threshold = 0.57735026919;

		Vector majorAxis;
		if(Math.Abs(axis.x) < threshold)
			majorAxis = UnitX;
		else if(Math.Abs(axis.y) < threshold)
			majorAxis = UnitY;
		else
			majorAxis = UnitZ;

		var M1 = axis.CrossProduct(majorAxis).Normalized;
		var M2 = axis.CrossProduct(M1);

		return x * M1 + y * M2 + z * axis;
	}

	public double this[int index] => index switch {
		0 => x,
		1 => y,
		2 => z,
		_ => throw new Exception()
	};

	private bool PrintMembers(StringBuilder builder)
    {
        builder.Append(nameof(x));
        builder.Append(" = ");
        builder.Append(x);
		builder.Append(", ");
        builder.Append(nameof(y));
        builder.Append(" = ");
        builder.Append(y);
		builder.Append(", ");
        builder.Append(nameof(z));
        builder.Append(" = ");
        builder.Append(z);
		return true;
	}
}
