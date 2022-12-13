
using namespace Engine;

Shapes.Plane.Plane(Vector normal, double d, IMaterial material) :
	normal(normal),
	d(d),
	A( (normal.x != 0 ? -d / normal.x : 0                                              ),
	   (normal.x != 0 ? 0             : (normal.y != 0 ? -d / normal.y : 0            )),
	   (normal.x != 0 ? 0             : (normal.y != 0 ? 0             : -d / normal.z))
	   ),
	material(material)
{
}

Shapes.Plane.Plane(Vector a, Vector b, Vector A, IMaterial material) :
	normal(a.CrossProduct(b).Normalized),
	A(A),
	material(material)
{
	d = A.DotProduct(normal);
}

Shapes.Plane.Plane(double a, double b, double c, double d, IMaterial material):
	normal(new Vector(a, b, c).Normalized),
	d(d),
	material(material),
	A( (a != 0 ? -d / a : 0                         ),
	   (a != 0 ? 0      : (b != 0 ? -d / b : 0     )),
	   (a != 0 ? 0      : (b != 0 ? 0      : -d / c))
	   )
{
}

HitPoint Shapes.Plane.Intersection(Vector start, Vector direction)
{
	double divident = normal.DotProduct(direction);

	if(divident == 0)
	{
		return NULL;
	}

	double t = (start.DotProduct(normal) + d) / normal.DotProduct(direction);

	if(t < 0)
	{
		return NULL;
	}

	return new HitPoint(t, normal, material);
}