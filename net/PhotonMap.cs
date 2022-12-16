namespace Engine;

public class PhotonMap
{
	private PhotonMapNode root;
	private Photon[] photons;
	private int current;
	private readonly int[][] indexes = new int[3][];

	public PhotonMap(int nphotons)
	{
		photons = new Photon[nphotons];
		current = 0;
	}

	public void QSort(int left, int right, short axis)
	{
		int l = left;
		int r = right;
		int m = left;

		while (l <= r)
		{
			while (photons[indexes[axis][l]].point[axis] < photons[indexes[axis][m]].point[axis] && l < right) l++;
			while (photons[indexes[axis][m]].point[axis] < photons[indexes[axis][r]].point[axis] && r > left) r--;

			if (l <= r)
			{
				int tmp = indexes[axis][l];
				indexes[axis][l] = indexes[axis][r];
				indexes[axis][r] = tmp;
				l++;
				r--;
			}
		}

		if (left < r) QSort(left, r, axis);
		if (l < right) QSort(l, right, axis);
	}

	public PhotonMapNode CreateSubTree(int left, int right)
	{
		int count = right - left + 1;

		if(count == 1)
		{
			return new PhotonMapNode(photons[indexes[0][left]], null, null, -1);
		}
		else
		{
			double[] distance = new double[3];
			distance[0] = photons[indexes[0][right]].point.x - photons[indexes[0][left]].point.x;
			distance[1] = photons[indexes[1][right]].point.y - photons[indexes[1][left]].point.y;
			distance[2] = photons[indexes[2][right]].point.z - photons[indexes[2][left]].point.z;

			//axis = 0 for x, 1 for y, 2 for z
			short axis;
			short forSort_axis1;
			short forSort_axis2;

			if(distance[0] > distance[1])
			{
				forSort_axis1 = 1;
				if(distance[0] > distance[2])
				{
					axis = 0;
					forSort_axis2 = 2;
				}
				else
				{
					axis = 2;
					forSort_axis2 = 0;
				}
			}
			else 
			{
				forSort_axis1 = 0;
				if(distance[1] > distance[2])
				{
					axis = 1;
					forSort_axis2 = 2;
				}
				else
				{
					axis = 2;
					forSort_axis2 = 1;
				}
			}

			for(int i = left; i <= right; i++)
			{
				indexes[forSort_axis1][i] = indexes[axis][i];
				indexes[forSort_axis2][i] = indexes[axis][i];
			}

			int m = left + (right - left) / 2;
			int mediana = indexes[axis][m];
			
			if(left == right - 1)
			{
				return new PhotonMapNode(photons[mediana], null, CreateSubTree(m + 1, right), axis);
			}
			
			QSort(left, m - 1, forSort_axis1);
			QSort(left, m - 1, forSort_axis2);
			QSort(m + 1, right, forSort_axis1);
			QSort(m + 1, right, forSort_axis2);
		
			return new PhotonMapNode(photons[mediana], CreateSubTree(left, m - 1), CreateSubTree(m + 1, right), axis);
		}
	}

	public void Build()
	{
		indexes[0] = new int[photons.Length];
		indexes[1] = new int[photons.Length];
		indexes[2] = new int[photons.Length];

		for(int i = 0; i < photons.Length; i++)
		{
			indexes[0][i] = i;
			indexes[1][i] = i;
			indexes[2][i] = i;
		}

		int last_index = photons.Length - 1;
		QSort(0, last_index, 0);
		QSort(0, last_index, 1);
		QSort(0, last_index, 2);
		root = CreateSubTree(0, last_index);
		
		indexes[0] = null;
		indexes[1] = null;
		indexes[2] = null;
		photons = null;
	}

	public bool Add(Photon photon)
	{
		photons[current] = photon;

		return (++current < photons.Length);
	}

	public void Clear()
	{
		root = null;
	}

	public void GoDown(PhotonMapNode node, ParamsForFind paramsForFind)
	{
		short axis = node.axis;
		
		double delta = paramsForFind.x[axis] - node.photon.point[axis];

		if(delta <= 0)
		{
			if(node.left != null)
			{
				GoDown(node.left, paramsForFind);

				if(delta * delta < paramsForFind.r2)
				{
					if(node.right != null)
					{
						GoDown(node.right, paramsForFind);
					}
				}
			}
		}
		else
		{
			if(node.right != null)
			{
				GoDown(node.right, paramsForFind);

				if(delta * delta < paramsForFind.r2)
				{
					if(node.left != null)
					{
						GoDown(node.left, paramsForFind);
					}
				}
			}
		}

		delta = (node.photon.point - paramsForFind.x).Norm;

		if(paramsForFind.currentCount < paramsForFind.count)
		{
			paramsForFind.result[paramsForFind.currentCount] = node.photon;
			paramsForFind.distances[paramsForFind.currentCount] = delta;
			paramsForFind.currentCount++;
			if(delta > paramsForFind.r2)
			{
				paramsForFind.r2 = delta;
			}
		}
		else if(delta < paramsForFind.r2)
		{
			for(int i = 0; i < paramsForFind.count; i++)
			{
				if(paramsForFind.distances[i] == paramsForFind.r2)
				{
					paramsForFind.result[i] = node.photon;
					paramsForFind.distances[i] = delta;
					paramsForFind.r2 = delta;
					break;
				}
			}
		}
	}

	public double FindNearest(Vector x, int count, Photon[] result)
	{
		ParamsForFind p = new ParamsForFind(x, count, result);
		GoDown(root, p);
		return p.r2;
	}

}

public record class PhotonMapNode(Photon photon, PhotonMapNode left, PhotonMapNode right, short axis);

public class ParamsForFind
{
	public ParamsForFind(Vector x, int count, Photon[] result)
	{
		this.x = x;
		this.count = count;
		this.result = result;
		distances = new double[count];
	}

	public readonly Vector x;
	public readonly int count; 
	public int currentCount; 
	public readonly Photon[] result; 
	public readonly double[] distances;
	public double r2;
}
