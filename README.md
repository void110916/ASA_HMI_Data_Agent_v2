# ASA HMI Data Agent v2 (asa develop tool)

## 簡介
包含開發ASA系列產品會用到的開發工具，稱為 asa develop tool。

此專案為[原python專案](https://gitlab.com/MVMC-lab/hmi/ASA_HMI_Data_Agent)以C# WinForm改寫而成，以改進python效能不足的問題。
## 功能

1. 終端與資料傳輸
    * 作為終端與ASA_M128對話
    * 透過HMI封包格式，與ASA_M128傳輸大量資料
2. 燒錄ASA_M128
   * 利用asaloader以指令方式燒綠資料
   * 提供GUI方便使用者使用

## 安裝方法

到[發布頁面](https://github.com/void110916/ASA_HMI_Data_Agent_v2/releases)下載打包好的執行檔