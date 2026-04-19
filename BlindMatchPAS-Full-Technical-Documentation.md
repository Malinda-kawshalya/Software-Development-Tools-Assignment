# Blind-Match PAS: Full Technical Documentation

## 1. Document Purpose
This document provides a complete technical description of the Blind-Match Project Approval System (PAS), including:
- Technologies and versions used
- Solution architecture and design decisions
- Detailed feature/module implementation
- Database schema and migration approach
- Security and RBAC model
- Testing strategy, implementation, and evidence
- Build/run/test instructions

It is designed as a submission-ready technical reference for development and report documentation.

## 2. Project Summary
Blind-Match PAS is a secure web-based approval platform where:
- Students submit project proposals
- Supervisors review proposals anonymously
- Supervisors express interest and confirm matches
- Identities are revealed only after a confirmed match
- Module Leaders oversee allocations and can reassign
- System Administrators manage users and roles

Core business emphasis:
- Fairness via blind review
- Role isolation via RBAC
- Traceability via audit logging
- Safe state transitions for proposal lifecycle

## 3. Technology Stack

### 3.1 Runtime and Framework
- Backend framework: ASP.NET Core MVC
- Runtime target: .NET 9.0 (implemented)
- Identity: ASP.NET Core Identity with role support
- Data access: Entity Framework Core
- Database provider: SQL Server
- Migration tooling: EF Core Migrations
- UI layer: Razor Views + Bootstrap + jQuery validation

Note:
- Project plan lists ASP.NET Core 8, while implementation targets net9.0. This is visible in [BlindMatchPAS.Web/BlindMatchPAS.Web.csproj](BlindMatchPAS.Web/BlindMatchPAS.Web.csproj) and [BlindMatchPAS.Tests/BlindMatchPAS.Tests.csproj](BlindMatchPAS.Tests/BlindMatchPAS.Tests.csproj).

### 3.2 Key NuGet Packages
Web project ([BlindMatchPAS.Web/BlindMatchPAS.Web.csproj](BlindMatchPAS.Web/BlindMatchPAS.Web.csproj)):
- Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore 9.0.8
- Microsoft.AspNetCore.Identity.EntityFrameworkCore 9.0.8
- Microsoft.AspNetCore.Identity.UI 9.0.8
- Microsoft.EntityFrameworkCore.SqlServer 9.0.8
- Microsoft.EntityFrameworkCore.Tools 9.0.8

Test project ([BlindMatchPAS.Tests/BlindMatchPAS.Tests.csproj](BlindMatchPAS.Tests/BlindMatchPAS.Tests.csproj)):
- xUnit 2.9.2
- xunit.runner.visualstudio 2.8.2
- Microsoft.NET.Test.Sdk 17.12.0
- Moq 4.20.72
- Microsoft.AspNetCore.Mvc.Testing 9.0.4
- Microsoft.EntityFrameworkCore.InMemory 9.0.4
- Microsoft.EntityFrameworkCore.Sqlite 9.0.4
- coverlet.collector 6.0.2

## 4. Solution Structure
- [BlindMatchPAS.sln](BlindMatchPAS.sln): solution root
- [BlindMatchPAS.Web](BlindMatchPAS.Web): ASP.NET Core MVC application
- [BlindMatchPAS.Tests](BlindMatchPAS.Tests): automated test project
- [Project-Plan.md](Project-Plan.md): planning baseline
- [TestEvidence](TestEvidence): test artifacts and evidence

## 5. High-Level Architecture
The system uses a layered MVC architecture:

1. Presentation Layer
- Razor Views under [BlindMatchPAS.Web/Views](BlindMatchPAS.Web/Views)
- Role-specific pages and forms

