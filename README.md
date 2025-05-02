# 🧩 Unity 2D 메타버스 프로젝트

이 프로젝트는 Unity 기반 2D으로 메타버스 환경을 만든 프로젝트로, 상호작용 가능한 NPC, 커스터마이징 가능한 변신, 레버-문 제어, A* 기반 경로 탐색, 싱글톤 매니저 기반 아키텍처 등을 포함하고 있습니다.

---

## 📌 핵심 구조 및 기능 요약

### 🎮 플레이어 & 상태머신
- **`PlayerController`**  
  - `StateMachine<T>` 기반 FSM(Finite State Machine) 구현
  - 상태 전환: `Idle` ↔ `Move`
  - `IState<T>` 인터페이스 구현을 통한 상태 독립성 확보
- **`PlayerMoveController`**  
  - 키보드, 마우스 클릭, A* 기반 자동 경로 탐색 지원
  - 이동 타입 분리: `None`, `Keyboard`, `Mouse`, `AStar`

### 🗺️ A* 경로 탐색
- **`TileMapAStar`**
  - 맨해튼 거리 기반 휴리스틱 사용
  - 벽 정보는 `Tilemap`으로부터 받아 HashSet으로 캐싱
  - 대각선 이동 시 충돌 체크 지원
  - 커스텀 `Node` 클래스 및 오픈리스트 기반 탐색 구현

### 🧱 상호작용 및 NPC 시스템
- `IInterfactable` 인터페이스 기반 상호작용 통일화
- `NPCController`는 `NPCFunction` 플래그 기반으로 `MiniGameNPC`, `StoryNPC` 컴포넌트 동적 부착
- 각 NPC 기능은 `INPCFunction` 인터페이스로 정의

### ⚙️ 매니저 및 공통 구조
- `Singleton<T>`: `DontDestroyOnLoad`, 중복 제거, 동적 생성 지원
- `TableManager`: ScriptableObject 기반 테이블 자동 등록
- `LoadSceneManager`: 비동기 씬 전환 처리
- `UIManager`: UIBase 기반 팝업 관리, 중복 방지

### 🧙 변신 시스템
- `UITransform`에서 선택한 `TransformData`에 따라 플레이어 외형 변경 (`AnimatorController + 속도 변경`)
- 변신 상태는 `Animator.runtimeAnimatorController`를 기준으로 구분

### 🧩 UI 시스템
- `UIBase`: 모든 UI 패널의 공통 기능 정의
- `UIDialogue`: 대사 출력, F 키로 진행, onComplete 콜백 처리
- `UIHUD`: 상호작용 가능한 오브젝트 진입 시 애니메이션 UI 출력
- `UIMinigamePanel`: 선택된 미니게임 타입에 따라 동적 씬 전환 및 최고 점수 표시

---

## 🧠 기술 설계 요약

| 항목 | 설명 |
|------|------|
| **디자인 패턴** | 싱글톤 패턴, 상태머신 패턴, 인터페이스 분리, 팩토리 메서드 기반 NPC 컴포넌트 부착 |
| **경로 탐색** | A* 알고리즘 커스텀 구현 (타일맵 기반 + 대각선 이동 고려) |
| **UI 설계** | `UIBase` 상속 기반 확장성 높은 구조, Manager 통해 일괄 관리 |
| **데이터 중심 구조** | ScriptableObject 테이블 자동 등록 및 타입 기반 조회 |
| **모듈화** | 각 시스템은 매니저/컨트롤러로 분리되어 책임 명확화 |
| **확장성** | NPC 기능, 변신 종류, 미니게임 등은 Enum 및 인터페이스로 유연하게 확장 가능 |

---

## 🚀 시연 예시 기능

- 키보드/마우스 기반 8방향 이동 및 자동 경로 탐색
- 특정 NPC와 대화 → 미니게임 진입
- 레버 조작 시 문 타일 상태 및 콜라이더 동기화
- 변신 UI를 통한 외형 및 이동속도 전환

---

## ✅ 요구 환경
- Unity 2022.3.17f1
- TextMeshPro 필수
- URP
