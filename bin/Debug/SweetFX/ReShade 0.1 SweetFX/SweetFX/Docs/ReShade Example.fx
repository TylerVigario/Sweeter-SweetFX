/*
 * This is an example shader demonstrating ReShade's shading language and its features.
 */

/*
 * Preprocessor Macros:
 * --------------------
 * 
 * __RESHADE__ (version of the injector)
 * __VENDOR__ (vendor id)
 * __DEVICE__ (device id)
 * __RENDERER__ (Direct3D9: 0xD3D9 / Direct3D10: 0xD3D10 / Direct3D11: 0xD3D11 / OpenGL: 0x061)
 *
 * __DATE_YEAR__ (current year)
 * __DATE_MONTH__ (current month)
 * __DATE_DAY__ (current day in month)
 *
 * BUFFER_WIDTH (screen width)
 * BUFFER_HEIGHT (screen height)
 * BUFFER_RCP_WIDTH (reciprocal screen width)
 * BUFFER_RCP_HEIGHT (reciprocal screen height)
 *
 * Annotations:
 * ------------
 *
 * texture imageTex < source = "path/to/image.bmp"; > { ... };
 * uniform float frametime < source = "frametime"; >; // Time in milliseconds it took for the last frame to complete.
 * uniform int framecount < source = "framecount"; >; // Total amount of frames since the game started.
 * uniform float4 date < source = "date"; >; // float4(year, month (1 - 12), day of month (1 - 31), time in seconds)
 * uniform float timer < source = "timer"; >; // Timer counting time in milliseconds.
 * uniform float timeleft < source = "timeleft"; >; // Time in milliseconds that is left until the current technique timeout is reached.
 * uniform bool keydown < source = "key"; keycode = 0x20; >; // True if specified keycode (in this case the spacebar) is pressed and false otherwise.
 *
 * technique tech1 < enabled = true; > { ... } // Enable this technique by default.
 * technique tech2 < timeout = 1000; > { ... } // Auto-toggle this technique off 1000 milliseconds after it was enabled.
 * technique tech3 < toggle = 0x20; > { ... } // Toggle this technique when the specified keycode (in this case the spacebar) is pressed.
 * technique tech3 < toggleTime = 100; > { ... } // Toggle this technique at the specified time (in seconds after midnight).
 *
 */

// Textures declared with semantics can be used to request special images:
// This texture recieves the games frame image.
texture texColorBuffer : COLOR; // or SV_Target
// This textures recieves the games depth information.
texture texDepthBuffer : DEPTH; // or SV_Depth

// Textures declared are created at runtime with the parameters specified in the body.
// The default value is used if an option is missing here.
texture texTarget
{
	// The texture dimensions (default: 1x1).
	Width = BUFFER_WIDTH / 2;
	Height = BUFFER_HEIGHT / 2;
	
	// The amount of mipmaps (default: 1).
	MipLevels = 1;
	
	// The internal texture format (default: RGBA8).
	// Available formats: R8, R32F, RG8, RGBA8, RGBA16, RGBA16F, RGBA32F
	// Available compressed formats: DXT1 or BC1, DXT3 or BC2, DXT5 or BC3, LATC1 or BC4, LATC2 or BC5
	Format = RGBA8;
};

// This texture has a source annotation, which means ReShade will load the image from the specified path,
// stretch it to the texture dimensions and then load it into the texture for use in the shader.
texture texImage < source = "image.bmp"; >
{
	Width = 800;
	Height = 600;
};

// Samplers are the bridge between textures and shaders. They set what to happen when a texture is sampled.
// Multiple samplers can refer to the same texture using different options.
// Missing options are again set to their defaults.
sampler samplerColor
{
	// The texture to be used for sampling. This one has to be declared beforehand.
	Texture = texColorBuffer;
	
	// The method used for resolving texture coordinates which are outside of bounds (default: CLAMP).
	// Available values: CLAMP, MIRROR, REPEAT or WRAP, BORDER
	AddressU = CLAMP;
	AddressV = CLAMP;
	AddressW = CLAMP;
	
	// The minification, magnification and minification mipmap filter types (default: LINEAR).
	// Available values: POINT, LINEAR, ANISOTROPIC
	MinFilter = LINEAR;
	MagFilter = LINEAR;
	MipFilter = LINEAR;
	
	// An offset applied to the calculated mipmap level (default: 0). This needs to be a floating point literal.
	MipLODBias = 0.0f;
	
	// The maxmimum mipmap levels accessible (default: -1000 to 1000). These need to be floating point literals.
	MinLOD = -1000.0f;
	MaxLOD = 1000.0f;
	
	// The maximum anisotropy used when anisotropic filtering is enabled (default: 1).
	MaxAnisotropy = 1;
	
	// Enable or disable converting to linear colors when sampling from the texture.
	SRGBTexture = FALSE;
};
sampler samplerDepth
{
	Texture = texDepthBuffer;
};
sampler samplerTarget
{
	Texture = texTarget;
};
sampler samplerImage
{
	Texture = texImage;
};

