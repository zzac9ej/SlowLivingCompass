# 慢活靈感指南 (Slow-Living Compass) 開發任務清單

本清單依據 `PRD.md` 與 `implementation_plan.md` 的規劃進行拆解，供開發階段追蹤使用。

## 1. 專案基礎建設 (Project Setup)
- [ ] 建立主要方案 `SlowLivingCompass.sln`
- [ ] 建立 Blazor WebAssembly Standalone 專案 `SlowLivingCompass.Client`
- [ ] 導入 `MudBlazor` UI 元件庫
- [ ] 導入 `Blazored.LocalStorage` 套件
- [ ] 啟用 PWA 功能以支援離線可用性

## 2. 核心資料與服務層 (Data & Services)
- [ ] 建立靜態景點資料集 (C# Static Class 或內建 JSON)，包含店名、氛圍標籤、照片路徑等。
- [ ] 實作 `PlaceService` 單例服務，負責資料讀取與標籤相似度計算。
- [ ] 實作 `JourneyService`，封裝 LocalStorage 的打卡紀錄讀寫邏輯。

## 3. UI 與功能實作：AC1 氛圍契合度 (Vibe Match)
- [ ] 實作首頁「尋找避難所」元件。
- [ ] UI 設計：實作直覺的「心情標籤 (Mood Chips)」供使用者複選。
- [ ] 邏輯銜接：呼叫 `PlaceService`，基於使用者選擇的標籤計算並推薦契合度最高的隱藏版空間。

## 4. UI 與功能實作：AC2 療癒的足跡紀錄 (Visualized Journey)
- [ ] 實作「探索護照 (Passport)」頁面。
- [ ] 邏輯銜接：讀取 LocalStorage 內的歷史造訪數量。
- [ ] UI 設計：根據造訪數量，動態替換或繪製 SVG/漸進式圖片，呈現手繪美食地圖的成長變化。

## 5. UI 與功能實作：AC3 單機輪流盲測 (Pass-and-Play)
- [ ] 實作「佛系下午茶」揪團設定介面（設定參與人數）。
- [ ] 隨機抽選 5 間候選餐廳，並準備對應之照片。
- [ ] 實作單機輪流投票介面：
  - [ ] 顯示「請將手機交給第 N 位朋友」的提示卡。
  - [ ] 針對 5 張照片實作 ❤️ 或 ✖️ 的選擇介面。
- [ ] 實作計票邏輯：
  - [ ] 統整交集票數。
  - [ ] 實作平手亂數抽籤 (Random) 判斷。
  - [ ] 顯示最終勝出店家的結果頁面。

## 6. 部署與上線 (Deployment)
- [ ] 調整 `Program.cs` 或 `wwwroot/index.html` 中的基底路徑 (`<base href="/" />`) 以適應 GitHub Pages。
- [ ] 撰寫 GitHub Actions Workflow (`.yml`)，實現推播主分支時自動編譯 `.NET WASM` 並部署至 `gh-pages` 分支。
- [ ] 執行封閉測試驗證。
