using System.Text;

namespace Engine;

public readonly record struct Matrix(
	double m11, double m12, double m13, double m14,
	double m21, double m22, double m23, double m24,
	double m31, double m32, double m33, double m34,
	double m41, double m42, double m43, double m44
) {
	public Vector MultiplicateByPosition(in Vector b) =>
		new Vector(
			m11 * b.x + m12 * b.y + m13 * b.z + m14,
			m21 * b.x + m22 * b.y + m23 * b.z + m24,
			m31 * b.x + m32 * b.y + m33 * b.z + m34
		);
		
	public Vector MultiplicateByDirection(in Vector b) =>
		new Vector(
			m11 * b.x + m12 * b.y + m13 * b.z,
			m21 * b.x + m22 * b.y + m23 * b.z,
			m31 * b.x + m32 * b.y + m33 * b.z
		);

	public static Matrix Identity =>
		new Matrix(
			1, 0, 0, 0,
			0, 1, 0, 0,
			0, 0, 1, 0,
			0, 0, 0, 1
		);

	public static Matrix TranslateX(double x) =>
		new Matrix(
			1, 0, 0, x,
			0, 1, 0, 0,
			0, 0, 1, 0,
			0, 0, 0, 1
		);

	public static Matrix TranslateY(double y) =>
		new Matrix(
			1, 0, 0, 0,
			0, 1, 0, y,
			0, 0, 1, 0,
			0, 0, 0, 1
		);

	public static Matrix TranslateZ(double z) =>
		new Matrix(
			1, 0, 0, 0,
			0, 1, 0, 0,
			0, 0, 1, z,
			0, 0, 0, 1
		);

	public static Matrix Translate(double x, double y, double z) =>
		new Matrix(
			1, 0, 0, x,
			0, 1, 0, y,
			0, 0, 1, z,
			0, 0, 0, 1
		);

	public static Matrix Translate(in Vector offset) =>
		new Matrix(
			1, 0, 0, offset.x,
			0, 1, 0, offset.y,
			0, 0, 1, offset.z,
			0, 0, 0, 1
		);

	public static Matrix ScaleX(double sx) =>
		new Matrix(
			sx, 0, 0, 0,
			0, 1, 0, 0,
			0, 0, 1, 0,
			0, 0, 0, 1
		);

	public static Matrix ScaleY(double sy) =>
		new Matrix(
			1, 0, 0, 0,
			0, sy, 0, 0,
			0, 0, 1, 0,
			0, 0, 0, 1
		);

	public static Matrix ScaleZ(double sz) =>
		new Matrix(
			1, 0, 0, 0,
			0, 1, 0, 0,
			0, 0, sz, 0,
			0, 0, 0, 1
		);

	public static Matrix Scale(double s) =>
		new Matrix(
			s, 0, 0, 0,
			0, s, 0, 0,
			0, 0, s, 0,
			0, 0, 0, 1
		);

	public static Matrix Scale(double sx, double sy, double sz) =>
		new Matrix(
			sx, 0, 0, 0,
			0, sy, 0, 0,
			0, 0, sz, 0,
			0, 0, 0, 1
		);

	public static Matrix Scale(in Vector s) =>
		new Matrix(
			s.x, 0, 0, 0,
			0, s.y, 0, 0,
			0, 0, s.z, 0,
			0, 0, 0, 1
		);

	public static Matrix RotateX(double alpha)
	{
		var (s, c) = Math.SinCos(alpha);

		return new Matrix(
			1, 0,  0, 0,
			0, c, -s, 0,
			0, s,  c, 0,
			0, 0,  0, 1
		);
	}

	public static Matrix RotateY(double alpha)
	{
		var (s, c) = Math.SinCos(alpha);

		return new Matrix(
			c, 0, -s, 0,
			0, 1,  0, 0,
			s, 0,  c, 0,
			0, 0,  0, 1
		);
	}

	public static Matrix RotateZ(double alpha)
	{
		var (s, c) = Math.SinCos(alpha);

		return new Matrix(
			c, -s, 0, 0,
			s,  c, 0, 0,
			0,  0, 1, 0,
			0,  0, 0, 1
		);
	}

	public Matrix Transpose =>
        new Matrix(
            m11, m21, m31, m41,
            m12, m22, m32, m42,
            m13, m23, m33, m43,
            m14, m24, m34, m44
        );

	public Matrix Inverse
	{
        get
        {
            var inv11 =
                     m22 * m33 * m44 - 
                     m22 * m34 * m43 - 
                     m32 * m23 * m44 + 
                     m32 * m24 * m43 +
                     m42 * m23 * m34 - 
                     m42 * m24 * m33;

            var inv21 =
                   -m21 * m33 * m44 + 
                    m21 * m34 * m43 + 
                    m31 * m23  * m44 - 
                    m31 * m24  * m43 - 
                    m41 * m23  * m34 + 
                    m41 * m24  * m33;

            var inv31 =
                    m21 * m32 * m44 - 
                    m21 * m34 * m42 - 
                    m31 * m22 * m44 + 
                    m31 * m24 * m42 + 
                    m41 * m22 * m34 - 
                    m41 * m24 * m32;

            var inv41 =
                   -m21 * m32 * m43 + 
                    m21 * m33 * m42 +
                    m31 * m22 * m43 - 
                    m31 * m23 * m42 - 
                    m41 * m22 * m33 + 
                    m41 * m23 * m32;

            var inv12 =
                   -m12 * m33 * m44 + 
                    m12 * m34 * m43 + 
                    m32 * m13 * m44 - 
                    m32 * m14 * m43 - 
                    m42 * m13 * m34 + 
                    m42 * m14 * m33;

            var inv22 =
                    m11 * m33 * m44 - 
                    m11 * m34 * m43 - 
                    m31 * m13 * m44 + 
                    m31 * m14 * m43 + 
                    m41 * m13 * m34 - 
                    m41 * m14 * m33;

            var inv32 =
                   -m11 * m32 * m44 + 
                    m11 * m34 * m42 + 
                    m31 * m12 * m44 - 
                    m31 * m14 * m42 - 
                    m41 * m12 * m34 + 
                    m41 * m14 * m32;

            var inv42 =
                    m11 * m32 * m43 - 
                    m11 * m33 * m42 - 
                    m31 * m12 * m43 + 
                    m31 * m13 * m42 + 
                    m41 * m12 * m33 - 
                    m41 * m13 * m32;

            var inv13 =
                    m12 * m23 * m44 - 
                    m12 * m24 * m43 - 
                    m22 * m13 * m44 + 
                    m22 * m14 * m43 + 
                    m42 * m13 * m24 - 
                    m42 * m14 * m23;

            var inv23 =
                   -m11 * m23 * m44 + 
                    m11 * m24 * m43 + 
                    m21 * m13 * m44 - 
                    m21 * m14 * m43 - 
                    m41 * m13 * m24 + 
                    m41 * m14 * m23;

            var inv33 =
                    m11 * m22 * m44 - 
                    m11 * m24 * m42 - 
                    m21 * m12 * m44 + 
                    m21 * m14 * m42 + 
                    m41 * m12 * m24 - 
                    m41 * m14 * m22;

            var inv43 =
                   -m11 * m22 * m43 + 
                    m11 * m23 * m42 + 
                    m21 * m12 * m43 - 
                    m21 * m13 * m42 - 
                    m41 * m12 * m23 + 
                    m41 * m13 * m22;

            var inv14 =
                   -m12 * m23 * m34 + 
                    m12 * m24 * m33 + 
                    m22 * m13 * m34 - 
                    m22 * m14 * m33 - 
                    m32 * m13 * m24 + 
                    m32 * m14 * m23;

            var inv24 =
                    m11 * m23 * m34 - 
                    m11 * m24 * m33 - 
                    m21 * m13 * m34 + 
                    m21 * m14 * m33 + 
                    m31 * m13 * m24 - 
                    m31 * m14 * m23;

            var inv34 =
                   -m11 * m22 * m34 + 
                    m11 * m24 * m32 + 
                    m21 * m12 * m34 - 
                    m21 * m14 * m32 - 
                    m31 * m12 * m24 + 
                    m31 * m14 * m22;

            var inv44 =
                    m11 * m22 * m33 - 
                    m11 * m23 * m32 - 
                    m21 * m12 * m33 + 
                    m21 * m13 * m32 + 
                    m31 * m12 * m23 - 
                    m31 * m13 * m22;

            var det = m11 * inv11 + m12 * inv21 + m13 * inv31 + m14 * inv41;

            det = 1.0 / det;

            return new Matrix(
                inv11 * det, inv12 * det, inv13 * det, inv14 * det,
                inv21 * det, inv22 * det, inv23 * det, inv24 * det,
                inv31 * det, inv32 * det, inv33 * det, inv34 * det,
                inv41 * det, inv42 * det, inv43 * det, inv44 * det
            );
        }
	}

	public static Matrix operator *(in Matrix a, in Matrix b) =>
		new Matrix(
			a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * b.m31 + a.m14 * b.m41,
			a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * b.m32 + a.m14 * b.m42,
			a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * b.m33 + a.m14 * b.m43,
			a.m11 * b.m14 + a.m12 * b.m24 + a.m13 * b.m34 + a.m14 * b.m44,

			a.m21 * b.m11 + a.m22 * b.m21 + a.m23 * b.m31 + a.m24 * b.m41,
			a.m21 * b.m12 + a.m22 * b.m22 + a.m23 * b.m32 + a.m24 * b.m42,
			a.m21 * b.m13 + a.m22 * b.m23 + a.m23 * b.m33 + a.m24 * b.m43,
			a.m21 * b.m14 + a.m22 * b.m24 + a.m23 * b.m34 + a.m24 * b.m44,
			
			a.m31 * b.m11 + a.m32 * b.m21 + a.m33 * b.m31 + a.m34 * b.m41,
			a.m31 * b.m12 + a.m32 * b.m22 + a.m33 * b.m32 + a.m34 * b.m42,
			a.m31 * b.m13 + a.m32 * b.m23 + a.m33 * b.m33 + a.m34 * b.m43,
			a.m31 * b.m14 + a.m32 * b.m24 + a.m33 * b.m34 + a.m34 * b.m44,
			
			a.m41 * b.m11 + a.m42 * b.m21 + a.m43 * b.m31 + a.m44 * b.m41,
			a.m41 * b.m12 + a.m42 * b.m22 + a.m43 * b.m32 + a.m44 * b.m42,
			a.m41 * b.m13 + a.m42 * b.m23 + a.m43 * b.m33 + a.m44 * b.m43,
			a.m41 * b.m14 + a.m42 * b.m24 + a.m43 * b.m34 + a.m44 * b.m44
		);

	private bool PrintMembers(StringBuilder builder)
    {
        builder.Append($"{m11:0.##} ");
        builder.Append($"{m12:0.##} ");
        builder.Append($"{m13:0.##} ");
        builder.Append($"{m14:0.##} | ");
		
        builder.Append($"{m21:0.##} ");
        builder.Append($"{m22:0.##} ");
        builder.Append($"{m23:0.##} ");
        builder.Append($"{m24:0.##} | ");
		
        builder.Append($"{m31:0.##} ");
        builder.Append($"{m32:0.##} ");
        builder.Append($"{m33:0.##} ");
        builder.Append($"{m34:0.##} | ");
		
        builder.Append($"{m41:0.##} ");
        builder.Append($"{m42:0.##} ");
        builder.Append($"{m43:0.##} ");
        builder.Append($"{m44:0.##}");
		return true;
	}
}
