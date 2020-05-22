Shader "Custom/Test_1"
{
	SubShader
	{
		Tags { "Queue" = "Geometry-1" }
		Lighting Off
		Pass
		{
			ZWrite On
			ZTest LEqual
			ColorMask 0
		}
	}
}
