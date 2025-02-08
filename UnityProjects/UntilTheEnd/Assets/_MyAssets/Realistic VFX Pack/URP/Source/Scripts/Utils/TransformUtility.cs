using UnityEngine;


namespace RFX.Utils
{

	public static class TransformUtility
	{
		public static Transform[] GetAllChildren(this Transform parent)
		{
			Transform[] children = new Transform[parent.childCount];

			for (int i = 0; i < parent.childCount; i++)
			{
				children[i] = parent.GetChild(i);
			}

			return children;
		}
	}

}