2. Application Layer
- Controllers orchestrate use cases and enforce role-based access:
  - [BlindMatchPAS.Web/Controllers/StudentController.cs](BlindMatchPAS.Web/Controllers/StudentController.cs)
  - [BlindMatchPAS.Web/Controllers/SupervisorController.cs](BlindMatchPAS.Web/Controllers/SupervisorController.cs)
  - [BlindMatchPAS.Web/Controllers/ModuleLeaderController.cs](BlindMatchPAS.Web/Controllers/ModuleLeaderController.cs)
  - [BlindMatchPAS.Web/Controllers/SystemAdminController.cs](BlindMatchPAS.Web/Controllers/SystemAdminController.cs)
  - [BlindMatchPAS.Web/Controllers/DashboardController.cs](BlindMatchPAS.Web/Controllers/DashboardController.cs)
  - [BlindMatchPAS.Web/Controllers/HomeController.cs](BlindMatchPAS.Web/Controllers/HomeController.cs)

3. Domain/Data Layer
- Entity models in [BlindMatchPAS.Web/Models](BlindMatchPAS.Web/Models)
- EF Core DbContext in [BlindMatchPAS.Web/Data/ApplicationDbContext.cs](BlindMatchPAS.Web/Data/ApplicationDbContext.cs)
- Migrations in [BlindMatchPAS.Web/Data/Migrations](BlindMatchPAS.Web/Data/Migrations)

4. Identity and Security Layer
- ASP.NET Core Identity + Roles configured in [BlindMatchPAS.Web/Program.cs](BlindMatchPAS.Web/Program.cs)
- Role/data seeding in [BlindMatchPAS.Web/Seeding/IdentitySeeder.cs](BlindMatchPAS.Web/Seeding/IdentitySeeder.cs)

## 6. Request Pipeline and Startup Configuration
Defined in [BlindMatchPAS.Web/Program.cs](BlindMatchPAS.Web/Program.cs).

### 6.1 Service Registration
- SQL Server DbContext registration with DefaultConnection
- Identity with roles and EF-backed stores
- Cookie paths:
  - LoginPath: /Identity/Account/Login
  - AccessDeniedPath: /Home/AccessDenied
- MVC controllers and views

### 6.2 Startup Behavior
- Role/user seeding at application startup via IdentitySeeder
- Development mode: migration end-point enabled
- Production mode: exception handler + HSTS

### 6.3 Middleware Pipeline
- HTTPS redirection
- Routing
- Status code re-execution to /Home/HttpStatus
- Authentication
- Authorization
- Static assets mapping
- MVC route mapping
- Razor Pages mapping (Identity UI)

## 7. Authentication and Authorization (RBAC)

### 7.1 Roles
The platform uses four roles:
- Student
- Supervisor
- ModuleLeader
- SystemAdministrator

Seeded in [BlindMatchPAS.Web/Seeding/IdentitySeeder.cs](BlindMatchPAS.Web/Seeding/IdentitySeeder.cs).

### 7.2 Role Protection by Controller
- Student features: [StudentController](BlindMatchPAS.Web/Controllers/StudentController.cs) with [Authorize(Roles = "Student")]
- Supervisor features: [SupervisorController](BlindMatchPAS.Web/Controllers/SupervisorController.cs) with [Authorize(Roles = "Supervisor")]
- Module leader features: [ModuleLeaderController](BlindMatchPAS.Web/Controllers/ModuleLeaderController.cs) with [Authorize(Roles = "ModuleLeader")]
- Admin features: [SystemAdminController](BlindMatchPAS.Web/Controllers/SystemAdminController.cs) with [Authorize(Roles = "SystemAdministrator")]
- Shared workspace routing: [DashboardController](BlindMatchPAS.Web/Controllers/DashboardController.cs) with [Authorize]

### 7.3 Unauthorized Handling
- Access denied route: /Home/AccessDenied
- Status code page route: /Home/HttpStatus?code={status}
- Defined in [BlindMatchPAS.Web/Program.cs](BlindMatchPAS.Web/Program.cs) and [BlindMatchPAS.Web/Controllers/HomeController.cs](BlindMatchPAS.Web/Controllers/HomeController.cs)

## 8. Functional Implementation by Module

