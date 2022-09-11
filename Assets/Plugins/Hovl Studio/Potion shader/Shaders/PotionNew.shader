Shader "HOVL/Potion"
{
	Properties
	{
		_Mask("Mask", 2D) = "white" {}
		_MainTex("MainTex", 2D) = "white" {}
		_Opacity("Opacity", Range( 0 , 1)) = 1
		_Color("Color", Color) = (1,0,0,1)
		_MasktilingXY("Mask tiling XY", Vector) = (0,-0.25,0,0)
		_Fullnes("Fullnes", Range( 0 , 1)) = 0.35
		_Wavesspeed("Waves speed", Float) = 5
		_Wavesdistance("Waves distance", Float) = 20
		_Wavespower("Waves power", Float) = 50
		_Pivotpoint("Pivot point", Float) = -0.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest LEqual
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass
		{
			Name "Default"
		CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_CLIP_RECT
			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			#include "UnityShaderVariables.cginc"

			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _TextureSampleAdd;
			uniform float4 _ClipRect;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float _Opacity;
			uniform sampler2D _Mask;
			uniform float4 _MasktilingXY;
			uniform float4 _Mask_ST;
			uniform float4 _Color;
			uniform float _Wavesdistance;
			uniform float _Wavesspeed;
			uniform float _Fullnes;
			uniform float _Wavespower;
			uniform float _Pivotpoint;
			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID( IN );
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				OUT.worldPosition = IN.vertex;
				
				
				OUT.worldPosition.xyz +=  float3( 0, 0, 0 ) ;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN  ) : SV_Target
			{
				float2 uv_MainTex = IN.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 appendResult23 = (float2(_MasktilingXY.x , _MasktilingXY.y));
				float2 uv0_Mask = IN.texcoord.xy * _Mask_ST.xy + _Mask_ST.zw;
				float2 panner20 = ( 1.0 * _Time.y * appendResult23 + uv0_Mask);
				float2 uv079 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float mulTime26 = _Time.y * _Wavesspeed;
				float temp_output_28_0 = ( sin( ( ( uv079.x * _Wavesdistance ) + mulTime26 ) ) + (((0.0 + (_Wavespower - 0.0) * (-1.0 - 0.0) / (1.0 - 0.0)) + (_Fullnes - 0.0) * (0.0 - (0.0 + (_Wavespower - 0.0) * (-1.0 - 0.0) / (1.0 - 0.0))) / (1.0 - 0.0)) + (( 1.0 - uv079.y ) - 0.0) * ((0.0 + (_Fullnes - 0.0) * (_Wavespower - 0.0) / (1.0 - 0.0)) - ((0.0 + (_Wavespower - 0.0) * (-1.0 - 0.0) / (1.0 - 0.0)) + (_Fullnes - 0.0) * (0.0 - (0.0 + (_Wavespower - 0.0) * (-1.0 - 0.0) / (1.0 - 0.0))) / (1.0 - 0.0))) / (1.0 - 0.0)) );
				float2 appendResult78 = (float2(-1.0 , _Pivotpoint));
				float temp_output_38_0 = (-4.0 + (length( (appendResult78 + (uv079 - float2( 0,0 )) * (float2( 1,1 ) - appendResult78) / (float2( 1,1 ) - float2( 0,0 ))) ) - 0.0) * (2.0 - -4.0) / (1.0 - 0.0));
				float clampResult32 = clamp( ( temp_output_28_0 - temp_output_38_0 ) , 0.0 , 1.0 );
				float clampResult44 = clamp( ( temp_output_28_0 + temp_output_38_0 ) , 0.0 , 1.0 );
				float clampResult56 = clamp( ( clampResult32 - clampResult44 ) , 0.0 , 1.0 );
				float4 clampResult55 = clamp( ( ( tex2D( _Mask, panner20 ) * _Color * ( clampResult32 - clampResult56 ) ) + ( _Color * float4( 0.745283,0.745283,0.745283,1 ) * clampResult56 ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
				
				half4 color = ( tex2D( _MainTex, uv_MainTex ) * _Opacity * clampResult55 );
				
				#ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17000
687;92;887;655;3797.29;-232.918;1.42675;True;False
Node;AmplifyShaderEditor.RangedFloatNode;68;-3247.626,721.5612;Float;False;Property;_Wavespower;Waves power;8;0;Create;True;0;0;False;0;50;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;70;-2330.165,203.2333;Float;False;Property;_Wavesdistance;Waves distance;7;0;Create;True;0;0;False;0;20;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;79;-2702.841,347.0948;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;69;-2353.327,364.5744;Float;False;Property;_Wavesspeed;Waves speed;6;0;Create;True;0;0;False;0;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-2784.766,961.9468;Float;False;Property;_Pivotpoint;Pivot point;9;0;Create;True;0;0;False;0;-0.5;-0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-2110.613,146.4072;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;20;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-3063.604,457.6144;Float;False;Property;_Fullnes;Fullnes;5;0;Create;True;0;0;False;0;0.35;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;26;-2081.457,368.6846;Float;False;1;0;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;67;-2965.456,582.4576;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;78;-2590.384,943.3563;Float;False;FLOAT2;4;0;FLOAT;-1;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;62;-2696.757,502.2593;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-70;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;59;-2074.876,451.2549;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;25;-1884.289,300.6898;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;63;-2692.389,655.3998;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;70;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;36;-2389.927,872.4304;Float;True;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;-1,-0.5;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LengthOpNode;37;-2090.193,902.4299;Float;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;27;-1733.606,348.7121;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;61;-1858.245,482.8112;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;38;-1894.998,932.9296;Float;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-4;False;4;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-1604.081,347.3522;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;43;-1327.135,833.5518;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;41;-1326.384,589.2441;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;22;-1723.752,164.3532;Float;False;Property;_MasktilingXY;Mask tiling XY;4;0;Create;True;0;0;False;0;0,-0.25,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;44;-1077.657,888.8188;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;32;-1084.148,586.3205;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;46;-926.0536,761.9135;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-1477.646,62.62439;Float;False;0;7;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;23;-1388.756,192.2407;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;20;-1195.134,129.97;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;56;-737.9833,702.134;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;57;-516.0291,496.9258;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;14;-880.5118,307.8891;Float;False;Property;_Color;Color 0;3;0;Create;True;0;0;False;0;1,0,0,1;1,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;-972.0174,115.8277;Float;True;Property;_Mask;Mask;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-174.1775,234.2768;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-153.7545,445.937;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0.745283,0.745283,0.745283,1;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;45;55.65034,326.9856;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;2;-968.5416,-78.5788;Float;True;Property;_MainTex;MainTex;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;55;255.0386,269.2798;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;8;60.71571,116.6346;Float;False;Property;_Opacity;Opacity;2;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;433.9773,54.74927;Float;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;4;737.0037,54.00163;Float;False;True;2;Float;ASEMaterialInspector;0;4;HOVL/Potion;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;True;2;False;-1;True;True;True;True;True;0;True;-9;True;False;0;False;-5;255;False;-8;255;False;-7;0;False;-4;0;False;-6;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;0;False;-1;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;24;0;79;1
WireConnection;24;1;70;0
WireConnection;26;0;69;0
WireConnection;67;0;68;0
WireConnection;78;1;76;0
WireConnection;62;0;51;0
WireConnection;62;3;67;0
WireConnection;59;0;79;2
WireConnection;25;0;24;0
WireConnection;25;1;26;0
WireConnection;63;0;51;0
WireConnection;63;4;68;0
WireConnection;36;0;79;0
WireConnection;36;3;78;0
WireConnection;37;0;36;0
WireConnection;27;0;25;0
WireConnection;61;0;59;0
WireConnection;61;3;62;0
WireConnection;61;4;63;0
WireConnection;38;0;37;0
WireConnection;28;0;27;0
WireConnection;28;1;61;0
WireConnection;43;0;28;0
WireConnection;43;1;38;0
WireConnection;41;0;28;0
WireConnection;41;1;38;0
WireConnection;44;0;43;0
WireConnection;32;0;41;0
WireConnection;46;0;32;0
WireConnection;46;1;44;0
WireConnection;23;0;22;1
WireConnection;23;1;22;2
WireConnection;20;0;21;0
WireConnection;20;2;23;0
WireConnection;56;0;46;0
WireConnection;57;0;32;0
WireConnection;57;1;56;0
WireConnection;7;1;20;0
WireConnection;48;0;7;0
WireConnection;48;1;14;0
WireConnection;48;2;57;0
WireConnection;49;0;14;0
WireConnection;49;2;56;0
WireConnection;45;0;48;0
WireConnection;45;1;49;0
WireConnection;55;0;45;0
WireConnection;18;0;2;0
WireConnection;18;1;8;0
WireConnection;18;2;55;0
WireConnection;4;0;18;0
ASEEND*/
//CHKSM=574639805A61E533A0C6ECC6FB1951E064ED10F4