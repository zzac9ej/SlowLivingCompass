# 系統架構與實作計畫 (Implementation Plan)

## 目標
根據慢活靈感指南 (Slow-Living Compass) 的 PRD，構建具備「氛圍推薦」、「動態打卡紀錄」與「單機輪流揪團」功能的 MVP 版本。提供極簡、無壓力的前端體驗。**本專案將完全依賴純靜態前端架構，可完美託管於 GitHub Pages 上。**

## 系統架構解決方案 (Solution Architecture)

#### [NEW] `SlowLivingCompass.sln`
主解決方案容器。

#### [NEW] `SlowLivingCompass.Client` (Blazor WebAssembly Standalone)
* 整個應用程式將拋棄後端 API，100% 下載至瀏覽器執行 (Stand-alone WebAssembly)。
* **資料來源**：將初期的商店名單與標籤資料，以 JSON 格式或 C# 靜態類別打包進 WebAssembly 專案中。
* **狀態永續**：針對「探索護照」，使用瀏覽器的 `LocalStorage` 紀錄使用者打卡狀態。
* **UI 框架**：導入 MudBlazor 提供簡潔清晰的介面。

---

### 2. 核心功能實作策略 (Core Features)

#### AC1: 發現氛圍契合度 (Vibe Match)
* **設計**：設計直覺的「心情標籤 (Mood Chips)」供使用者進行複選。
* **邏輯**：直接在前端 C# `PlaceService` 單例服務中，計算所選標籤與內建資料集景點的相似度，依最高契合度進行單一或少數推薦。不進行網路傳輸，反應極快。

#### AC2: 療癒的足跡紀錄 (Visualized Journey)
* **紀錄**：呼叫 `Blazored.LocalStorage` 套件，寫入景點的 Id 作為造訪紀錄。
* **UI顯示**：讀取 LocalStorage 後，基於造訪數量利用動態 SVG 或預載漸進式圖片呈現地圖。

#### AC3: 佛系揪團共識 - 單機輪流盲測 (Pass-and-Play)
* **流程改變**：不再傳送網址，而是朋友聚在一起時發起。
* **投票處理**：發起人設定參與人數，系統挑選 5 間候選餐廳。畫面提示「請將手機交給第 1 位朋友」，該人對 5 張照片滑動點擊 ❤️/✖️。完成後畫面提示「請交給第 2 位朋友」，以此類推。
* **結果判定**：所有人在同一支設備上完成後，前端立即統計交集，最高票勝出。平手由 C# `Random` 抽籤，直接公佈最終結果。

---

### 3. 發布與極端情境 (Deployment & Edge Cases)

* **GitHub Pages 部署**：
  在 `Program.cs` 調整基底路徑 (`<base href="/" />`)，透過 GitHub Actions 編譯 `.NET 8/9 WASM` 並發布到 `gh-pages` 分支。
* **PWA 支援 (離線可用)**：
  開啟 Blazor WebAssembly 的 Progressive Web App 支援，確保使用者在開啟網頁後，即使騎車進入沒有訊號的地區，依然能無縫瀏覽本地圖資與進行 AC3 的單機投票。
