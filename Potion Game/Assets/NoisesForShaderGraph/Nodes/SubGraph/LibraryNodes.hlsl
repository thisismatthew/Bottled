

float3 mod289(float3 x) 
{
    return x - floor(x * (1.0 / 289.0)) * 289.0;
}
float2 mod289(float2 x)
{
    return x - floor(x * (1.0 / 289.0)) * 289.0;
}
float3 permute(float3 x)
{
    return mod289(((x * 34.0) + 1.0) * x);
}

float4 mod289(float4 x)
{
    return x - floor(x / 289.0) * 289.0;
}

float4 _permute(float4 x)
{
    return mod289((x * 34.0 + 1.0) * x);
}


float4 permute(float4 x)
{
    return fmod((34.0 * x + 1.0) * x, 289.0);
}


float2 cellular2x2(float2 P)
{
#define K 0.142857142857
#define K2 0.0714285714285
#define jitter 0.8
    float2 Pi = fmod(floor(P), 289.0);
    float2 Pf = frac(P);
    float4 Pfx = Pf.x + float4(-0.5, -1.5, -0.5, -1.5);
    float4 Pfy = Pf.y + float4(-0.5, -0.5, -1.5, -1.5);
    float4 p = permute(Pi.x + float4(0.0, 1.0, 0.0, 1.0));
    p = permute(p + Pi.y + float4(0.0, 0.0, 1.0, 1.0));
    float4 ox = fmod(p, 7.0) * K + K2;
    float4 oy = fmod(floor(p * K), 7.0) * K + K2;
    float4 dx = Pfx + jitter * ox;
    float4 dy = Pfy + jitter * oy;
    float4 d = dx * dx + dy * dy; 
#if 0
	// Cheat and pick only F1
	d.xy = min(d.xy, d.zw);
	d.x = min(d.x, d.y);
	return d.xx; // F1 duplicated, F2 not computed
#else

    d.xy = (d.x < d.y) ? d.xy : d.yx; 
    d.xz = (d.x < d.z) ? d.xz : d.zx;
    d.xw = (d.x < d.w) ? d.xw : d.wx;
    d.y = min(d.y, d.z);
    d.y = min(d.y, d.w);
    return sqrt(d.xy);
#endif
}
float randomN(in float2 st)
{
    return frac(sin(dot(st.xy,
						 float2(12.9498, 78.233)))
				* 43758.5363123);
}

float noiseW(float2 st)
{
    float2 i = floor(st);
    float2 f = frac(st);
    float2 u = f * f * (3.0 - 2.0 * f);
    return lerp(lerp(randomN(i + float2(0.0, 0.0)),
					 randomN(i + float2(1.0, 0.0)), u.x),
				lerp(randomN(i + float2(0.0, 1.0)),
					 randomN(i + float2(1.0, 1.0)), u.x), u.y);
}



float2 rotate2d(float angle)
{
    return float4(cos(angle), -sin(angle), sin(angle), cos(angle));
}

float lines(in float2 pos, float b, float scaleLine)
{
    float scale = scaleLine;
    pos *= scale;
    return smoothstep(0.0,
					.5 + b * .5,
					abs((sin(pos.x * 3.1415) + b * 2.0)) * .5);
}


float random(float2 st)
{
    float r = frac(sin(dot(st.xy,
					float2(12.9898, 78.233)))
					* 43758.5453123);
    return r * clamp(pow(distance(r, 0.6), 2.5) * 100, 0, 1);
}


void Wood_float(float2 UV, float scaleLine, float DepthLine, float deformLine, out float Out)
{
    float2 st = UV.xy;
	

    float2 pos = st.yx * float2(10, 3) * (scaleLine / 3);

    float pattern = pos.x;

    pos = rotate2d(noiseW(pos) * deformLine) * pos;

    pattern = lines(pos, DepthLine, scaleLine);

    Out = pattern;
}



