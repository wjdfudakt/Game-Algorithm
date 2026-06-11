# 게임 알고리즘 포트폴리오 과제

---

## 플레이어 이동

Assets>Unity-Algorithm Assets>Player

PlayerMove
### 방향키 이동 (↑ ↓ ← →)
- 키보드 방향키를 사용하여 월드 좌표 기준 X, Z 축 이동을 수행한다.

PhysicsMove
### WASD 이동 및 점프 (Move & Jump)
- WASD 키를 이용하여 월드 좌표 기준 이동을 수행한다.
- 스페이스바를 통해 점프 기능을 구현하였다.
- Rigidbody를 사용하여 물리 기반 이동을 구현하였으며,
  가속, 감속, 관성, 충돌 등의 물리 효과가 적용된다.

Plane
### 비행기 조작 (Plane)
- 로컬 좌표계를 기준으로 이동 및 회전을 처리한다.
- WS: 가속 / 감속
- AD: 요잉(Yaw, 좌우 회전)
- ↑ ↓: 피치(Pitch, 상하 회전)
- ← →: 롤(Roll, 기체 기울기)

TankMovement
### 탱크 조작 (Tank)
- Plane과 유사한 구조를 가지며 로컬 좌표를 기반으로 이동한다.
- Y 좌표 이동은 사용하지 않는다.

---

## 몬스터 AI (감지 / 회전 / 상태 전환)

Assets>Unity-Algorithm Assets>Monster>PatrolMonster, MonsterAttack

PatrolMonster는 Patrol, Detect, Chase, Attack 상태를 가지며
거리와 시야각에 따라 상태가 전환된다.

### Patrol (순찰)
- 지정된 범위를 기준으로 플레이어를 감지한다.

### Detect (발견)
- 플레이어가 감지 범위 + 시야각 내에 있을 경우 진입한다.
- 일정 시간 동안 플레이어를 바라보며 정지 상태를 유지한다.

### Chase (추적)
- 플레이어 방향으로 이동하며 추적한다.
- 감지 타이머는 초기화된다.

### Attack (공격)
- 공격 범위 내 진입 시 공격 상태로 전환된다.
- 공격은 시도 거리, 공격 범위, 시전 시간, 쿨타임을 가진다.
- 시전 시간 동안 범위 내에 있을 경우 공격 성공,
  벗어나면 공격 실패로 처리된다.

---

## 자료구조 활용

Assets>Unity-Algorithm Assets>Monster>PatrolMonster

### List (순찰 경로 관리)

몬스터의 Patrol 경로를 List 자료구조로 관리하였다.

#### 사용 이유
1. 순찰 포인트는 상황에 따라 추가/삭제가 가능하므로 배열보다 List가 적합하다.
2. Index를 통해 순서대로 포인트를 쉽게 관리할 수 있다.
3. 마지막 포인트에 도착 후 카운트를 0으로 만드는 코드를 통해 순찰 경로를 루프 구조로 구성하기 용이하다.