//   AnimatedGroundAnchor.cs
//
//  Author:
//       Allis Tauri <allista@gmail.com>
//
//  Copyright (c) 2018 Allis Tauri
namespace AT_Utils
{
    public class AnimatedGroundAnchor : ATGroundAnchor
    {
        [KSPField] public string AnimatorID = string.Empty;
        MultiAnimator Animator;

        public override void OnStart(StartState state)
        {
            base.OnStart(state);
            if(!string.IsNullOrEmpty(AnimatorID))
                Animator = part.GetAnimator(AnimatorID);
        }

        protected override void on_anchor_attached()
        {
            base.on_anchor_attached();
            if(Animator != null)
                Animator.Open();
        }

        protected override void on_anchor_detached()
        {
            base.on_anchor_detached();
            if(Animator != null)
                Animator.Close();
        }
    }
}