void AnisotropicNoise_float(float2 UV, float Scale, float Width, float Smoothness, out float Out)
{

    float2 UV_ = UV * float2(1, Width * Scale);
				
    float2 t;

    t = random(floor((UV_)));
    float2 i;
    float a;
    float OfssetOct = 1;
    float Octaves = 20;
				
    for (int iii = 0; iii < Octaves; iii++)
    {

        i = (floor((UV_ + lerp(0, float2(1.0 / Scale, 0.0), Smoothness * OfssetOct)) * Scale + (float2(t.y * 5, 0) * 100)) * float2(199999, 1));


        OfssetOct = OfssetOct - (1 / Octaves);

        a = a + random(i + t.y);
					
    }


    a = a / Octaves;

    Out = a;
}


void cellular_float(float2 P, float Scale, out float2 Out)
{
#define K 0.142857142857 
#define Ko 0.428571428571 
#define jitter 1.0 
    P = P * Scale;
    float2 Pi = fmod(floor(P), 288.1);
    float2 Pf = frac(P);
    float3 oi = float3(-1.0, 0.0, 1.0);
    float3 of = float3(-0.5, 0.5, 1.5);
    float3 px = permute(oi + Pi.x);
    float3 p = permute(px.x + Pi.y + oi);
    float3 ox = frac(p * K) - Ko;
    float3 oy = fmod(floor(p * K), 7.0) * K - Ko;
    float3 dx = Pf.x + 0.5 + jitter * ox;
    float3 dy = Pf.y - of + jitter * oy;
    float3 d1 = dx * dx + dy * dy; 
    p = permute(px.y + Pi.y + oi); 
    ox = frac(p * K) - Ko;
    oy = fmod(floor(p * K), 7.0) * K - Ko;
    dx = Pf.x - 0.5 + jitter * ox;
    dy = Pf.y - of + jitter * oy;
    float3 d2 = dx * dx + dy * dy; 
    p = permute(px.z + Pi.y + oi); 
    ox = frac(p * K) - Ko;
    oy = fmod(floor(p * K), 7.0) * K - Ko;
    dx = Pf.x - 1.5 + jitter * ox;
    dy = Pf.y - of + jitter * oy;
    float3 d3 = dx * dx + dy * dy; 

    float3 d1a = min(d1, d2);
    d2 = max(d1, d2); 
    d2 = min(d2, d3); 
    d1 = min(d1a, d2); 
    d2 = max(d1a, d2); 
    d1.xy = (d1.x < d1.y) ? d1.xy : d1.yx; 
    d1.xz = (d1.x < d1.z) ? d1.xz : d1.zx; 
    d1.yz = min(d1.yz, d2.yz); 
    d1.y = min(d1.y, d1.z); 
    d1.y = min(d1.y, d2.x); 
    Out = pow(0.1 + (sqrt(d1.xy) * 0.6),1.5);
}



void GradientAngle_float(float Angle, float2 UV, float2 center, out float angle)
{
     angle = atan2(UV.y - center.y, UV.x - center.x) + (radians(Angle));
    float rad = 6.283185307164;
    angle = (angle + rad) % rad / rad;
 

} 


float4 _taylorInvSqrt(float4 r)
{
    return 1.79284291400159 - 0.85373472095314 * r;
}


float _mod289(float x)
{
    return x - floor(x * (1.0 / 289.0)) * 289.0;
}
float4 _mod289(float4 x)
{
    return x - floor(x * (1.0 / 289.0)) * 289.0;
}
float3 _mod289(float3 x)
{
    return x - floor(x * (1.0 / 289.0)) * 289.0;
}
float3 _perm(float3 x)
{
    return _mod289(((x * 34.0) + 1.0) * x);
}
float4 _perm(float4 x)
{
    return _mod289(((x * 34.0) + 1.0) * x);
}