## 8.1 Student Module
Implementation: [BlindMatchPAS.Web/Controllers/StudentController.cs](BlindMatchPAS.Web/Controllers/StudentController.cs)

Capabilities:
- View own proposals (Index)
- View proposal details and match/reveal status (Details)
- Create proposal (Create GET/POST)
- Edit proposal (Edit GET/POST)
- Withdraw proposal (Withdraw POST)

Important rules enforced:
- Students only access own proposals via user ID filtering
- Edit/withdraw allowed only for Pending and UnderReview statuses
- Matched or Withdrawn proposals cannot be edited/withdrawn
- Technical stack input is parsed into normalized tag records in ProposalTechStack
- All major actions are audit-logged

Validation model:
- [BlindMatchPAS.Web/Models/StudentProposalInputModel.cs](BlindMatchPAS.Web/Models/StudentProposalInputModel.cs)

## 8.2 Supervisor Module
Implementation: [BlindMatchPAS.Web/Controllers/SupervisorController.cs](BlindMatchPAS.Web/Controllers/SupervisorController.cs)

Capabilities:
- Blind proposal dashboard with filtering
- Expertise management (add/remove)
- View proposal details without student identity pre-match
- Express interest
- Confirm match
- View full match details post-confirmation

Business logic highlights:
- Pending proposal becomes UnderReview when interest is expressed
- Confirm match requires prior interest
- Confirm match blocked for withdrawn or already matched proposals
- Confirm match executes in DB transaction
- MatchAssignment creation triggers identity reveal path
- Audit logging for key supervisor actions

## 8.3 Module Leader Module
Implementation: [BlindMatchPAS.Web/Controllers/ModuleLeaderController.cs](BlindMatchPAS.Web/Controllers/ModuleLeaderController.cs)

Capabilities:
- Dashboard metrics (areas, matches, pending)
- Research area CRUD
- Allocation overview
- Reassign matched projects to another supervisor

Business controls:
- Duplicate research areas blocked (case-insensitive)
- Reassignment validates selected account is a Supervisor role
- Reassignment and CRUD actions are audit-logged

## 8.4 System Administrator Module
Implementation: [BlindMatchPAS.Web/Controllers/SystemAdminController.cs](BlindMatchPAS.Web/Controllers/SystemAdminController.cs)

Capabilities:
- View all users and role summary
- Create users and assign role
- Change user role
- Delete users

Data consistency behaviors:
- Auto-create StudentProfile/SupervisorProfile when assigning those roles
- Remove profile data and actor logs on user deletion
- Audit-log admin actions

## 9. Business Process Flows

## 9.1 Proposal Lifecycle
Status enum in [BlindMatchPAS.Web/Models/ProposalStatus.cs](BlindMatchPAS.Web/Models/ProposalStatus.cs):
- Pending
- UnderReview
- Matched
- Withdrawn

Typical transition path:
1. Student creates proposal: Pending
2. Supervisor expresses interest: UnderReview
3. Supervisor confirms match: Matched
4. Student can withdraw before match: Withdrawn

Guardrails:
- Edit/withdraw blocked after Matched or Withdrawn
- Confirm match blocked without prior interest

## 9.2 Blind Matching and Identity Reveal
Before confirmation:
- Supervisor views anonymous proposal content only
- Student identity is hidden

After confirmation:
- MatchAssignment is created
- Student details view returns both student/supervisor identity fields
- Supervisor match details view includes both identities and contact fields

## 10. Database Design
DbContext: [BlindMatchPAS.Web/Data/ApplicationDbContext.cs](BlindMatchPAS.Web/Data/ApplicationDbContext.cs)

## 10.1 Core Entity Sets
- ProjectProposals
- ResearchAreas
- StudentProfiles
- SupervisorProfiles
- ProposalTechStacks
- SupervisorExpertise
- SupervisorInterests
- MatchAssignments
- AuditLogs

## 10.2 Key Entities and Purpose

