Shader "PostEffect/Holo"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _LineColor("LineColor", Color) = (0,0,0,0)
        _LineSpeed("LineSpeed",Range(0,10)) = 5
        _LineSize("LineSize",Range(0,1)) = 0.01
        _ColorGap("ColorGap",Range(0,1.0)) = 0.01
        _Alpha("Alpha", Range(0,1)) = 0.5
        _FrameRate("FrameRate", Range(0,30)) = 15
        _Frequency("Frequency", Range(0,1)) = 0.1
        _GlitchScale("GlitchScale", Range(1,10)) = 1
    }
        SubShader
        {
            Tags
            {
                "RenderType" = "Tranparent"
            }
            Blend SrcAlpha OneMinusSrcAlpha

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    float2 line_uv : TEXCOORD1;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float2 line_uv : TEXCOORD1;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _LineColor;
                float _LineSpeed;
                float _LineSize;
                float _ColorGap;
                float _Alpha;
                float _FrameRate;
                float _Frequency;
                float _GlitchScale;

                //�����_���Ȓl��Ԃ�
                float rand(float2 co) //�����̓V�[�h�l�ƌĂ΂��@�����l��n���Γ������̂�Ԃ�
                {
                    return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
                }

                //�p�[�����m�C�Y
                float perlinNoise(fixed2 st)
                {
                    fixed2 p = floor(st);
                    fixed2 f = frac(st);
                    fixed2 u = f * f * (3.0 - 2.0 * f);

                    float v00 = rand(p + fixed2(0, 0));
                    float v10 = rand(p + fixed2(1, 0));
                    float v01 = rand(p + fixed2(0, 1));
                    float v11 = rand(p + fixed2(1, 1));

                    return lerp(lerp(dot(v00, f - fixed2(0, 0)), dot(v10, f - fixed2(1, 0)), u.x),
                                lerp(dot(v01, f - fixed2(0, 1)), dot(v11, f - fixed2(1, 1)), u.x),
                                u.y) + 0.5f;
                }

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    //UV�X�N���[��
                    o.line_uv.y = v.line_uv.y - _Time.z * _LineSpeed;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float2 uv = i.uv;
                    //RGB���炵�ăz���O�������ۂ�
                    float r = tex2D(_MainTex, uv + _ColorGap * perlinNoise(_Time.z)).r;
                    float b = tex2D(_MainTex, uv - _ColorGap * perlinNoise(_Time.z)).b;
                    float2 ga = tex2D(_MainTex, uv).ga;
                    float4 shiftColor = fixed4(r, ga.x, b, ga.y);
                    //�m�C�Y���C���̕�Ԓl�v�Z
                    float interpolation = step(frac(i.line_uv.y * 15), _LineSize);
                    //�m�C�Y���C�����܂ރs�N�Z���J���[
                    float4 noiseLineColor = lerp(shiftColor, _LineColor, interpolation);
                    float posterize = floor(frac(perlinNoise(frac(_Time)) * 10) / (1 / _FrameRate)) * (1 / _FrameRate);
                    //uv.y�����̃m�C�Y�v�Z -1 < random < 1
                    float noiseY = 2.0 * rand(posterize) - 0.5;
                    //�O���b�`�̍����̕�Ԓl�v�Z �ǂ̍����ɏo�����邩�͎��ԕω��Ń����_��
                    float glitchLine1 = step(uv.y - noiseY, rand(uv));
                    float glitchLine2 = step(uv.y - noiseY, 0);
                    float glitch = saturate(glitchLine1 - glitchLine2);
                    //uv.x�����̃m�C�Y�v�Z -0.1 < random < 0.1
                    float noiseX = (2.0 * rand(posterize) - 0.5) * 0.1;
                    float frequency = step(abs(noiseX),_Frequency);
                    noiseX *= frequency;
                    //�O���b�`�K�p
                    uv.x = lerp(uv.x, uv.x + noiseX * _GlitchScale, glitch);
                    float4 noiseColor = tex2D(_MainTex, uv);
                    float4 finalColor = noiseLineColor * noiseColor;
                    //�A���t�@����
                    finalColor.a = _Alpha;
                    return finalColor;
                }
                ENDCG
            }
        }}
