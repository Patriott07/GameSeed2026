Shader "Custom/RadiantGradientFallOff"
{
    Properties
    {
        // Warna area api (default oranye kemerahan)
        [HDR] _Color ("Aura Color", Color) = (1, 0.4, 0, 1)
        
        // Seberapa tajam gradien pudarnya
        _Falloff ("Falloff Power", Range(0.1, 10.0)) = 2.0
    }
    
    SubShader
    {
        // Mengatur agar shader ini transparan dan tidak menutupi objek lain
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back // Tidak merender bagian dalam bola

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL; // Arah hadap tiap vertex
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
            };

            fixed4 _Color;
            float _Falloff;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                
                // Ambil arah hadap (normal) di dunia 3D
                o.worldNormal = UnityObjectToWorldNormal(v.normal);

                // Hitung arah dari kamera menuju ke titik di bola tersebut
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDir = normalize(UnityWorldSpaceViewDir(worldPos));
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Pastikan vektor arahnya akurat
                float3 normal = normalize(i.worldNormal);
                float3 viewDir = normalize(i.viewDir);

                // MATEMATIKA INTI: Dot Product
                // Menghitung seberapa lurus permukaan menatap kamera.
                // Hasilnya: 1 (Tengah) sampai 0 (Pinggir/Siluet).
                float NdotV = max(0, dot(normal, viewDir));

                // Aplikasikan falloff (membuat gradiennya lebih tajam/lembut)
                float alpha = pow(NdotV, _Falloff);

                // Ambil warna utama, lalu kalikan transparansinya dengan hasil hitungan
                fixed4 finalColor = _Color;
                finalColor.a *= alpha;

                return finalColor;
            }
            ENDCG
        }
    }
}