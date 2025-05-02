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

### 🧠 기술 설계 요약

| 항목 | 설명 |
|------|------|
| **디자인 패턴** | 싱글톤 패턴, 상태머신 패턴, 인터페이스 분리, 팩토리 메서드 기반 NPC 컴포넌트 부착 |
| **경로 탐색** | A* 알고리즘 커스텀 구현 (타일맵 기반 + 대각선 이동 고려) |
| **UI 설계** | `UIBase` 상속 기반 확장성 높은 구조, Manager 통해 일괄 관리 |
| **데이터 중심 구조** | ScriptableObject 테이블 자동 등록 및 타입 기반 조회 |
| **모듈화** | 각 시스템은 매니저/컨트롤러로 분리되어 책임 명확화 |
| **확장성** | NPC 기능, 변신 종류, 미니게임 등은 Enum 및 인터페이스로 유연하게 확장 가능 |

---
## 🧠 기술 설계 상세 설명
### ✅ 싱글톤 패턴 적용 (Singleton<T>)

**설계 목적**
매니저 클래스는 게임 전역에서 하나만 존재해야 하며, 씬 전환 간에도 유지되어야 합니다.

**구현 방식**
- `DontDestroyOnLoad`로 씬 전환 시 오브젝트 유지
- `Awake()`에서 중복 인스턴스 제거
- 존재하지 않을 경우 자동 생성(`new GameObject`)

**적용 클래스**
- `GameManager`, `UIManager`, `LoadSceneManager`, `TableManager`, `UIMinigamePanel`, `UIDialogue` 등

**장점**
- 전역 접근 용이
- 중복 인스턴스 방지
- 자동 초기화로 관리 편의성 향상

### ✅ 상태머신 구조 (`StateMachine<T>`, `IState<T>`)

**설계 목적**  
플레이어 상태(Idle, Move 등)를 명확하게 분리하여 복잡한 로직을 간결하게 유지합니다.

**구현 방식**
- `StateMachine<T>`는 상태의 생명주기(`OnEnter`, `OnExit`, `OnUpdate`, `OnFixedUpdate`)를 관리
- `IState<T>` 인터페이스로 상태 클래스 정의

**장점**
- 유지보수 및 확장성 우수
- 각 상태가 책임을 분리하여 독립적으로 관리됨
- 새로운 상태 추가 시 기존 코드에 영향 없음(OCP 원칙 적용)

### ✅ 인터페이스 기반 상호작용

**설계 목적**  
NPC, 레버 등 다양한 오브젝트에 대해 일관된 상호작용 구조를 제공하여 유연하게 확장 가능하도록 설계합니다.

**정의된 인터페이스
- `IInterfactable`: 상호작용 인터페이스 (`Interact()`, `Exit()`)
- `INPCFunction`: NPC 기능 실행 인터페이스
- `ITable`: ScriptableObject 테이블 초기화 인터페이스

**장점**
- DIP(의존성 역전 원칙) 실현
- 기능별 책임 분리 → 테스트 단순화, 유지보수 쉬움
- 다형성을 활용한 유연한 기능 확장

### ✅ A* 경로 탐색 알고리즘 (`TileMapAStar.cs`)

**설계 목적**  
마우스 클릭을 통해 타일 기반 이동 시 장애물을 회피하며 최적의 경로를 찾기 위해 A* 알고리즘을 사용합니다.

**핵심 구성**
- `Node` 클래스: F = G + H 비용 계산
- 맨해튼 거리 기반 휴리스틱 사용
- 벽 정보는 `Tilemap`을 기반으로 `HashSet`에 저장
- 대각선 이동 시 충돌 여부 체크 포함

**장점**
- 장애물 회피 자동 경로 이동 구현 가능
- 대각선 이동 등 실제 맵 조건을 반영
- 향후 NPC, Monster AI에도 재활용 가능

### ✅ ScriptableObject 기반 데이터 테이블

**설계 목적**
게임 데이터(NPC, 미니게임, 변신 등)를 외부 데이터 자산으로 관리하여 코드와 데이터를 분리합니다.

**구현 방식**
- `ITable` 인터페이스를 통해 테이블마다 `CreateTable()`정의
- `TableManager`가 실행 시 모든 테이블을 자동 초기화 및 등록
- 에디터에서 `AutoAssignTables()`로 ScriptableObject 자동 검색 가능

**적용 테이블 예시**
- `NPCTalbe`, `MiniGameTable`, `TransformTable`

**장점**
- 디자이이너와의 협업에 적합
- 런타임 중 안전한 타입 기반 접근 가능
- 유지보수 및 확장 용이(데이터 중심 설계)

### ✅ UI 모듈화 및 공통화

**설계 목적**
모든 UI패널의 공통 동작을 통합ㅂ하고, 중복 UI 처리 문제를 방지합니다.

**핵심 구조**
- `UIBase` 클래스를 통해 `Open()`, `Close()` 기본 동작 정의
- `UIManager`가 중복 제어 처리
- 팝업 UI는 `OpenedPopup` 리스트로 관리

**장점**
- UI 오픈/클로즈 로직 일관화
- 대화 → 미니게임 → 씬 전환 등의 흐름을 깔끔하게 연결 가능
- 향후 팝업 추가 시 UIBase만 상속하면 자동 적용됨

### ✅ 변신 시스템

**설계 목적**
플레이어의 외형 및 속도를 동적으로 변경하여 커스터마이징 기능을 제공합니다.

**구현 방식**
- `TransformData`를 `ScriptableObject`로 관리
- `UITransform`에서 변신 목록을 동적으로 생성 및 선택 처리
- `PlayerController.TransformTo()`에서 애니메이터와 이동속도 적용

**핵심 요소**
- UITransform → TransformData 선택 → PlayerController.TransformTo() 호출
- ScriptableObject로 데이터화 → 애니메이션, 속도, 이름 등 자유 설정 가능


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
