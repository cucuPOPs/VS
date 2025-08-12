# 소개
뱀서라이크 "탕탕특공대" 모바일게임의 모작을 진행중입니다.  
2025.07.28 ~ 현재진행중

<img width="373" height="659" alt="모작1" src="https://github.com/user-attachments/assets/f75a70fc-635b-4c56-bf40-b6719132712e" />

# 플레이 영상 
https://github.com/user-attachments/assets/b70f840a-77ea-43b0-ad39-ca7ca3806811

---
## 핵심 시스템: 중앙 집중형 Manager System

**싱글톤(Singleton) + 서비스 로케이터(Service Locator)** 패턴 기반의 **`Managers`** 클래스가 중심입니다.

* **구조**

  * `UIManager`, `SceneManagerEx`, `ObjectManager`, `ResourceManager` 등 핵심 기능을 매니저 단위로 분리
  * `Managers` 클래스가 각 매니저에 대한 전역 접근점 제공 (static property)
* **장점**

  * **디커플링**: 시스템 간 직접 의존성 제거 → 코드 유연성 & 재사용성 향상
  * **중앙 관리**: 모든 매니저의 초기화/정리를 `Managers`에서 일괄 관리 → 씬 전환·게임 재시작 안정성 확보

```C#
public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }

    // 각 매니저에 대한 정적 프로퍼티 제공
    public static TableDataManager Table => Instance?._table;
    public static UIManager UI => Instance?._ui;
    public static SceneManagerEx Scene => Instance?._scene;
    public static ResourceManager Resource => Instance?._res;
    public static PoolManager Pool => Instance?._pool;
    public static ObjectManager Object => Instance?._obj;
    
    // ... 이하 생략
}
```
---

### 1. 데이터 기반 설계 (Data-Driven Design)

* **데이터 파이프라인**

  1. 기획자가 CSV 파일로 밸런스 데이터 작성 (스킬, 몬스터 스펙 등)
  2. 게임이 시작되면, `CSVReader`가  CSV파일을 파싱
  3. `TableDataManager`가 Dictionary\<int, SkillData> 등으로 변환 후 메모리 적재
* **효과**: 코드 수정 없이 데이터 변경만으로 게임 밸런스 조정 가능

---

### 2. 리소스 & 오브젝트 관리 최적화

* **오브젝트 풀링 (`PoolManager`)**: 총알, 몬스터, 아이템 등 재활용 → `Instantiate` / `Destroy` 최소화, GC 부담 감소
* **리소스 캐싱 (`ResourceManager`)**: `Resources.Load` 결과 캐싱 → 디스크 I/O 감소, 로딩 속도 향상
* **중앙 관리 (`ObjectManager`)**: 생성/소멸 전담 → 풀 우선 사용, 필요 시 새 생성

---

### 3. 확장 가능한 UI 시스템

* **UI\_Base & 자동 바인딩**: enum + 제네릭 `Bind<T>`로 UI 컴포넌트 자동 연결
* **팝업 관리**: Stack 자료구조로 순서·레이어 제어
* **이벤트 처리**: `UI_EventHandler` + 확장 메서드 `BindEvent` → 클릭·드래그 이벤트 간결 등록

---

### 4. 공간 분할 기반 충돌 감지 최적화 (Spatial Partitioning)

* **`GridManager`**: 게임 월드를 셀 단위로 분할
* **원리**: 충돌 검사 시 현재 셀 + 주변 8셀만 탐색 → 탐색 범위 대폭 축소
* **효과**: 수백 개의 오브젝트가 동시에 등장해도 안정적 프레임 유지

---

### 5. 컴포넌트 기반 스킬 시스템

* **구조**: `SkillBase` 상속 → 스킬 개별 컴포넌트(FireBallSkill 등) 구현
* **추상 클래스**: `RepeatSkill`, `SequenceSkill`로 발동 방식 분리
* **확장성**: 새 스킬 추가 시 `DoSkill`만 구현하면 됨
* **데이터 연동**: 스킬 데이터(레벨, 데미지, 쿨타임 등)는 `TableDataManager`에서 자동 적용

---


