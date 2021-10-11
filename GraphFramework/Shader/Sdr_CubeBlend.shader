Shader "Unlit/Sdr_CubeBlend"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Alpha("CutOff", Range(0,1)) = 1
        _Color("Main Tint", Color) = (1,1,1,1)
    }
    SubShader
    {
        //IgnorProjector 有啥用？ 着色器不会受到projector（投影器）的影响，是不会被透视效果影响吗？
        Tags { "Queue " = "Transparent""IngnorProjector" = "True" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Tags{"LightMode" = "ForwardBase"}//让unity以基本的前向渲染路径提供必要的光照参数
            ZWrite off
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0; 
                float4 vertex : SV_POSITION;
            };

            fixed _Alpha;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
              
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                  
                  if(col.a - _Alpha < 0)
                  {
                      discard;
                  }
                //return col;
                return fixed4(col.rgb,_Alpha);

            }
            ENDCG
        }
    }
}
