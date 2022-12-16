namespace Engine;

public record class Vector(double x, double y, double z)
{
	public Vector Normalized => this / Length;

	public double Norm => x * x + y * y + z * z;

	public double Length => Math.Sqrt(Norm);

	public static Vector operator +(Vector a, Vector b) =>
		new Vector(a.x + b.x, a.y + b.y, a.z + b.z);
		
	public static Vector operator -(Vector a, Vector b) =>
		new Vector(a.x - b.x, a.y - b.y, a.z - b.z);
		
	public static Vector operator *(Vector a, double factor) =>
		new Vector(a.x * factor, a.y * factor, a.z * factor);

	public static Vector operator *(double factor, Vector a) =>
		new Vector(a.x * factor, a.y * factor, a.z * factor);

	public static Vector operator /(Vector a, double factor) =>
		a * (1 / factor);

	public Vector CrossProduct(Vector vector) =>
		new Vector(
			y * vector.z - vector.y * z,
			z * vector.x - vector.z * x,
			x * vector.y - vector.x * y
		);

	public double DotProduct(Vector vector) =>
		vector.x * x + vector.y * y + vector.z * z;

	public Vector Transform(Vector axis)
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
}
