struct appdata
            {
                float4 positionOS   : POSITION;
                float4 tangentOS: TANGENT;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct vertOut
            {
                float4 positionCS  : SV_POSITION;
                float2 waterUV:TEXCOORD0;
                float2 foamUV:TEXCOORD5;
                float4 screenPosition: TEXCOORD1;
                float3 normalWS: NORMAL;
                float3 tangentWS : TEXCOORD3;
                float3 biTangent : TEXCOORD2;
                float3 positionWS: TEXCOORD4;
            };

 
            sampler2D _CameraOpaqueTexture;
            CBUFFER_START(UnityPerMaterial)
                
                sampler2D _MainTex;
                float _TextureBlend;
                float4 _MainTex_ST;
                half4 _BaseColor;
                half4 _FoamColor;
                half4 _SurfaceColor;
                half4 _BottomColor;
                float _Depth;
                float _WaveSpeed;
                float _WaveScale;
                float _WaveStrength;
                float _FoamAmount;
                float _FoamCutoff;
                float _FoamSpeed;
                float _FoamScale;
                float _SpecularIntensity;
                float _Gloss;
                float _Smoothness;
                float _NoiseNormalStrength;
                float _WaterShadow;
            CBUFFER_END

            

            vertOut vert(appdata v)
            {
                vertOut o;

                _MainTex_ST.zw += _Time.y*_WaveSpeed;
                _MainTex_ST.xy *= _WaveScale;
                o.waterUV = TRANSFORM_TEX(v.uv, _MainTex);

                _MainTex_ST.zw += _Time.y*_FoamSpeed;
                _MainTex_ST.xy *= _FoamScale;
                o.foamUV = TRANSFORM_TEX(v.uv, _MainTex);

                float waterGradientNoise;
                Unity_GradientNoise_float(o.waterUV, 1, waterGradientNoise);
                v.positionOS.y += _WaveStrength*(2*waterGradientNoise-1);

                o.positionWS = GetVertexPositionInputs(v.positionOS.xyz).positionWS;
                o.normalWS = GetVertexNormalInputs(v.normalOS).normalWS;//conver OS normal to WS normal
                o.tangentWS = GetVertexNormalInputs(v.normalOS,v.tangentOS).tangentWS;
                o.biTangent = cross(o.normalWS, o.tangentWS)
                              * (v.tangentOS.w) 
                              * (unity_WorldTransformParams.w);

                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.screenPosition = ComputeScreenPos(o.positionCS);

                return o;
            }

            float DepthFade (float rawDepth,float strength, float4 screenPosition){
                float sceneEyeDepth = LinearEyeDepth(rawDepth, _ZBufferParams);
                float depthFade = sceneEyeDepth;
                depthFade -= screenPosition.a;
                depthFade /= strength;
                depthFade = saturate(depthFade);
                return depthFade;
            }

            //float3 _LightDirection;
            half4 frag(vertOut i, FRONT_FACE_TYPE frontFace : FRONT_FACE_SEMANTIC) : SV_Target//get front face of object
            {
                
                
                float2 screenSpaceUV = i.screenPosition.xy/i.screenPosition.w;
                
                float rawDepth = SampleSceneDepth(screenSpaceUV);
                float depthFade = DepthFade(rawDepth,_Depth, i.screenPosition);
                float4 waterDepthCol = lerp(_BottomColor,_SurfaceColor,1-depthFade);




                float waterGradientNoise;
                Unity_GradientNoise_float(i.waterUV, 1, waterGradientNoise);

                float3 gradientNoiseNormal;
                float3x3 tangentMatrix = float3x3(i.tangentWS, i.biTangent,i.normalWS);
                Unity_NormalFromHeight_Tangent_float(waterGradientNoise, 0.1,i.positionWS,tangentMatrix,gradientNoiseNormal);
                gradientNoiseNormal *= _NoiseNormalStrength;

                gradientNoiseNormal += i.screenPosition.xyz ;
                float4 gradientNoiseScreenPos = float4(gradientNoiseNormal,i.screenPosition.w );
                float4 waterDistortionCol = tex2Dproj(_CameraOpaqueTexture,gradientNoiseScreenPos);



                float foamDepthFade = DepthFade(rawDepth,_FoamAmount, i.screenPosition);
                foamDepthFade *= _FoamCutoff;

                float foamGradientNoise;
                Unity_GradientNoise_float(i.foamUV, 1, foamGradientNoise);

                float foamCutoff = step(foamDepthFade, foamGradientNoise);
                foamCutoff *= _FoamColor.a;

                float4 foamColor = lerp(waterDepthCol, _FoamColor, foamCutoff);


                float4 mainTex = tex2D(_MainTex,i.waterUV);
                float4 finalCol = lerp(waterDistortionCol, foamColor, foamColor.a);
                finalCol = lerp(mainTex,finalCol,_TextureBlend);




                float3 gradientNoiseNormalWS;
                Unity_NormalFromHeight_World_float(waterGradientNoise,0.1,i.positionWS,tangentMatrix,gradientNoiseNormalWS);

                InputData inputData = (InputData)0;//declare InputData struct
                inputData.normalWS = gradientNoiseNormalWS;// if front face return 1 else return -1
                //inputData.positionWS = i.positionWS;
                inputData.viewDirectionWS = GetWorldSpaceNormalizeViewDir(i.positionWS);//get view dir base on positionWS
               
                SurfaceData surfaceData = (SurfaceData)0;//declare SurfaceData 
                surfaceData.albedo = float3(1,1,1)*_WaterShadow;
                surfaceData.alpha = 1;
                surfaceData.specular = _Gloss;
                surfaceData.smoothness = _Smoothness;
                
               //return float4( gradientNoiseNormalWS,1);
               return finalCol +UniversalFragmentBlinnPhong(inputData , surfaceData)*_SpecularIntensity;
               //return UniversalFragmentBlinnPhong(inputData , surfaceData);
            }