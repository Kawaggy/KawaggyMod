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

float4 DeathWhisper(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float luminosity = (color.r + color.g + color.b) / 3;
    color.gb = 0;
    color.r += -frac((uTime / 2)) * luminosity;
    color.r += -frac(uTime) * luminosity * 0.5;
    
    return color * sampleColor;
}

technique Technique1
{
    pass DeathWhisperPass
    {
        PixelShader = compile ps_2_0 DeathWhisper();
    }
}