float dm_SimplexNoise(float3 v)
{
    const float2 C = float2(1.0 / 6.0, 1.0 / 3.0);
    const float4 D = float4(0.0, 0.5, 1.0, 2.0);
    float3 i = floor(v + dot(v, C.yyy));
    float3 x0 = v - i + dot(i, C.xxx);
    float3 g = step(x0.yzx, x0.xyz);
    float3 l = 1.0 - g;
    float3 i1 = min(g.xyz, l.zxy);
    float3 i2 = max(g.xyz, l.zxy);
    float3 x1 = x0 - i1 + 1.0 * C.xxx;
    float3 x2 = x0 - i2 + 2.0 * C.xxx;
    float3 x3 = x0 - 1. + 3.0 * C.xxx;
    i = _mod289(i);
    float4 p = _perm(_perm(_perm(
                i.z + float4(0.0, i1.z, i2.z, 1.0))
            + i.y + float4(0.0, i1.y, i2.y, 1.0))
            + i.x + float4(0.0, i1.x, i2.x, 1.0));

    float n_ = 1.0 / 7.0; 
    float3 ns = n_ * D.wyz - D.xzx;
    float4 j = p - 49.0 * floor(p * ns.z * ns.z); 
    float4 x_ = floor(j * ns.z);
    float4 y_ = floor(j - 7.0 * x_);
    float4 x = x_ * ns.x + ns.yyyy;
    float4 y = y_ * ns.x + ns.yyyy;
    float4 h = 1.0 - abs(x) - abs(y);
    float4 b0 = float4(x.xy, y.xy);
    float4 b1 = float4(x.zw, y.zw);
    float4 s0 = floor(b0) * 2.0 + 1.0;
    float4 s1 = floor(b1) * 2.0 + 1.0;
    float4 sh = -step(h, 0.0);
    float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
    float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
    float3 p0 = float3(a0.xy, h.x);
    float3 p1 = float3(a0.zw, h.y);
    float3 p2 = float3(a1.xy, h.z);
    float3 p3 = float3(a1.zw, h.w);
    float4 norm = _taylorInvSqrt(float4(dot(p0, p0), dot(p1, p1), dot(p2, p2), dot(p3, p3)));
    p0 *= norm.x;
    p1 *= norm.y;
    p2 *= norm.z;
    p3 *= norm.w;

    float4 m = max(0.6 - float4(dot(x0, x0), dot(x1, x1), dot(x2, x2), dot(x3, x3)), 0.0);
    m = m * m;
    return 42.0 * dot(m * m, float4(dot(p0, x0), dot(p1, x1), dot(p2, x2), dot(p3, x3)));
}




float snoise(float2 v)
{

    const float4 C = float4(0.211324865405187,

                        0.366025403784439,

                        -0.577350269189626,

                        0.024390243902439);


    float2 i = floor(v + dot(v, C.yy));
    float2 x0 = v - i + dot(i, C.xx);


    float2 i1 = float2(0.0,0);
    i1 = (x0.x > x0.y) ? float2(1.0, 0.0) : float2(0.0, 1.0);
    float2 x1 = x0.xy + C.xx - i1;
    float2 x2 = x0.xy + C.zz;

    i = mod289(i);
    float3 p = permute(
            permute(i.y + float3(0.0, i1.y, 1.0))
                + i.x + float3(0.0, i1.x, 1.0));

    float3 m = max(0.5 - float3(
                        dot(x0, x0),
                        dot(x1, x1),
                        dot(x2, x2)
                        ), 0.0);

    m = m * m;
    m = m * m;


    float3 x = 2.0 * frac(p * C.www) - 1.0;
    float3 h = abs(x) - 0.5;
    float3 ox = floor(x + 0.5);
    float3 a0 = x - ox;

    m *= 1.79284291400159 - 0.85373472095314 * (a0 * a0 + h * h);

    float3 g = float3(0,0,0);
    g.x = a0.x * x0.x + h.x * x0.y;
    g.yz = a0.yz * float2(x1.x, x2.x) + h.yz * float2(x1.y, x2.y);
    return 130.0 * dot(m, g);
}



