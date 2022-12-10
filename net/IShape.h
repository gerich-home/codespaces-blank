#pragma once


namespace Engine
{
	interface IShape
	{
		HitPoint Intersection(Vector start, Vector direction);
	};
}
