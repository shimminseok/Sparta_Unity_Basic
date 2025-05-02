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
### 1. 싱글톤 패턴 적용 (Singleton<T>)
목적: 매니저 클래스(GameManager, UIManager, LoadSceneManager 등)는 전역 접근이 필요하고, 게임 전체에서 단 하나만 존재해야 하기 때문.

구현 포인트:

DontDestroyOnLoad로 씬 전환 시 유지

Awake()에서 중복 인스턴스 제거

인스턴스가 존재하지 않으면 자동 생성 (new GameObject)

장점:

전역 접근 가능

씬 간 데이터 유지

테스트 및 디버깅 편의성

### 2. 상태머신 설계 (StateMachine<T>, IState<T>)
목적: 플레이어의 동작 상태(Idle, Move 등)를 명확히 분리하여 복잡도를 줄이고, 확장성 높은 구조를 만들기 위해.

구현 구조:

StateMachine<T> 클래스는 상태 변경과 생명주기 관리 (OnEnter, OnExit, Update, FixedUpdate)

IState<T> 인터페이스를 통해 상태별 클래스를 정의 (IdleState, MoveState)

장점:

코드 가독성과 유지보수성 향상

각 상태가 자기 책임만 가지므로 기능 분리가 명확

상태 추가 시 기존 코드 수정 없이 확장 가능

### 3. 인터페이스 분리 원칙 적용
예시 인터페이스:

IInterfactable: 상호작용 가능한 객체(NPC, 레버 등)의 통합 인터페이스

INPCFunction: NPC 기능을 담당하는 컴포넌트의 공통 인터페이스

ITable: ScriptableObject 기반 테이블의 공통 생성 인터페이스

장점:

DIP(의존성 역전 원칙) 실현

기능별 책임 분리 → 테스트 단순화, 유지보수 쉬움

다형성을 활용한 유연한 기능 확장

### 4. A* 경로 탐색 알고리즘 (TileMapAStar)
목적: 마우스 클릭 기반 자동 이동 시 장애물을 피해서 자연스러운 경로를 찾기 위함

구현 요소:

Node 클래스 기반의 G/H/F 비용 계산

walls HashSet으로 타일맵 기반 충돌 정보 캐싱

IsDiagonalMoveBlocked로 대각선 충돌 예외 처리

장점:

복잡한 맵에서도 정확하고 빠른 경로 탐색 가능

키보드와 병행 가능한 이동 타입 구현

향후 몬스터 AI, NPC 자동 이동에도 재사용 가능

### 5. ScriptableObject 기반 데이터 테이블
적용 예시:

NPCTable, MiniGameTable, TransformTable 등

핵심 구현:

TableManager에서 CreateTable() 자동 호출

AutoAssignTables()를 통해 에디터에서 자동으로 ScriptableObject 등록

장점:

데이터 중심 설계 가능 (디자이너와 협업 용이)

런타임 중 불필요한 중복 생성 없이 테이블 관리

타입 기반으로 안전하게 테이블 접근 (GetTable<T>())

### 6. UI 모듈화 및 공통화
UIBase 클래스를 통해 공통 기능 정의: Open(), Close() 등

Manager 패턴과 함께 동작: UIManager가 중복 팝업 방지 및 관리

콜백 구조 적용: UIDialogue.StartDefaultDialogue(dialogues, onComplete) 식으로 연동

장점:

UI 오픈/클로즈 로직 일관화

대화 → 미니게임 → 씬 전환 등의 흐름을 깔끔하게 연결 가능

향후 팝업 추가 시 UIBase만 상속하면 자동 적용됨

### 7. 변신 시스템
기능: 플레이어가 특정 외형(AnimatorController)과 속도를 바꿔 전환

핵심 요소:

UITransform → TransformData 선택 → PlayerController.TransformTo() 호출

ScriptableObject로 데이터화 → 애니메이션, 속도, 이름 등 자유 설정 가능

장점:

유저 커스터마이징 요소 강화

외형 전환 시 애니메이션 변경과 이동 로직 일괄 적용

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
