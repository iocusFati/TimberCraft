Shader "Hidden/Cibbi's shaders/Toony Standard/Outlined" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		[Normal] _BumpMap ("Normal Map", 2D) = "bump" {}
		_BumpScale ("Normal Scale", Float) = 1
		_EmissionMap ("Emission Map", 2D) = "white" {}
		[HDR] _EmissionColor ("Emission Color", Vector) = (0,0,0,1)
		_OcclusionMap ("Occlusion Map", 2D) = "white" {}
		_Occlusion ("Occlusion", Range(0, 1)) = 1
		_MSOT ("MSOT map", 2D) = "white" {}
		_Ramp ("Ramp Texture", 2D) = "white" {}
		_RampColor ("Ramp Color", Vector) = (1,1,1,1)
		_ShadowIntensity ("Shadow Intensity", Range(0, 1)) = 0.7
		_RampOffset ("Ramp Offset", Range(-1, 1)) = 0
		_OcclusionOffsetIntensity ("Occlusion Offset", Range(0, 1)) = 0
		_RimStrength ("Rim Strength", Range(0, 1)) = 0
		_RimSharpness ("Rim Sharpness", Range(0, 1)) = 0
		_RimIntensity ("Rim Intensity", Range(-1, 1)) = 0
		[HDR] _RimColor ("Rim Color", Vector) = (1,1,1,1)
		_Metallic ("Metallic", Range(0, 1)) = 0
		_MetallicMap ("Metallic Map", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0, 1)) = 0
		_GlossinessMap ("Smoothness Map", 2D) = "white" {}
		_TangentMap ("Tangent Map", 2D) = "white" {}
		_Anisotropy ("Anisotropy", Range(-1, 1)) = 0
		_AnisotropyMap ("Anisotropy Map", 2D) = "white" {}
		_FakeHighlights ("Fake Highlights", 2D) = "black" {}
		_Matcap ("Matcap", 2D) = "white" {}
		_Cubemap ("Cubemap", Cube) = "" {}
		[HDR] _IndirectColor ("Color", Vector) = (0.7,0.7,0.7,1)
		_HighlightPattern ("Highlight Pattern", 2D) = "white" {}
		_HighlightRamp ("Highlight Ramp Texture", 2D) = "white" {}
		[HDR] _HighlightRampColor ("Highlight Ramp Color", Vector) = (1,1,1,1)
		_HighlightRampOffset ("Highlight Ramp Offset", Range(-1, 1)) = 0
		_HighlightIntensity ("Highlight Intensity", Range(0, 1)) = 1
		_FakeHighlightIntensity ("Fake Highlight Intensity", Range(0, 1)) = 0.5
		_DetailMask ("Detail Mask", 2D) = "white" {}
		_DetailIntensity ("Detail Intensity", Range(0, 1)) = 1
		_DetailTexture ("Albedo (RGB)", 2D) = "white" {}
		_DetailColor ("Color", Vector) = (1,1,1,1)
		[Normal] _DetailBumpMap ("Detail Normal Map", 2D) = "bump" {}
		_DetailBumpScale ("Detail Normal Scale", Float) = 1
		_ThicknessMap ("Thickness Map", 2D) = "white" {}
		_SSColor ("Subsurface Color", Vector) = (0,0,0,0)
		_SSDistortion ("Distortion", Range(0, 3)) = 1
		_SSPower ("Power", Range(0, 3)) = 1
		_SSScale ("Scale", Range(0, 3)) = 1
		[IntRange] _StencilID ("Stencil ID (0-255)", Range(0, 255)) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comparison", Float) = 0
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilOp ("Stencil Operation", Float) = 0
		[IntRange] _OutlineStencilID ("Stencil ID (0-255)", Range(0, 255)) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _OutlineStencilComp ("Stencil Comparison", Float) = 0
		[Enum(UnityEngine.Rendering.StencilOp)] _OutlineStencilOp ("Stencil Operation", Float) = 0
		_OutlineWidthMap ("Outline Thickness Map", 2D) = "white" {}
		[IntRange] _OutlineWidth ("Outline Width", Range(0, 10)) = 2
		[IntRange] _OutlineOffsetX ("Outline Offset X", Range(-10, 10)) = 0
		[IntRange] _OutlineOffsetY ("Outline Offset Y", Range(-10, 10)) = 0
		_OutlineTexture ("Color", 2D) = "white" {}
		_OutlineColor ("Outline Color", Vector) = (0,0,0,1)
		_IsOutlineEmissive ("Emissive Outline", Float) = 0
		_Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
		[Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull Mode", Float) = 2
		[HideInInspector] _Mode ("__Mode", Float) = 0
		[HideInInspector] _SpMode ("__SpMode", Float) = 0
		[HideInInspector] _IndirectSpecular ("__IndirectSpecular", Float) = 0
		[HideInInspector] _Workflow ("__Workflow", Float) = 0
		[HideInInspector] _SrcBlend ("__src", Float) = 1
		[HideInInspector] _DstBlend ("__dst", Float) = 0
		[HideInInspector] _ZWrite ("__zw", Float) = 1
		[HideInInspector] _MainRampMin ("__MainRampMin", Vector) = (0.001,0.001,0.001,0.001)
		[HideInInspector] _MainRampMax ("__MainRampMax", Vector) = (1,1,1,1)
		[HideInInspector] _ToonyHighlights ("__ToonyHighlights", Float) = 0
		[HideInInspector] _OcclusionOffset ("__OcclusionOffset", Float) = 0
		[HideInInspector] _IndirectOverride ("__IndirectOverride", Float) = 0
		[HideInInspector] _EmissiveRim ("__EmissiveRim", Float) = 0
		[HideInInspector] _RampOn ("__RampOn", Float) = 1
		[HideInInspector] _RimLightOn ("__RimLight", Float) = 0
		[HideInInspector] _SpecularOn ("__EnableSpecular", Float) = 0
		[HideInInspector] _DetailMapOn ("__DetailMap", Float) = 0
		[HideInInspector] _SSSOn ("__SSSOn", Float) = 0
		[HideInInspector] _StencilOn ("__StencilOn", Float) = 0
		[HideInInspector] _OutlineOn ("__OutlineOn", Float) = 0
		[HideInInspector] _ToonRampBox ("__ToonRampBox", Float) = 1
		[HideInInspector] _RimLightBox ("__RimLightBox", Float) = 0
		[HideInInspector] _SpecularBox ("__SpecularBox", Float) = 0
		[HideInInspector] _DetailBox ("__DetailBox", Float) = 0
		[HideInInspector] _SSSBox ("__SSSBox", Float) = 0
		[HideInInspector] _StencilBox ("__StencilBox", Float) = 0
		[HideInInspector] _OutlineBox ("__OutlineBox", Float) = 0
		[HideInInspector] _NeedsFix ("__NeedsFix", Float) = 0.5
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	//CustomEditor "Cibbi.ToonyStandard.ToonyStandardGUI"
}