void Billow3DNoise_float(float3 Pos, float Frequency, float Gain, int Octaves, float3 Offset, float Lacunarity, float Bias, float Persistence,out float Out)
{
    float F = Frequency;
    float A = Gain;
    float N = 0;
    for (int i = 1; i < Octaves; i++)
    {
        float3 pos = Offset + Pos;
        float s = dm_SimplexNoise(pos*F);
        s *= A;
        s = abs(s);
        N += (s * Lacunarity) + Bias;
        F *= Lacunarity;
        A *= Persistence;
    }
    Out = (N + 1) * 0.5;
}


void Billow2DNoise_float(float2 UV, float Frequency, float Gain, int Octaves, float3 Offset, float Lacunarity, float Bias, float Persistence, out float Out)
{
    float F = Frequency;
    float A = Gain;
    float N = 0;
    for (int i = 1; i < Octaves; i++)
    {
        float2 pos = Offset + UV;
        float s = snoise(pos * F);
        s *= A;
        s = abs(s);
        N += (s * Lacunarity) + Bias;
        F *= Lacunarity;
        A *= Persistence;
    }
    Out = (N + 1) * 0.5;
}
 

float2 d3D_Cellular(float3 P)
{
#define K 0.142857142857 
#define Ko 0.428571428571 
#define K2 0.020408163265306 
#define Kz 0.166666666667 
#define Kzo 0.416666666667 
#define jitter 1.0
    float3 Pi = _mod289(floor(P));
    float3 Pf = frac(P) - 0.5;
    float3 Pfx = Pf.x + float3(1.0, 0.0, -1.0);
    float3 Pfy = Pf.y + float3(1.0, 0.0, -1.0);
    float3 Pfz = Pf.z + float3(1.0, 0.0, -1.0);
    float3 p = _perm(Pi.x + float3(-1.0, 0.0, 1.0));
    float3 p1 = _perm(p + Pi.y - 1.0);
    float3 p2 = _perm(p + Pi.y);
    float3 p3 = _perm(p + Pi.y + 1.0);
    float3 p11 = _perm(p1 + Pi.z - 1.0);
    float3 p12 = _perm(p1 + Pi.z);
    float3 p13 = _perm(p1 + Pi.z + 1.0);
    float3 p21 = _perm(p2 + Pi.z - 1.0);
    float3 p22 = _perm(p2 + Pi.z);
    float3 p23 = _perm(p2 + Pi.z + 1.0);
    float3 p31 = _perm(p3 + Pi.z - 1.0);
    float3 p32 = _perm(p3 + Pi.z);
    float3 p33 = _perm(p3 + Pi.z + 1.0);
    float3 ox11 = frac(p11 * K) - Ko;
    float3 oy11 = fmod(floor(p11 * K), 7.0) * K - Ko;
    float3 oz11 = floor(p11 * K2) * Kz - Kzo; 
    float3 ox12 = frac(p12 * K) - Ko;
    float3 oy12 = fmod(floor(p12 * K), 7.0) * K - Ko;
    float3 oz12 = floor(p12 * K2) * Kz - Kzo;
    float3 ox13 = frac(p13 * K) - Ko;
    float3 oy13 = fmod(floor(p13 * K), 7.0) * K - Ko;
    float3 oz13 = floor(p13 * K2) * Kz - Kzo;
    float3 ox21 = frac(p21 * K) - Ko;
    float3 oy21 = fmod(floor(p21 * K), 7.0) * K - Ko;
    float3 oz21 = floor(p21 * K2) * Kz - Kzo;
    float3 ox22 = frac(p22 * K) - Ko;
    float3 oy22 = fmod(floor(p22 * K), 7.0) * K - Ko;
    float3 oz22 = floor(p22 * K2) * Kz - Kzo;
    float3 ox23 = frac(p23 * K) - Ko;
    float3 oy23 = fmod(floor(p23 * K), 7.0) * K - Ko;
    float3 oz23 = floor(p23 * K2) * Kz - Kzo;
    float3 ox31 = frac(p31 * K) - Ko;
    float3 oy31 = fmod(floor(p31 * K), 7.0) * K - Ko;
    float3 oz31 = floor(p31 * K2) * Kz - Kzo;
    float3 ox32 = frac(p32 * K) - Ko;
    float3 oy32 = fmod(floor(p32 * K), 7.0) * K - Ko;
    float3 oz32 = floor(p32 * K2) * Kz - Kzo;
    float3 ox33 = frac(p33 * K) - Ko;
    float3 oy33 = fmod(floor(p33 * K), 7.0) * K - Ko;
    float3 oz33 = floor(p33 * K2) * Kz - Kzo;
    float3 dx11 = Pfx + jitter * ox11;
    float3 dy11 = Pfy.x + jitter * oy11;
    float3 dz11 = Pfz.x + jitter * oz11;
    float3 dx12 = Pfx + jitter * ox12;
    float3 dy12 = Pfy.x + jitter * oy12;
    float3 dz12 = Pfz.y + jitter * oz12;
    float3 dx13 = Pfx + jitter * ox13;
    float3 dy13 = Pfy.x + jitter * oy13;
    float3 dz13 = Pfz.z + jitter * oz13;
    float3 dx21 = Pfx + jitter * ox21;
    float3 dy21 = Pfy.y + jitter * oy21;
    float3 dz21 = Pfz.x + jitter * oz21;
    float3 dx22 = Pfx + jitter * ox22;
    float3 dy22 = Pfy.y + jitter * oy22;
    float3 dz22 = Pfz.y + jitter * oz22;
    float3 dx23 = Pfx + jitter * ox23;
    float3 dy23 = Pfy.y + jitter * oy23;
    float3 dz23 = Pfz.z + jitter * oz23;
    float3 dx31 = Pfx + jitter * ox31;
    float3 dy31 = Pfy.z + jitter * oy31;
    float3 dz31 = Pfz.x + jitter * oz31;
    float3 dx32 = Pfx + jitter * ox32;
    float3 dy32 = Pfy.z + jitter * oy32;
    float3 dz32 = Pfz.y + jitter * oz32;
    float3 dx33 = Pfx + jitter * ox33;
    float3 dy33 = Pfy.z + jitter * oy33;
    float3 dz33 = Pfz.z + jitter * oz33;
    float3 d11 = dx11 * dx11 + dy11 * dy11 + dz11 * dz11;
    float3 d12 = dx12 * dx12 + dy12 * dy12 + dz12 * dz12;
    float3 d13 = dx13 * dx13 + dy13 * dy13 + dz13 * dz13;
    float3 d21 = dx21 * dx21 + dy21 * dy21 + dz21 * dz21;
    float3 d22 = dx22 * dx22 + dy22 * dy22 + dz22 * dz22;
    float3 d23 = dx23 * dx23 + dy23 * dy23 + dz23 * dz23;
    float3 d31 = dx31 * dx31 + dy31 * dy31 + dz31 * dz31;
    float3 d32 = dx32 * dx32 + dy32 * dy32 + dz32 * dz32;
    float3 d33 = dx33 * dx33 + dy33 * dy33 + dz33 * dz33;

#if 0
	// Cheat and sort out only F1
	float3 d1 = min(min(d11,d12), d13);
	float3 d2 = min(min(d21,d22), d23);
	float3 d3 = min(min(d31,d32), d33);
	float3 d = min(min(d1,d2), d3);
	d.x = min(min(d.x,d.y),d.z);
	return sqrt(d.xx); // F1 duplicated, no F2 computed
#else

    float3 d1a = min(d11, d12);
    d12 = max(d11, d12);
    d11 = min(d1a, d13); 
    d13 = max(d1a, d13);
    d12 = min(d12, d13); 
    float3 d2a = min(d21, d22);
    d22 = max(d21, d22);
    d21 = min(d2a, d23); 
    d23 = max(d2a, d23);
    d22 = min(d22, d23); 
    float3 d3a = min(d31, d32);
    d32 = max(d31, d32);
    d31 = min(d3a, d33); 
    d33 = max(d3a, d33);
    d32 = min(d32, d33); 
    float3 da = min(d11, d21);
    d21 = max(d11, d21);
    d11 = min(da, d31);
    d31 = max(da, d31); 
    d11.xy = (d11.x < d11.y) ? d11.xy : d11.yx;
    d11.xz = (d11.x < d11.z) ? d11.xz : d11.zx;
    d12 = min(d12, d21); 
    d12 = min(d12, d22); 
    d12 = min(d12, d31);
    d12 = min(d12, d32); 
    d11.yz = min(d11.yz, d12.xy); 
    d11.y = min(d11.y, d12.z); 
    d11.y = min(d11.y, d11.z);
    return sqrt(d11.xy);
#endif
}


