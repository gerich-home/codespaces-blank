
using namespace Engine;

Shapes.Square.Square(Vector a, Vector b, Vector c, ITexturedMaterial material):
	a(a),
	ba(b - a),
	ca(c - a),
	normal((b - a).CrossProduct(c - a)),
	n((b - a).CrossProduct(c - a).Normalized),
	material(material)
{
}
	
Shapes.Square.Square(Vector a, Vector b, Vector c, IMaterial material):
	a(a),
	ba(b - a),
	ca(c - a),
	normal((b - a).CrossProduct(c - a)),
	n((b - a).CrossProduct(c - a).Normalized),
	material(new Materials.TexturedMaterialAdapter(material))
{
}

HitPoint Shapes.Square.Intersection(Vector start, Vector direction)
{
	double t = 0;
	double t1 = 0;
	double t2 = 0;
	

	double divident = - direction.DotProduct(normal);
	
	if(!divident)
	{
		return NULL;
	}

	double factor = 1 / divident;

	Vector sa = start - a;
	Vector saxdir = sa.CrossProduct(direction);
	t1 = -ba.DotProduct(saxdir) * factor;
	
	if((t1 < 0) || (1 < t1))
	{
		return NULL;
	}
				
	t2 = ca.DotProduct(saxdir) * factor;
	
	if((t2 < 0) || (1 < t2))
	{
		return NULL;
	}
	
	t = sa.DotProduct(normal) * factor;

	if(t < 100 * double.Epsilon)
	{
		return NULL;
	}

	return new HitPoint(t, n, material.MaterialAt(t1, t2));
}