1. ProjectProposal ([ProjectProposal.cs](BlindMatchPAS.Web/Models/ProjectProposal.cs))
- Stores core proposal data and lifecycle status
- Linked to student via StudentUserId

2. StudentProfile ([StudentProfile.cs](BlindMatchPAS.Web/Models/StudentProfile.cs))
- Extended profile for student role users

3. SupervisorProfile ([SupervisorProfile.cs](BlindMatchPAS.Web/Models/SupervisorProfile.cs))
- Extended profile for supervisor role users

4. ResearchArea ([ResearchArea.cs](BlindMatchPAS.Web/Models/ResearchArea.cs))
- Canonical research taxonomy managed by module leader

5. ProposalTechStack ([ProposalTechStack.cs](BlindMatchPAS.Web/Models/ProposalTechStack.cs))
- Normalized proposal technologies/tags

6. SupervisorExpertise ([SupervisorExpertise.cs](BlindMatchPAS.Web/Models/SupervisorExpertise.cs))
- Supervisor-declared research strengths

7. SupervisorInterest ([SupervisorInterest.cs](BlindMatchPAS.Web/Models/SupervisorInterest.cs))
- Tracks interest expression events by supervisor and proposal

8. MatchAssignment ([MatchAssignment.cs](BlindMatchPAS.Web/Models/MatchAssignment.cs))
- Finalized match binding proposal, student, supervisor

9. AuditLog ([AuditLog.cs](BlindMatchPAS.Web/Models/AuditLog.cs))
- Cross-module trace log for key actions

## 10.3 Important Constraints and Indexes
Implemented in OnModelCreating:
- Unique ResearchArea.Name
- Unique StudentProfile.UserId
- Unique SupervisorProfile.UserId
- Unique ProposalTechStack(ProjectProposalId, Name)
- Unique SupervisorExpertise(SupervisorUserId, ResearchArea)
- Unique SupervisorInterest(SupervisorUserId, ProjectProposalId)
- Unique MatchAssignment.ProjectProposalId
- Indexed AuditLog.CreatedAtUtc
- Indexed ProjectProposal(StudentUserId, Status)

## 10.4 Migrations
Migration files are in [BlindMatchPAS.Web/Data/Migrations](BlindMatchPAS.Web/Data/Migrations) and include:
- Initial schema
- Student proposal module
- Supervisor expertise and blind review
- Matching and identity reveal
- Research area management
- Missing core entities and audit

Snapshot file:
- [BlindMatchPAS.Web/Data/Migrations/ApplicationDbContextModelSnapshot.cs](BlindMatchPAS.Web/Data/Migrations/ApplicationDbContextModelSnapshot.cs)

## 11. Configuration and Environment
Configuration file: [BlindMatchPAS.Web/appsettings.json](BlindMatchPAS.Web/appsettings.json)

Includes:
- SQL Server connection string (DefaultConnection)
- Seed user emails for all roles
- Shared seed password for demo
- Logging levels

Important security note:
- Demo credentials in appsettings are suitable for local dev/testing only and should be replaced for production.

## 12. Testing Strategy and Implementation
Test project: [BlindMatchPAS.Tests](BlindMatchPAS.Tests)

Testing categories mapped to project plan:

## 12.1 Unit Testing
- Matching logic
- Proposal state transitions
- Reveal trigger logic

Implemented in:
- [BlindMatchPAS.Tests/MatchingLogicIntegrationTests.cs](BlindMatchPAS.Tests/MatchingLogicIntegrationTests.cs)
- [BlindMatchPAS.Tests/StudentRevealTriggerTests.cs](BlindMatchPAS.Tests/StudentRevealTriggerTests.cs)

## 12.2 Integration Testing
- RBAC checks
- End-to-end feature flow checks

Implemented in:
- [BlindMatchPAS.Tests/SecurityIntegrationTests.cs](BlindMatchPAS.Tests/SecurityIntegrationTests.cs)
- [BlindMatchPAS.Tests/MatchingLogicIntegrationTests.cs](BlindMatchPAS.Tests/MatchingLogicIntegrationTests.cs)

