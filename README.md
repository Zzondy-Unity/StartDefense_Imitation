# GameCamp 과제 프로젝트

이 프로젝트는 **게임캠프 과제**로 제작한 디펜스형 게임입니다.  
타일 위에 영웅을 소환하고 승급, 업그레이드 등이 있습니다.

웨이브 단위로 몰려오는 몬스터들을 처치하는 구조를 가지고 있습니다.

---

## 📆 제작 기간

- **기간**: 2025년 11월 19일 ~ 2025년 11월 26일 (총 7일)

---

## 📊 데이터 시트

게임 내 각종 데이터(영웅, 몬스터, 스테이지 등)는 구글 스프레드시트를 통해 관리됩니다.

- **구글 시트 주소**
    - https://docs.google.com/spreadsheets/d/10UynqUkjLib50vgNyNwcJ3E-EnGcA6ZNMpkDBr0bUn8/edit?usp=sharing

구글 시트 → JSON 변환 → `DataManager`에서 읽어오는 구조입니다.

---

## 🧱 전체 구조 개요

프로젝트는 **씬을 넘어가도 유지되는 싱글톤 GameManager**를 중심으로,  
여러 매니저들이 역할을 나누어 담당하는 구조입니다.

### GameManager & 글로벌 매니저

- **GameManager**
    - `SingletonDontDestroy<GameManager>`를 상속한 싱글톤
    - 씬이 전환되어도 파괴되지 않으며, 게임 전체에서 사용하는 매니저들을 생성 및 보관
    - 매니저들의 **초기화 순서**를 명시적으로 관리

- **DataManager**
    - 구글 시트에서 JSON으로 변환된 데이터를 로드
    - JSON 데이터를 `Dictionary` 형태로 보관하여 빠르게 조회
    - 플레이어의 기본 정보를 **저장 / 로드**하는 기능 포함

- **ResourceManager**
    - `주소(경로)를 key`, `로드된 에셋을 value`로 하는 `Dictionary` 관리
    - 동일 에셋의 중복 로드를 방지하여 **리소스 재활용** 및 최적화

- **SoundManager**
    - 현재 프로젝트에서는 **사용되지 않았습니다.**
    - 향후 BGM / SFX 등을 관리하기 위한 확장 포인트

- **InputManager**
    - Input System 기반
    - 마우스/터치 입력을 받아 이를 **이벤트(Action<Vector2> onClick)**로 브로드캐스트
    - 씬/오브젝트는 `GameManager.Input`을 통해 입력 이벤트를 구독

- **EventManager**
    - 전역 이벤트 버스 역할
    - 어디서나 이벤트를 발행(Publish)하고, 구독(Subscribe)할 수 있음
    - 규모가 커질 경우 이벤트 흐름이 복잡해질 수 있으므로 **사용 시 주의 필요**

- **UIManager**
    - 게임 내 **UI의 생성, 표시, 숨김, 재사용**을 관리
    - 특정 UI 타입(예: `UIGameHUD`, `UISummon`, `UIUpgrade` 등)을 열고 닫는 인터페이스 제공

- **SceneController**
    - 씬 전환을 관리하는 매니저
    - **비동기(Async) 로딩**을 사용하여 씬을 전환
    - 현재 씬, 이전 씬 정보 및 `curSceneManager`를 관리

---

## 🎮 씬 및 게임 진행 구조

### BaseSceneManager

- 각 씬마다 **반드시 하나의 SceneManager**가 존재하며, `BaseSceneManager`를 상속합니다.
- 공통 역할:
    - `SceneName curScene` 프로퍼티
    - `ObjectPoolManager objectPoolManager` 관리
    - `Init()`, `OnEnter()`, `OnExit()` 템플릿 제공
- `GameManager.Scene.curSceneManager` 를 통해 현재 씬 매니저에 접근할 수 있습니다.

### GameSceneManager

게임 플레이가 이루어지는 **GameScene 전용 매니저**입니다.

- 보유 컴포넌트 및 매니저
    - **Tilemap**: 타일 배치
    - **TileManager**: 타일 정보 및 경로 관리
    - **WaveManager**: 웨이브 및 몬스터 소환 관리
    - **StageManager**: 영웅, 골드, 미네랄 등 스테이지 상태 관리
    - **Commander**: 영웅 소환/지휘 담당

- 주요 역할
    - 게임 시작 시 필요한 매니저들 초기화
    - 입력 이벤트(onClick)를 구독하여 **타일 클릭 처리**
    - 게임 종료 이벤트(`GameEnd`)를 받아 결과 UI 표시 및 각 매니저에 전달

---

## 🧩 주요 서브 시스템

### TileManager

- **타일맵(Tilemap)**을 기반으로 타일 정보를 구성
- `TypedTile(ScriptableObject)`을 통해 각 타일에 **타입(TileType)**을 부여
    - 예: `Spawn`, `Goal`, `Road`, `Normal` 등
- 기능
    - `GetTileNode(Vector3 worldPos)` : 월드 좌표를 타일 노드로 변환
    - 스폰 위치, 목표 위치, 몬스터 이동 경로 관리
    - `ChangeTile(TileNode curTile, TileType type)` : 해당 타일을 다른 타일 노드로 변환

### WaveManager

- 웨이브(라운드) 정보를 관리
- 각 웨이브마다 몬스터 스폰 타이밍, 개수, 종류를 제어
- `TileManager`에서 제공하는 스폰 위치 / 경로를 기반으로 몬스터 소환

### StageManager

- 현재 스테이지(라운드)의 **진행 상태**를 관리
    - 소환된 영웅 목록
    - 플레이어 자원
        - 골드
        - 미네랄
        - 그 외 스테이지 관련 수치
- 영웅 소환, 승급, 초월 등의 요구 사항과 비용 관리

---

## 🧟 몬스터/타일/UI 설계

### 몬스터

- **상태 머신(FSM)** 구조를 사용하여 행동을 관리합니다.
    - 예: Idle, Move, Attack 등
- 각 상태는 명확한 책임을 가지며, 전이 조건에 따라 상태가 변경됩니다.

### 타일

- `Tilemap`의 `Tile`을 상속한 **ScriptableObject(`TypedTile`)**를 사용
- 게임에 맞는 타일 타입(`TileType`)을 부여하여:
    - 스폰 지점
    - 수리가 필요한 타일
    - 길(Road)
    - 영웅 배치 가능 타일(Normal)
      등을 구분합니다.

### UI

- **Atomic Design 패턴**을 참고하여 설계되었습니다.
    - 다만 현재는 **atom과 molecule의 경계가 명확하지 않은 부분**이 있어  
      추후 리팩터링이 필요한 상태입니다.
- `UIManager`를 통해 UI를 일관된 방식으로 생성/표시/숨김 처리합니다.

---

## 📁 외부 에셋

- 외부 에셋 파일은 **버전 관리 시스템에서 ignore 처리**되어 있습니다.
    - 따라서 프로젝트를 클론했을 때 일부 리소스는 포함되어 있지 않을 수 있습니다.
    - 사용한 에셋 : 2D Casual UI HD
    - https://assetstore.unity.com/packages/2d/gui/icons/2d-casual-ui-hd-82080

---

## 📝 비고

- 이 프로젝트는 **과제 목적**으로 제작되었습니다.
- 코드 구조, 매니저 설계, UI 구조(Atomic Design), 상태 머신 등은  
  이후 다른 프로젝트에서도 재사용/확장 가능하도록 설계하는 것을 목표로 합니다.
