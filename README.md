**Test for Unity Developer – Combat Game in Unity (Portrait Mode)**

## Gameplay Overview

Là một trò chơi chiến đấu cận chiến trong sàn Boxing, người chơi tham gia các trận đấu hấp dẫn dưới dạng màn hình dọc (portrait). Trò chơi hỗ trợ 3 chế độ chơi:

- **1 vs 1** – Người chơi solo với 1 AI
- **1 vs Many** – Một mình chống lại một nhóm kẻ địch
- **Many vs Many** – Trận đánh đồng đội với nhiều chiến binh trên cả hai bên

###  Cơ chế điều khiển:

| Thao tác                             | Hành động                         |
|-------------------------------------|-----------------------------------|
| Vuốt (trái/phải/lên/xuống)         | Di chuyển nhân vật                |
| Nhấn giữ (Hold)                     | **Phòng thủ (Block)** khi đang trong trạng thái combat |
| Nhấn đúp (Double Tap)               | **Tấn công (Attack)** khi đang trong trạng thái combat |

> **Lưu ý về Tấn công**:
- Chỉ thực hiện được khi người chơi đang trong **trạng thái combat**.
- **Không thể spam liên tục** – có **thời gian chờ giữa mỗi đúp nhấn** (cooldown).
- Nếu nhấn đúp sai thời điểm, đòn đánh sẽ không được tung ra.


###  Logic trạng thái:

- Khi nhân vật **chạm mặt địch thủ**, sẽ chuyển sang **trạng thái combat**.
- Trong trạng thái này:
  - Kích hoạt block (giữ)
  - Kích hoạt tấn công (nhấn đúp)
---

## Chế Độ Chơi

| Chế độ       | Mô tả                                                                 |
|--------------|-----------------------------------------------------------------------|
| 1 vs 1       | Người chơi đấu tay đôi với 1 kẻ địch trong sàn Boxing                 |
| 1 vs Many    | Một mình người chơi chống lại nhóm địch được AI điều khiển            |
| Many vs Many | Người chơi phối hợp cùng đồng đội AI chống lại nhóm địch được AI điều khiển|

---