// Structures are user defined data types that can be used in the shader code.
struct MyStruct
{
	uint member0;
	float4 member1, member2;
};

// Uniform variables can be used to get values from ReShade.
// You can declare single uniform variables or whole blocks by using a struct as type,
// which are set as a whole and thus usually faster when using multiple variables.
// Initializers are used for the initial value when providied.
uniform float4 UniformSingleValue = float4(0.0f, 0.0f, 0.0f, 0.0f);
// It is recommended to use constants instead of uniforms if the value is not changing.
static const float4 ConstantSingleValue = float4(0.0f, 0.0f, 0.0f, 0.0f);

// If a source annotation is added it can be used to request special values from ReShade.
// The following for instance gets a high precision timer to be used in functions below:
uniform float timer < source = "timer"; >;

// Functions can be declared as usual. Parameters can be qualified with "in" (default and implicit if none is used), "out" or "inout".
float4 DoNothing(float4 color)
{
	return color;
}

// Semantics are used to tell the runtime which arguments to connect between shader stages. They are ignored on
// non-entry-point functions (those not used in any pass below). Semantics starting with "SV_" are system value semantics and serve a special meaning.
// All semantics are case insensitive.
// The following vertex shader demonstrates how to generate a simple fullscreen triangle with the three vertices provided by ReShade:
void MainVS(in uint id : SV_VertexID, out float4 position : SV_Position, out float2 texcoord : TEXCOORD0)
{
	texcoord = float2((id << 1) & 2, id & 2);
	position = float4(texcoord * float2(2, -2) + float2(-1, 1), 0, 1);
}
// The following pixel shader simply returns the color of the games output again without modifying it:
void MainPS0(in float4 pos : SV_Position, in float2 texcoord : TEXCOORD0, out float4 color : SV_Target)
{
	/*
	 * Intrinsics:
	 * -----------
	 *
	 * abs, sign, rcp, all, any, sin, sinh, cos, cosh, tan, tanh, asin, acos, atan, atan2, sincos, exp, exp2, log, log2, log10, sqrt, rsqrt, ceil, floor, frac, trunc, round, radians, degrees, ddx, ddy, noise, length, normalize, transpose, determinant, asint, asuint, asfloat, asfloat, mul, mad, dot, cross, distance, pow, modf, frexp, ldexp, min, max, clamp, saturate, step, smoothstep, lerp, reflect, refract, faceforward
	 * tex2D, tex2Doffset, tex2Dlod, tex2Dlodoffset, tex2Dgather, tex2Dgatheroffset, tex2Dbias, tex2Dfetch, tex2Dsize
	 *
	 * Statements:
	 * -----------
	 *
	 * if (...) { ... } else { ... }
	 * switch (...) { case ...: ... break; ... default: ... break; }
	 * for (...; ...; ...) { ... }
	 * while (...) { ... }
	 * do { ... } while (...);
	 *
	 * continue;
	 * break;
	 * return ...;
	 * discard; // Use the "discard" keyword to abort rendering a pixel completly and step out of the function (in pixelshaders only).
	 */
	
	// Most shader intrinsics and some new ones are available (see list above). Here "tex2D" is used to sample from the frame color texture using the "samplerColor" sampler from above:
	color = tex2D(samplerColor, texcoord);
	
	// You can declare local variables and use them in the scope they were declared.
	bool myBool = false;
	int myInt = 0;
	uint myUint = 0;
	float myFloat = 0.0f;
	
	// You can call functions declared before.
	color = DoNothing(color);
}
// The following pixel shader takes the output of the previous pass and adds the depthbuffer content to the right screen side.
float4 MainPS1(in float4 pos : SV_Position, in float2 texcoord : TEXCOORD0) : SV_Target
{
	// Here color information is sampled with "samplerTarget" and thus from "texTarget",
	// which was set as rendertarget in the previous pass (see the technique definition below) and now contains its output.
	// In this case it is the game output, but downsampled to half because the texture is only half of the screen size.
	float4 color = tex2D(samplerTarget, texcoord);
	
	// Only execute the following code when on the right half of the screen.
	if (texcoord.x > 0.5f)
	{
		// Sample from the game depthbuffer using the "samplerDepth" sampler declared above, which samples from "texDepth" texture.
		float depth = tex2D(samplerDepth, texcoord).r;
		
		// Linearize the depth values to better visualize them.
		depth = 2.0 / (-99.0 * depth + 101.0);
		
		// Return that value.
		color.rgb = depth.rrr;
	}
	
	return color;
}

