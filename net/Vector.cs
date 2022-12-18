using System.Text;

namespace Engine;

public readonly record struct Vector(double x, double y, double z)
{
	public Ray RayAlong(in Vector direction) =>
		new Ray(this, direction);

	public static Vector Zero => new Vector(0, 0, 0);

	public Vector Normalized => this / Length;

	public double Norm => x * x + y * y + z * z;

	public double Length => Math.Sqrt(Norm);

	public static Vector operator +(in Vector a, in Vector b) =>
		new Vector(a.x + b.x, a.y + b.y, a.z + b.z);
		
	public static Vector operator -(in Vector a, in Vector b) =>
		new Vector(a.x - b.x, a.y - b.y, a.z - b.z);
		
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
		Vector t;

		if(Math.Abs(axis.x) < Math.Abs(axis.y))
		{
			if(Math.Abs(axis.x) < Math.Abs(axis.z))
				t = axis with {x = 1};
			else
				t = axis with {z = 1};
		}
		else
		{
			if(Math.Abs(axis.y) < Math.Abs(axis.z))
				t = axis with {y = 1};
			else
				t = axis with {z = 1};
		}

		var M1 = axis.CrossProduct(t).Normalized;
		var M2 = axis.CrossProduct(M1);

		return new Vector(
			x * M1.x + y * M2.x + z * axis.x,
			x * M1.y + y * M2.y + z * axis.y,
			x * M1.z + y * M2.z + z * axis.z
		);
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
