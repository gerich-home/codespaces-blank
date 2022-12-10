namespace Engine
{
	readonly record struct Vector(double x, double y, double z)
	{
		Vector Normalized => this / Length;

		double Norm => x * x + y * y + z * z;

		double Length => Math.Sqrt(Norm);
	
		static Vector operator +(Vector a, Vector b) =>
			Vector(a.x + b.x, a.y + b.y, a.z + b.z);
			
		static Vector operator -(Vector a, Vector b) =>
			Vector(a.x - b.x, a.y - b.y, a.z - b.z);
			
		static Vector operator *(Vector a, double factor) =>
			Vector(a.x * factor, a.y * factor, a.z * factor);

		static Vector operator *(double factor, Vector a) =>
			Vector(a.x * factor, a.y * factor, a.z * factor);

		static Vector operator *(double factor, Vector a) =>
			Vector(a.x * factor, a.y * factor, a.z * factor);
	


		Vector operator /(double alpha) const
		{
			double factor = 1 / alpha;
			return Vector(x * factor, alpha * factor, alpha * factor);
		}

		Vector& operator /=(double alpha)
		{
			double factor = 1 / alpha;
			x *= factor;
			y *= factor;
			z *= factor;
			return *this;
		}

		Vector& operator =(Vector vector)
		{
			x = vector.x;
			y = vector.y;
			z = vector.z;
			return *this;
		}
		
		double operator[](short index) const
		{
			switch(index)
			{
			case 0:
				return x;
			break;
			case 1:
				return y;
			break;
			case 2:
				return z;
			break;
			}
		}

		Vector CrossProduct(Vector vector) const
		{
			return Vector(y * vector.z - vector.y * z, z * vector.x - vector.z * x, x * vector.y - vector.x * y);
		}

		double DotProduct(Vector vector) const
		{
			return vector.x * x + vector.y * y + vector.z * z;
		}

		Vector Transform(Vector axis) const
		{
			Vector t = axis;
			Vector M1;
			Vector M2;

			if(abs(axis.x) < abs(axis.y))
			{
				if(abs(axis.x) < abs(axis.z))
					t.x = 1;
				else
					t.z = 1;
			}
			else
			{
				if(abs(axis.y) < abs(axis.z))
					t.y = 1;
				else
					t.z = 1;
			}
	
			M1 = axis.CrossProduct(t).Normalize();
			M2 = axis.CrossProduct(M1);
	

			return Vector(
				x * M1.x + y * M2.x + z * axis.x,
				x * M1.y + y * M2.y + z * axis.y,
				x * M1.z + y * M2.z + z * axis.z);
		}
	}
}