Shader "Unlit/Unlit with Shadow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LambertValue ("LambertMultiplier",Range(0.0,1.0)) = 0.25
        _Ambient("Ambient",Range(0.0,1.0)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {

         Tags{
                "LightMode" = "UniversalForward"
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Lighting.cginc"


            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _LambertValue;
            float _Ambient;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 NdotL = dot(i.normal, lightDir);
                float3 diffuse = (NdotL * _LambertValue + _Ambient) * _LightColor0.rgb * col.xyz;
                float4 ret = float4(diffuse, 1);
                return ret;
            }
            ENDCG
        }

        // Pass to render object as a shadow caster
        Pass
        {
            Name "CastShadow"
            Tags { "LightMode" = "ShadowCaster" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"

            struct v2f
            {
                V2F_SHADOW_CASTER;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }

            float4 frag(v2f i) : COLOR
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
}