## 12.3 Validation Testing
- Required field validation
- Invalid transition (edit after matched)

Implemented in:
- [BlindMatchPAS.Tests/StudentValidationTests.cs](BlindMatchPAS.Tests/StudentValidationTests.cs)

## 12.4 Security Testing
- Unauthorized route access behavior
- Authentication/session role behavior

Implemented in:
- [BlindMatchPAS.Tests/SecurityIntegrationTests.cs](BlindMatchPAS.Tests/SecurityIntegrationTests.cs)

## 12.5 Test Utilities
Shared helpers in [BlindMatchPAS.Tests/TestUtilities.cs](BlindMatchPAS.Tests/TestUtilities.cs):
- InMemory and SQLite context creation
- UserManager mocking
- Test user/claims attachment to controllers
- Valid model builders

## 13. Test Evidence and Results
Evidence files:
- Test case matrix: [TestEvidence/Test-Cases.md](TestEvidence/Test-Cases.md)
- Evidence checklist: [TestEvidence/Evidence-Checklist.md](TestEvidence/Evidence-Checklist.md)
- TRX execution report: [TestEvidence/Reports/BlindMatchPAS.Tests.trx](TestEvidence/Reports/BlindMatchPAS.Tests.trx)

Latest recorded automated result:
- Total tests: 10
- Passed: 10
- Failed: 0
- Skipped: 0

Coverage note:
- Automated logic and controller behavior are tested.
- UI Razor files are not directly unit-tested, which is standard for this test scope.

## 14. Build, Run, and Test Instructions
Run from repository root.

### 14.1 Restore
```powershell
dotnet restore
```

### 14.2 Build
```powershell
dotnet build BlindMatchPAS.sln
```

### 14.3 Run Web App
```powershell
dotnet run --project BlindMatchPAS.Web/BlindMatchPAS.Web.csproj
```

### 14.4 Run Tests
```powershell
dotnet test BlindMatchPAS.Tests/BlindMatchPAS.Tests.csproj
```

### 14.5 Run Tests with TRX Report
```powershell
dotnet test BlindMatchPAS.Tests/BlindMatchPAS.Tests.csproj --logger "trx;LogFileName=BlindMatchPAS.Tests.trx" --results-directory TestEvidence/Reports
```

### 14.6 Run Tests with Coverage
```powershell
dotnet test BlindMatchPAS.Tests/BlindMatchPAS.Tests.csproj --collect:"XPlat Code Coverage"
```

## 15. Architectural Strengths and Design Rationale
1. Strong role isolation
- Controller-level role gates reduce accidental cross-role access.

2. Enforced workflow integrity
- Explicit proposal status transitions prevent invalid operations.

3. Fair review model
- Supervisor pre-match view keeps student identity hidden.

4. Auditability
- Cross-module audit records provide operational traceability.

5. Transaction-safe matching
- Confirm match uses DB transaction to protect consistency.

6. Test-backed delivery
- Unit, integration, validation, and security concerns are all automated.

## 16. Known Gaps / Improvement Opportunities
1. Service layer extraction
- Move heavy controller logic into domain/application services for cleaner separation.

2. Additional test depth
- Add tests for module leader reassignment edge cases and admin lifecycle edge cases.

3. API compatibility layer
- If future mobile/API clients are needed, expose selected flows through secured APIs.

4. Production hardening
- Replace demo credentials and move secrets to environment variables/secret stores.
- Add centralized exception logging and monitoring pipeline.

## 17. Conclusion
Blind-Match PAS has been implemented as a secure, role-based MVC platform with:
- Complete core functional modules
- Structured domain model and migration history
- Enforced workflow and identity reveal rules
- Audited administrative and business actions
- Automated tests aligned to the project testing strategy

This documentation can be used as the primary technical chapter source for the final coursework report.