void CellularNoise_float(float3 Pos, float3 Offset, float Frequency, out float Out)
{
float2 N = 0;
float3 pos = Offset + Pos;
    N = d3D_Cellular(pos*Frequency);
    Out = (N.y + N.x) * 0.5;
}



inline float2 Voronoi_Random_float(float2 UV, float offset)
{
    float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
    UV = frac(sin(mul(UV, m)) * 46839.32);
    return float2(sin(UV.y * +offset) * 0.5 + 0.5, cos(UV.x * offset) * 0.5 + 0.5);
}

void Voronoi_float(float2 UV, float AngleOffset, float CellDensity, out float Out)
{
    CellDensity = CellDensity;
    float2 g = floor(UV * CellDensity);
    float2 f = frac(UV * CellDensity);
    float t = 8.0;
    float3 res = float3(8.0, 0.0, 0.0);

    for (int y = -1; y <= 1; y++)
    {
        for (int x = -1; x <= 1; x++)
        {
            float2 lattice = float2(x, y);
            float2 offset = Voronoi_Random_float(lattice + g, AngleOffset);
            float d = distance(lattice + offset, f);
						

            if (d < res.x)
            {

                res = float3(d, offset.x, offset.y);
                Out = pow(res.x, 2);
            }
						

        }
    }
}

