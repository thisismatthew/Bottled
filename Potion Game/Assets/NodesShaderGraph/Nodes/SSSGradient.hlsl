void SSSGradientFunction_float(float3 Normal, float Depth, Gradient Gradient, float Transmission, float Intensity, float3 lightDirection, out float3 Out)
            {

             float remap1 = ((Depth)*4.0+-3.0);

            float nulll = 0.0;

            float M_ = (-1.0);

            float _Gradient = saturate((nulll + ( ((dot(Normal,lightDirection)*(1.0 - remap1)) - M_) * (1.0 - nulll) ) / (2.0 - M_)));

            float Gr = (1.0 - (_Gradient*0.9+0.1));

            float3 _ColorSSS_ = 0;



                float3 color = Gradient.colors[0].rgb;
                [unroll]
                for (int c = 1; c < 8; c++) 
                {
                    float colorPos = saturate((1 - _Gradient - Gradient.colors[c-1].w) / (Gradient.colors[c].w - Gradient.colors[c-1].w)) * step(c, Gradient.colorsLength-1);
                    color = lerp(color, Gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), Gradient.type));
                }

            //#ifndef UNITY_COLORSPACE_GAMMA
                color = SRGBToLinear(color);
            //#endif

                float alpha = Gradient.alphas[0].x;
                [unroll]
                for (int a = 1; a < 8; a++) 
                {
                    float alphaPos = saturate((1 - _Gradient - Gradient.alphas[a-1].y) / (Gradient.alphas[a].y - Gradient.alphas[a-1].y)) * step(a, Gradient.alphasLength-1);
                    alpha = lerp(alpha, Gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), Gradient.type));
                }
                _ColorSSS_ = float4(color, alpha);



            float3 FinalResult = (lerp((_ColorSSS_*saturate(lerp(_Gradient,(_Gradient*Gr),(_Gradient*(Transmission+0.3))))),_ColorSSS_,Transmission)*(Intensity*3.0));





            Out = FinalResult;
            }