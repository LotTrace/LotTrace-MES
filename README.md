#  LotTrace-MES

**C# ASP.NET Core & WPF 기반의 실시간 공정 로트(Lot) 추적 관리 시스템**

본 프로젝트는 제조 현장의 생산 흐름을 디지털화하고, 제품의 생성부터 출하까지의 전 과정을 추적(Traceability)하기 위해 설계된 미니 MES(Manufacturing Execution System) 솔루션입니다.

---

##  Architecture

본 프로젝트는 **Clean Architecture** 패턴을 준수하여 관심사를 분리하고 유지보수성을 극대화했습니다.

* **LotTrace.Api**: 외부 통신을 담당하는 ASP.NET Core Web API 계층 (RESTful API, Swagger)
* **LotTrace.Application**: 비즈니스 로직 및 서비스 인터페이스 계층 (Service Pattern)
* **LotTrace.Infrastructure**: DB 접근 및 외부 기술 구현 계층 (Entity Framework Core)
* **LotTrace.Domain**: 가장 핵심이 되는 엔티티 및 도메인 모델 정의 계층

---

## Key Features

- **Lot Issuance**: 표준 규칙에 따른 고유 로트 번호(Lot ID) 자동 채번 및 생성
- **Process Tracking**: 공정 이동(Move)에 따른 실시간 상태(Wait/Run/Hold/Finish) 관리
- **Traceability (Genealogy)**: Lot ID 기반의 생산 이력 및 부모-자식 관계 추적
- **Inventory Monitoring**: 현재 공정 내 재공(WIP, Work In Process) 수량 실시간 집계

---

## Tech Stack

### Backend
- **Framework**: .NET 10 / ASP.NET Core
- **ORM**: Entity Framework Core
- **Database**: MS SQL Server
- **Documentation**: Swagger

### Frontend
- **Framework**: WPF (Windows Presentation Foundation)
- **Pattern**: MVVM (Model-View-ViewModel)
- **Communication**: HttpClient (Asynchronous API Calls)

*WPF Repository : 

---

## 🚀 Getting Started

1. **Repository Clone**
   ```bash
   git clone [https://github.com/javadocq/LotTrace-MES.git](https://github.com/javadocq/LotTrace-MES.git)