// An effect file can have multiple techniques, each representing a full render pipeline,
// which is executed to apply post-processing effects. A technique must have a name.
// ReShade executes all enabled techniques in the order they were defined in the effect file.
// The "enabled" annotation is used to set the default state, which later can be changed, for instance by pressing the key specified with "toggle".
technique Example < enabled = true; >
{
	// A technique is made up of one or more passes which contain information about which renderstates to set
	// and what shaders to execute. They are run sequentially starting with the top most declared. A name is optional.
	pass p0
	{
		// Each pass can set renderstates. The default value is used if one is not specified in the pass body.
		
		// The following two accept function names declared above which are used as entry points for the shader.
		// Please note that all parameters must have an associated semantic so the runtime can match them between shader stages.
		VertexShader = MainVS;
		PixelShader = MainPS0;
		
		// RenderTarget0 to RenderTarget7 allow you to set one or more rendertargets for rendering to textures.
		// Set them to a texture name declared above in order to write the color output (SV_Target0 to RenderTarget0, SV_Target1 to RenderTarget1, ...)
		// to this texture in this pass.
		// If multiple rendertargets are used, the dimensions of them has to match each other.
		// If no rendertargets are set here, RenderTarget0 points to the backbuffer.
		// Be aware that you can only read OR write a texture at the same time, so don't sample from it while it's still bound as render target here.
		// RenderTarget and RenderTarget0 are aliases.
		RenderTarget = texTarget;
		
		// The following renderstates set various other parameters for rendering.
		
		// The mask applied to the color output before it is written to the rendertarget.
		RenderTargetWriteMask = 0xF; // or ColorWriteEnable
		
		// Enable or disable applying gamma correction to the color output.
		SRGBWriteEnable = FALSE;
		
		// Enable or disable the depth and stencil tests.
		DepthEnable = FALSE; // or ZEnable
		StencilEnable = FALSE;
		
		// Enable or disable writing to the internal depthbuffer for depth testing.
		DepthWriteMask = FALSE; // or ZWriteEnable
		
		// The function used for depth testing.
		// Available values: NEVER, LESS, GREATER, LEQUAL or LESSEQUAL, GEQUAL or GREATEREQUAL, EQUAL, NEQUAL or NOTEQUAL, ALWAYS
		DepthFunc = ALWAYS; // or ZFunc
		
		// The masks applied before reading from/writing to the stencilbuffer.
		// Available values: 0-255
		StencilReadMask = 0xFF; // or StencilMask
		StencilWriteMask = 0xFF;
		
		// The reference value used with the stencil function.
		StencilRef = 0;
		
		// The function used for stencil testing.
		// Available values: NEVER, LESS, GREATER, LEQUAL or LESSEQUAL, GEQUAL or GREATEREQUAL, EQUAL, NEQUAL or NOTEQUAL, ALWAYS
		StencilFunc = ALWAYS;
		
		// The operation to perform on the stencil buffer when the stencil test passed/failed/stencil passed but depth test failed.
		// Available values: KEEP, ZERO, REPLACE, INCR, INCRSAT, DECR, DECRSAT, INVERT
		StencilPassOp = KEEP; // or StencilPass
		StencilFailOp = KEEP; // or StencilFail
		StencilDepthFailOp = KEEP; // or StencilZFail
	}
	pass p1
	{
		VertexShader = MainVS;
		PixelShader = MainPS1;
	}
}