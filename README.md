# 1151VR-HW3-414262351-DPT


- YouTube : <https://youtu.be/urjCxNOXjpA?si=dRsOO_W79ap1nBL->
- GitHub : <https://github.com/cheese91383/1151VR-HW3-414262351-DPT>

## 成品功能

- 點擊遊戲畫面 ( 滑鼠或手機觸控 ) ，輪盤開始旋轉，並隨著摩擦力慢慢減速直到停下。
- 停下後，輪盤指針所指的格子內容 ( 例如:咖哩、炸雞 ) 會顯示在 Debug Console。
- 轉動時播放旋轉音效

## 製作流程

### 1. 匯入素材

- 圖片 : 繪製完後把輪盤圖與指針圖拖進 `Assets/Sprite`，再拖進場景(Scene)。
- 指針放在輪盤正上方
- 輪盤的第一格要對齊指針的位置，從指針處開始、依順時針方向排列。
- 更換背景顏色
- 音效 : 使用 <https://elevenlabs.io/sound-effects/roulette> 的 Roulette wheel spinning，下載後放進 `Assets/Audio`。來源: elevenlabs.io。

### 2. 旋轉腳本 RouletteController.cs

在 `Assets/Script` 建立 `RouletteController.cs`，並掛在輪盤物件上。腳本的運作邏輯如下 :

| 功能 | 做法 |
|---|---|
| 旋轉 | 在 Update 中每一幀使用 `transform.Rotate` 轉動，並乘上 `Time.deltaTime`，讓轉速不受電腦快慢影響。 |
| 減速 | 每一幀把轉速乘上指數衰減 `Mathf.Exp(-friction * Time.deltaTime)`，模擬摩擦力，越轉越慢；轉速低於 1 度/秒就視為停止。 |
| 判斷結果 | 輪盤順時針轉 θ 度後，指針下方是「原本逆時針 θ 度處」的格子，因此用「目前角度 − 初始角度」除以每格的角度，取整數得到格子編號，再用編號到名稱陣列中取出內容。 |
| 輸入 | 用 `Mouse.current`(滑鼠左鍵)與 `Touchscreen.current`(手機觸控)判斷點擊 |
| 音效 | 腳本標示 `RequireComponent(typeof(AudioSource))`，掛上後自動加入 Audio Source。轉動時循環播放 Spin Clip，音調隨轉速降低 |

### 3. Inspector 設定

選到輪盤物件，在 Roulette Controller 元件中設定以下欄位:

| 欄位 | 說明 | 預設值 |
|---|---|---|
| Sector Count | 輪盤總共分成幾格。 | 8 |
| Turns Range | 每次旋轉總圈數的隨機範圍，兩個數字相差至少 1。 | 5 ~ 10 |
| Friction | 減速係數，數字越大停得越快。 | 1.5 |
| Sector Names | 每一格的內容 ( 拉麵、雞排 … )。Size 設成格數，從指針正下方那格開始，依順時針順序填入 Element 0、1、2 …。 | |
| Spin Clip | 旋轉時循環播放的音效。 | |
| Max / Min Pitch | 轉速最快與最慢時的音調。 | 0.63 / 0.39 |

### 4. 在 Android 上執行

- 切換平台 : File 點 Build Profiles，選 Android，按 Switch Profile。
- 加入場景 : 在 Scene List 勾選 SampleScene。
- Player 設定 : Project Settings 點 Player，輸入 company 名稱，填入 Package Name；橫式遊玩時 : Resolution and Presentation 的 Default Orientation 選 Landscape Left。
- 輸出 APK
- 安裝到手機
- 手機端的點擊由 Touchscreen 判斷處理，所以觸碰螢幕任何位置即可開始旋轉。
