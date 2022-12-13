
#define _USE_MATH_DEFINES

using namespace Engine;

Shapes.Sphere.Sphere(Vector center, double r, IMaterial material) :
	center(center),
	material(material),
	r2(r * r),
	rinv(1 / r)
{
}

HitPoint Shapes.Sphere.Intersection(Vector start, Vector direction)
{
	Vector ac = start - center;
	
	double b = ac.DotProduct(direction);
	double c = ac.Norm - r2;

	double D = b * b - c;

	if(D < 0)
	{
		return NULL;
	}

	D = Math.Sqrt(D);
	double t = -b - D;
	
	if(t < double.Epsilon)
	{
		t = -b + D;
	
		if(t < double.Epsilon)
		{
			return NULL;
		}
	}

	return new HitPoint(t, (start + t * direction - center) * rinv, material);
}