void CrackNoise_float(float2 UV, float sharpness, float width, float Scale, out float Out)
{
    

float _Voronoi_2952CCB4_Out_0;
float _Voronoi_2952CCB4_Out_1;
float _Voronoi_2952CCB4_Out_2;
float _Voronoi_2952CCB4_Out_3;

float _Voronoi_2952CCB4_Cells_4;



    Voronoi_float((UV * Scale), 2, 5, _Voronoi_2952CCB4_Out_0);
    Voronoi_float((UV * Scale) + float2((width / 100) * -1, 0), 2, 5, _Voronoi_2952CCB4_Out_1);
    Voronoi_float((UV * Scale) + float2((width / 100), (width / 100) * -1), 2, 5, _Voronoi_2952CCB4_Out_2);
    Voronoi_float((UV * Scale) + float2(0, (width / 100)), 2, 5, _Voronoi_2952CCB4_Out_3);

float _Blur = (_Voronoi_2952CCB4_Out_0 + _Voronoi_2952CCB4_Out_1 + _Voronoi_2952CCB4_Out_2 + _Voronoi_2952CCB4_Out_3) / 4;

    float crack = 1 - _Blur * Scale;
    float Fin = 1 - pow(lerp(_Blur, _Voronoi_2952CCB4_Out_0, sharpness), 3);
    Out = Fin;

}


