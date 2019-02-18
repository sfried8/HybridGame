using System;
using System.Collections;
using UnityEngine;
[System.Serializable]
public class Point : IEquatable<Point>
{

	[SerializeField]
	private int x;
	public int X
	{
		get
		{
			return x;
		}
		set
		{
			x = value;
		}
	}
	public int Col
	{
		get
		{
			return x;
		}
		set
		{
			x = value;
		}
	}

	[SerializeField]
	private int y;
	public int Y
	{
		get
		{
			return y;
		}
		set
		{
			y = value;
		}
	}
	public int Row
	{
		get
		{
			return y;
		}
		set
		{
			y = value;
		}
	}

	[SerializeField]
	private int z;
	public int Z
	{
		get
		{
			return z;
		}
		set
		{
			z = value;
		}
	}
	public int Plane
	{
		get
		{
			return z;
		}
		set
		{
			z = value;
		}
	}
	public static Point GridCoord (int row, int col)
	{
		return new Point (col, row);
	}
	public static Point ZERO ()
	{
		return new Point (0, 0, 0);
	}
	public static Point ONE ()
	{
		return new Point (1, 1, 1);
	}
	public Vector3 ToVector3 ()
	{
		return new Vector3 (x, y, z);
	}
	public Point (int x, int y, int z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}
	public Point (int x, int y)
	{
		this.x = x;
		this.y = y;
		this.z = 0;
	}
	public static bool operator == (Point obj1, Point obj2)
	{
		if (ReferenceEquals (obj1, obj2))
		{
			return true;
		}

		if (ReferenceEquals (obj1, null))
		{
			return false;
		}
		if (ReferenceEquals (obj2, null))
		{
			return false;
		}

		return (obj1.X == obj2.X &&
			obj1.Y == obj2.Y &&
			obj1.Z == obj2.Z);
	}

	// this is second one '!='
	public static bool operator != (Point obj1, Point obj2)
	{
		return !(obj1 == obj2);
	}

	public bool Equals (Point other)
	{
		if (ReferenceEquals (null, other))
		{
			return false;
		}
		if (ReferenceEquals (this, other))
		{
			return true;
		}

		return Z.Equals (other.Z) &&
			X.Equals (other.X) &&
			Y.Equals (other.Y);
	}

	public override bool Equals (object obj)
	{
		if (ReferenceEquals (null, obj))
		{
			return false;
		}
		if (ReferenceEquals (this, obj))
		{
			return true;
		}

		return obj.GetType () == GetType () && Equals ((Point) obj);
	}

	public override int GetHashCode ()
	{
		unchecked
		{
			int hashCode = x.GetHashCode ();
			hashCode = (hashCode * 397) ^ y.GetHashCode ();
			hashCode = (hashCode * 397) ^ z.GetHashCode ();
			return hashCode;
		}
	}

}