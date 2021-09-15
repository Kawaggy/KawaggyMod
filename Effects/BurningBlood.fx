sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;

float4 BurningBlood(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float luminosity = (color.r + color.g + color.b) / 3;
    
    color.rgb = (1 - luminosity) * color.a;
    color.gb = 0;
    
    float divideBy = 2;
    color.r += -frac(uTime / divideBy) * color.a;
    color.r += -frac(uTime / (divideBy / 2)) * color.a * 0.75;
    return color * sampleColor;
}

technique Technique1
{
    pass BurningBloodPass
    {
        PixelShader = compile ps_2_0 BurningBlood();
    }
}