void CrackNoise2_float(float2 UV, float sharpness,float sharpness2, float width, float BlurMask1, float BlurMask2, float BlurMask3, float blurr,float Scale, out float Out)
{
    
    BlurMask1 = lerp(1,BlurMask1,blurr);
    BlurMask2 = lerp(1, BlurMask2, blurr);
    BlurMask3 = lerp(1, BlurMask3, blurr);
    
    
    float _Voronoi_2952CCB4_Out_0;
    float _Voronoi_2952CCB4_Out_1;
    float _Voronoi_2952CCB4_Out_2;
    float _Voronoi_2952CCB4_Out_3;

    float _Voronoi_2952CCB4_Cells_4;



    Voronoi_float((UV * Scale), 2, 5, _Voronoi_2952CCB4_Out_0);
    Voronoi_float((UV * Scale * BlurMask1) + float2((width / 100) * -1, 0), 2, 5, _Voronoi_2952CCB4_Out_1);
    Voronoi_float((UV * Scale * BlurMask2) + float2((width / 100), (width / 100) * -1), 2, 5, _Voronoi_2952CCB4_Out_2);
    Voronoi_float((UV * Scale * BlurMask3) + float2(0, (width / 100)), 2, 5, _Voronoi_2952CCB4_Out_3);

    float _Blur = (_Voronoi_2952CCB4_Out_0 + _Voronoi_2952CCB4_Out_1 + _Voronoi_2952CCB4_Out_2 + _Voronoi_2952CCB4_Out_3) / 4;

    float crack = 1 - _Blur * Scale;
    float Fin = 1 - pow(lerp(_Blur, _Voronoi_2952CCB4_Out_0, sharpness), 3);
    
    Out = clamp(Fin,0,1);

}


void Squama_Voronoi_float(float2 UV, float AngleOffset, float CellDensity, float DepthSquama, out float Out, out float Cells)
{
    float2 g = floor(UV * CellDensity);
    float2 f = frac(UV * CellDensity);
    float t = 8.0;
    float3 res = float3(8.0, 0.0, 0.0);

    for (int y = -1; y <= 1; y++)
    {
        for (int x = -1; x <= 1; x++)
        {
            float2 lattice = float2(x, y);
            float2 offset = Voronoi_Random_float(lattice + g, AngleOffset);
            float d = distance(lattice + offset, f);



            if (d < res.x)
            {
                d = pow(d, DepthSquama);
                res = float3(d, offset.x, offset.y);
                Out = res.x;
                Cells = res.y;
            }
						

        }
    }
}

void simplexNoise3D_float(float3 v,out float Out)
{
    const float2 C = float2(1.0 / 6.0, 1.0 / 3.0);
    float3 i = floor(v + dot(v, C.yyy));
    float3 x0 = v - i + dot(i, C.xxx);
    float3 g = step(x0.yzx, x0.xyz);
    float3 l = 1.0 - g;
    float3 i1 = min(g.xyz, l.zxy);
    float3 i2 = max(g.xyz, l.zxy);
    float3 x1 = x0 - i1 + C.xxx;
    float3 x2 = x0 - i2 + C.yyy;
    float3 x3 = x0 - 0.5;
    i = mod289(i);
    float4 p =
	  _permute(_permute(_permute(i.z + float4(0.0, i1.z, i2.z, 1.0))
										+ i.y + float4(0.0, i1.y, i2.y, 1.0))
										+ i.x + float4(0.0, i1.x, i2.x, 1.0));
    float4 j = p - 49.0 * floor(p / 49.0); 
    float4 x_ = floor(j / 7.0);
    float4 y_ = floor(j - 7.0 * x_); 
    float4 x = (x_ * 2.0 + 0.5) / 7.0 - 1.0;
    float4 y = (y_ * 2.0 + 0.5) / 7.0 - 1.0;
    float4 h = 1.0 - abs(x) - abs(y);
    float4 b0 = float4(x.xy, y.xy);
    float4 b1 = float4(x.zw, y.zw);

    float4 s0 = floor(b0) * 2.0 + 1.0;
    float4 s1 = floor(b1) * 2.0 + 1.0;
    float4 sh = -step(h, 0.0);
    float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
    float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
    float3 g0 = float3(a0.xy, h.x);
    float3 g1 = float3(a0.zw, h.y);
    float3 g2 = float3(a1.xy, h.z);
    float3 g3 = float3(a1.zw, h.w);
    float4 norm = 1.7928429124568 - (float4(dot(g0, g0), dot(g1, g1), dot(g2, g2), dot(g3, g3))) * 0.95312124162;
    g0 *= norm.x;
    g1 *= norm.y;
    g2 *= norm.z;
    g3 *= norm.w;
    float4 m = max(0.6 - float4(dot(x0, x0), dot(x1, x1), dot(x2, x2), dot(x3, x3)), 0.0);
    m = m * m;
    m = m * m;
    float4 px = float4(dot(x0, g0), dot(x1, g1), dot(x2, g2), dot(x3, g3));
    Out = 42.0 * dot(m, px);
}

