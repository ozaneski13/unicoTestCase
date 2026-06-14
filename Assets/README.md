Varsayımlar

-Kazanma koşulu -> Wavedeki bütün düşmanların ölmesi
-Kaybetme koşulu -> Bir düşmanın en alt safe noktaya ulaşması
-Düşmanlar turretlara hasar verebiliyor. Yok edince yoluna devam ediyor.
-Spawn koşulları firstDelay vs.
-Turret yerleşim şu an için mouse ile yapılıyor.
-3D
-Kolay editlenebilir olması adına ScriptableObject driven hazırladım.
-Eventler ağırlıklı olarak SO üzerinden ilerliyor. UniTask ya da düz event sistemi kullanılabilir, kendi projelerimde bunu tercih ettiğim için bunu kullandım.
-Turret sistemi harici pooling var. Turret sisteminde gerek duymadım.
-Turretlar mermisiz de hasar verebilirdi ancak zaman kalırsa mermilere visual eklemek istediğim için hit based bir sistem hazırladım.