//   MultiGeometryAnimator.cs
//
//  Author:
//       Allis Tauri <allista@gmail.com>
//
//  Copyright (c) 2016 Allis Tauri

using System.Collections.Generic;
using UnityEngine;

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

		[KSPField] 
		[SerializeField]
		public VectorCurve CoMCurve = new VectorCurve();

		public void AssumeDragCubePosition(string anim)
		{
			setup_animation();
			int pos;
			if(!cube_positions.TryGetValue(anim, out pos)) pos = 0;
			seek(pos, false);
		}
		public bool UsesProceduralDragCubes() { return false; }
		public bool IsMultipleCubesActive { get { return true; } }

		protected override void on_norm_time(float t)
		{
			part.DragCubes.SetCubeWeight(A, t);
			part.DragCubes.SetCubeWeight(B, 1-t);
			if(part.DragCubes.Procedural)
				part.DragCubes.ForceUpdate(true, true, false);
			if(CoMCurve.Length > 0) part.CoMOffset = CoMCurve.Evaluate(t);
		}

		public void UpdateCoMOffset()
		{
			if(CoMCurve.Length == 0) return;
			part.CoMOffset = CoMCurve.Evaluate(ntime);
		}
	}

	public class GeometryAnimatorUpdater : ModuleUpdater<MultiGeometryAnimator>
	{ 
		protected override void on_rescale(ModulePair<MultiGeometryAnimator> mp, Scale scale)
		{ 
			mp.module.EnergyConsumption = mp.base_module.EnergyConsumption * scale.absolute.quad * scale.absolute.aspect;
			if(mp.module.CoMCurve != null)
			{
				mp.module.CoMCurve.Scale(scale.ScaleVectorRelative(Vector3d.one)); 
				mp.module.UpdateCoMOffset();
			}
		}
	}
}

