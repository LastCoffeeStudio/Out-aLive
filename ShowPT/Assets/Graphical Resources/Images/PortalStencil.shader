Shader "Unlit/PortalStencil"
{
	SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

		ColorMask 0
		ZWrite Off

		Pass{
			Stencil{
				Ref 5
				Comp Always
				Pass Replace
			}
		}
	}
}
