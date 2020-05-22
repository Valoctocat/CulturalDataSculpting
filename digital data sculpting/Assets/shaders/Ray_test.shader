Shader "Unlit/Ray_test"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
			#define MAX_STEPS 100
			#define MAX_DIST 100
			#define BORD_DIST 1
			#define SURF_DIST 1e-2
			#define THICKNESS 1e-1



            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
				
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 ro : TEXCOORD1;
				float3 hitPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;





            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.ro = mul(unity_WorldToObject,float4(_WorldSpaceCameraPos,1) );// origin in world space
				o.hitPos = v.vertex;// origin relative to object
                return o;
            }
			float sdBox( float3 p, float3 b )
			{
			  float3 q = abs(p) - b;
			  return length(max(q,0.0)) + min(max(q.x,max(q.y,q.z)),0.0);
			}
			float opOnion( in float sdf, in float thickness )
			{
				 return abs(sdf)-thickness;
			}
			
			float sdCappedCylinder(float3 p, float h, float r)
			{
				float2 d = abs(float2(length(p.xz), p.y)) - float2(h, r);
				return (min(max(d.x, d.y), 0.0) + length(max(d, 0.0)));
			}


			float sdSphere(float3 p, float s, float3 ro)
			{
				//return length(p-float3(ro+(_SinTime[3])/10)) - s;
				return length(p-float3(ro)) - s;

			}
			
			//shape to build
			float GetDist(float3 p,float3 ro, bool cam) {
				
				//float d = length(p)-0.5;
				//float 
				//float d = max(opOnion(sdCappedCylinder(p, 0.1, 0.1),THICKNESS), -sdSphere(p,2,ro));
				float d=0;
				if (1)
				{
					d = max(opOnion(sdBox(p, float3(0.5,0.5,0.5)),THICKNESS), -sdSphere(p,0.5,ro));
				}
				else
				{
					d = opOnion(sdBox(p, float3(1,1,1)),THICKNESS);
				}

				return d;
			}

			
			

			float Raymarch(float3 ro, float3 rd) {
				float dO = 0;//distance origin
				float dS;//distance from scene
				for (int i = 0; i < MAX_STEPS; i++)
				{
					float3 p = ro + dO*rd;
					dS = GetDist(p,ro,1);//distance to scene
					dO += dS;
					if (dS<SURF_DIST || dO>MAX_DIST) 
						break;
					
				}
				return dO;
			}

			float3 GetNormal(float3 p, float3 ro) {
				float2 e = float2(1e-2, 0);
				//approximation de la tangente de notre surface pour trouver la normale
				float3 n = GetDist(p, ro,0) - float3(
					GetDist(p - e.xyy,ro,0),
					GetDist(p - e.yxy,ro,0),
					GetDist(p - e.yyx,ro,0)
					);
				return normalize(n);
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float2 uv = i.uv-0.5;
				float3 ro = i.ro;
				float3 rd = normalize(i.hitPos - ro);//normalize(float3(uv.x, uv.y, 1));

				float d = Raymarch(ro, rd);
				//float d = GetDist(ro+rd);

                fixed4 col = tex2D(_MainTex, i.uv);
				if (d > MAX_DIST) {
					discard;
				}
				else {
					float3 p = ro + rd * d;
					if (d == 0.05) {
						col = 1;
					}

					else {
						
						float3 n = GetNormal(p, ro);
						if(length(col)>(1.02+_SinTime[2] / 52)){
							col.rgb = n*0.2 + col*( (_SinTime[3] / 2) + 1.02);
							
							}
						else{
							col.rgb = n+col.rgb/2;
						}

					
					}
					

				}

				
                return col;
            }
            ENDCG
        }
    }
}
