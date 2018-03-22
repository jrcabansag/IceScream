using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData {

	private static int level1Rating = 0;
	private static int level2Rating = 0;
	private static int level3Rating = 0;
	private static int level4Rating = 0;
	private static bool iceCreamHat = false;
	private static bool pixieCut = false;
	private static bool zombified = false;
	private static bool daisyHat = false;

	public static int Level1Rating
	{
		get
		{
			return level1Rating;
		}
		set
		{
			level1Rating = value;
		}
	}

	public static int Level2Rating
	{
		get
		{
			return level2Rating;
		}
		set
		{
			level2Rating = value;
		}
	}

	public static int Level3Rating
	{
		get
		{
			return level3Rating;
		}
		set
		{
			level3Rating = value;
		}
	}

	public static int Level4Rating
	{
		get
		{
			return level4Rating;
		}
		set
		{
			level4Rating = value;
		}
	}

	public static bool IceCreamHat
	{
		get
		{
			return iceCreamHat;
		}
		set
		{
			iceCreamHat = value;
		}
	}

	public static bool PixieCut
	{
		get
		{
			return pixieCut;
		}
		set
		{
			pixieCut = value;
		}
	}

	public static bool Zombified
	{
		get
		{
			return zombified;
		}
		set
		{
			zombified = value;
		}
	}

	public static bool DaisyHat
	{
		get
		{
			return daisyHat;
		}
		set
		{
			daisyHat = value;
		}
	}
}
