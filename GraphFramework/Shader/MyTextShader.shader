Shader "Unlit/MyTextShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}//这个大括号干啥的，white是定义基础颜色？
        _Alpha("Alpha",range(0,1))  = 0 
    }
    SubShader
    {
        //Tags有啥用？
        Tags { "RenderType"="Transparent"  "Queue" = "AlphaTest"}
        
        LOD 100//这个是啥？层次细节100？定义纹理的mipmap？

        //需求：实现一个基本的渲染纹理和网格的着色器，不需要光照，不需要阴影，特定的颜色混合模式，两个绿色混合使用（a+b）*0.5
        Pass
        {
            //ZWrite off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"


            float _Alpha;
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : Normal;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                
                float4 vertex : SV_POSITION;
                float3 normal: Normal;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = v.normal *0.5 + float3(0.5,0.5,0.5);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                if(_Alpha < 0.5)
                {
                    discard;
                }
                //return fixed4(col,min(max(_Alpha ,0),1)  );
                return fixed4(i.normal,0);
            }
            
            ENDCG
        }
    }
}
