//   MultiGeometryAnimator.cs
//
//  Author:
//       Allis Tauri <allista@gmail.com>
//
//  Copyright (c) 2016 Allis Tauri

using System.Collections.Generic;


namespace AT_Utils
{
	/// <summary>
	/// This variant of MultiAnimator implements the MultipleDragCube interface
	/// and should be used to animate geometry changes, e.g. moving doors, etc.
	/// </summary>
	public class MultiGeometryAnimator : MultiAnimator, IMultipleDragCube
	{
		const string A = "Opened";
		const string B = "Closed";
		static readonly string[] cube_names = {A, B};
		static readonly Dictionary<string,int> cube_positions = new Dictionary<string, int>{{ A, 1 }, { B, 0 }};
		public string[] GetDragCubeNames() { return cube_names; }

		public void AssumeDragCubePosition(string anim)
		{
			setup_animation();
			int pos;
			if(!cube_positions.TryGetValue(anim, out pos)) pos = 0;
			seek(pos, false);
		}
		public bool UsesProceduralDragCubes() { return false; }

		protected override void on_progress(float p)
		{
			part.DragCubes.SetCubeWeight(A, p);
			part.DragCubes.SetCubeWeight(B, 1f - p);
			if(part.DragCubes.Procedural)
				part.DragCubes.ForceUpdate(true, true, false);
		}
	}
}

