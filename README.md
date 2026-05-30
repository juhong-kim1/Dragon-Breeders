# Dragon Breeders

드래곤을 육성하고 전투시키는 방치형 모바일 시뮬레이션 게임입니다.  
Google Play 출시작 / 개발 1인, 기획 2인 프로젝트

---

## 핵심 기능

### 드래곤 관리 시스템
- 배고픔, 청결, 친밀도, 스태미나를 시간 기반으로 갱신
- 성장 단계에 따라 스탯 데이터를 재적용하는 육성 루프 구조
- 상태 간 상호작용을 통한 자연스러운 방치형 플레이 설계

### 데이터 드리븐 설계
- 성장, 전투, 아이템, 디버프 등 게임 수치 전반을 CSV 기반 테이블로 분리
- 코드 수정 없이 데이터만으로 밸런스 조정 가능한 구조

### 저장 시스템
- JSON 직렬화를 활용한 게임 상태 저장 / 로드
- 버전 비교 및 VersionUp 로직으로 데이터 구조 변경 시 기존 저장 데이터 유지

---

## 기술 스택

- **Engine** : Unity
- **Language** : C#
- **Data** : CSV (DataTable 기반)
- **Save** : JSON 직렬화

---

## 플레이스토어

[Google Play에서 보기](https://play.google.com/store/apps/details?id=com.juhong-kim1.Dragon-Breeders)