void LineRandom_float(float2 UV,float Scale,out float Out)
{
    
    Out = random(floor((UV + float2(UV.y, UV.x)) * Scale));
}
void HexagonNode_float(float2 UV, float Scale, out float Hexagon, out float2 HexPos, out float2 HexUV, out float2 HexIndex)
{
    float2 p = UV * Scale;
    p.x *= 1.11;
    float isTwo = frac(floor(p.x) / 2.0) * 2.0;
    float isOne = 1.0 - isTwo;
    p.y += isTwo * 0.5;
    float2 rectUV = frac(p);
    float2 grid = floor(p);
    p = frac(p) - 0.5;
    float2 s = sign(p);
    p = abs(p);
    Hexagon = abs(max(p.x * 1.5 + p.y, p.y * 2.0) - 1.0);

    float isInHex = step(p.x * 1.5 + p.y, 1.0);
    float isOutHex = 1.0 - isInHex;
    float2 grid2 = float2(0, 0);

    grid2 = lerp(
		float2(s.x, +step(0.0, s.y)),
		float2(s.x, -step(s.y, 0.0)),
		isTwo) * isOutHex;
    HexIndex = (grid + grid2);
    HexPos = HexIndex / 1.5;
    
    HexUV = lerp(rectUV, rectUV - s * float2(1.0, 0.5), isOutHex);
}

void Fractal_float(float2 UV,float2 Center, float Scale,float Iterations,out float Out)
{
    float2 F = Center + (UV - .5) * pow(Scale,5);
    float2 z;
    float IT;
    for (IT = 0; IT < Iterations; IT++)
    {
        z = float2(z.x * z.x - z.y * z.y, 2 * z.x * z.y) + F;
        if (length(z) > 2)
            break;

    }
    Out = IT / Iterations;

}



inline float SimpleNoise_RandomValue_float(float2 uv)
{
    return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
}

inline float SimpleNnoise_Interpolate_float(float a, float b, float t)
{
    return (1.0 - t) * a + (t * b);
}


inline float SimpleNoise_ValueNoise_float(float2 uv)
{
    float2 i = floor(uv);
    float2 f = frac(uv);
    f = f * f * (3.0 - 2.0 * f);

    uv = abs(frac(uv) - 0.5);
    float2 c0 = i + float2(0.0, 0.0);
    float2 c1 = i + float2(1.0, 0.0);
    float2 c2 = i + float2(0.0, 1.0);
    float2 c3 = i + float2(1.0, 1.0);
    float r0 = SimpleNoise_RandomValue_float(c0);
    float r1 = SimpleNoise_RandomValue_float(c1);
    float r2 = SimpleNoise_RandomValue_float(c2);
    float r3 = SimpleNoise_RandomValue_float(c3);

    float bottomOfGrid = SimpleNnoise_Interpolate_float(r0, r1, f.x);
    float topOfGrid = SimpleNnoise_Interpolate_float(r2, r3, f.x);
    float t = SimpleNnoise_Interpolate_float(bottomOfGrid, topOfGrid, f.y);
    return t;
}
void SimpleNoiseMultiOctaves_float(float2 UV, float Scale,float Octaves, out float Out)
{
    float t = 0.0;
    float freq;
    float amp;
    for (int i = 0; i < Octaves;i++)
    {
    
        freq = pow(2.0, float(i));
        amp = pow(0.5, float(Octaves - i));
        t += SimpleNoise_ValueNoise_float(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;


    }
    Out = t;
}