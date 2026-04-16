# Blind-Match PAS Project Plan (PUSL2020 Coursework)

## 1. Project Overview
This document presents the complete development plan for the Secure Web-Based Project Approval System (PAS) with Blind Matching.

### Goal
Develop a secure ASP.NET Core web application where:
- Students submit project proposals.
- Supervisors review proposals anonymously.
- Matching is based on technical fit and research alignment.
- Identities are revealed only after supervisor confirmation.

## 2. Technology Stack
- Backend Framework: ASP.NET Core 8 (MVC)
- Authentication and Security: ASP.NET Core Identity + Role-Based Access Control (RBAC)
- Database: SQL Server
- ORM and Schema Management: Entity Framework Core + Migrations
- Frontend: Razor Views + Bootstrap 5
- Testing: xUnit (unit and integration testing)
- Version Control: Git + GitHub

## 3. Team Roles and Responsibilities
Each member must implement at least one functionality.

- Member 1: Project Lead and Architecture
  - Solution structure
  - CI workflow and code quality standards
- Member 2: Student Features
  - Proposal submission and management
  - Status tracking
- Member 3: Supervisor Features
  - Expertise selection
  - Blind review dashboard
- Member 4: Module Leader Features
  - Research area management
  - Allocation oversight and reassignment
- Member 5: System Administrator Features
  - User account management
  - Infrastructure and migration setup
- Member 6: QA and Testing Lead
  - Automated test suite
  - Test report and evidence collection

## 4. Functional Requirements Mapping

### 4.1 Student
- Secure login
- Create proposal with Title, Abstract, Technical Stack, Research Area
- View, edit, withdraw proposal before matching
- Track project status: Pending, Under Review, Matched
- View supervisor details after confirmation

### 4.2 Supervisor
- Secure login
- Manage preferred research areas/tags
- Browse filtered project list based on expertise
- View project details anonymously (no student identity)
- Express interest and confirm match
- View student details after confirmation

### 4.3 Module Leader
- Secure login
- Create and manage research areas/tags
- View full allocation dashboard
- Reassign or intervene in project allocations

### 4.4 System Administrator
- Manage student and supervisor user accounts
- Configure ASP.NET Core environment and SQL Server connection
- Implement and maintain EF Core migrations
- Enforce RBAC restrictions across all dashboards

## 5. System Design

### Core Entities
- ApplicationUser (Identity)
- Role
- StudentProfile
- SupervisorProfile
- ResearchArea
- ProjectProposal
- ProposalTechStack
- SupervisorInterest
- MatchAssignment
- AuditLog

### Key Business Rules
- A proposal can be edited or withdrawn only before matching.
- Supervisor dashboard must hide student identity until match confirmation.
- Identity reveal triggers only when match is confirmed.
- Module Leader can override assignments.
- All role-specific routes must be protected with RBAC.

## 6. Development Plan (8 Weeks)

### Week 1: Planning and Setup
- Finalize requirements and user stories
- Create solution and project structure
- Setup SQL Server connection
- Configure Identity and role seeding

### Week 2: Authentication and RBAC
- Implement login flow for all roles
- Protect role-specific pages and endpoints
- Add unauthorized access handling

### Week 3: Student Module
- Proposal create/view/edit/withdraw
- Validation for required fields
- Status management (Pending/Under Review/Matched)

### Week 4: Supervisor Module
- Expertise/tags management
- Blind review dashboard (anonymous proposal view)
- Filter proposals by research area

### Week 5: Matching and Identity Reveal
- Express interest workflow
- Confirm match transaction logic
- Automatic identity reveal to both parties after confirmation

### Week 6: Module Leader + Admin Module
- Research area CRUD
- Allocation oversight dashboard
- Manual reassignment feature
- Admin user management screens

### Week 7: Testing and Hardening
- Unit tests for service/business logic
- Integration tests for role authorization and major flows
- Security checks and bug fixing

### Week 8: Finalization and Submission
- Prepare final report
- Capture screenshots and evidence
- Verify migration scripts and reproducibility
- Final QA and demo preparation

## 7. Testing Strategy

### Testing Types
- Unit Testing
  - Matching logic
  - Proposal state transitions
  - Reveal trigger logic
- Integration Testing
  - Role-based access control checks
  - End-to-end feature flow checks
- Validation Testing
  - Required field validation
  - Invalid transitions (e.g., edit after matched)
- Security Testing
  - Unauthorized route access attempts
  - Session/authentication behavior

### Test Evidence
- Test case table (ID, objective, steps, expected, actual)
- Screenshots of pass/fail outcomes
- xUnit test execution reports

## 8. Git and Project Management Standards
- Branching strategy:
  - main: stable releases
  - develop: integration branch
  - feature/*: individual tasks
- Commit message format:
  - feat: add proposal withdrawal
  - fix: protect supervisor route with role attribute
  - test: add integration tests for login access
- Pull request review before merge
- Commit history must show progressive development from initialization to final product

## 9. Deliverables Checklist

### Deliverable 1: Development
- Complete ASP.NET Core source code
- Professional Git history
- EF Core migration files and database schema generation evidence

### Deliverable 2: Report
- Testing strategy explanation
- Testing implementation details
- Requirement-to-feature mapping
- Security and RBAC approach
- Screenshots/evidence for major functional flows

## 10. Risk Management
- Risk: Scope overload
  - Mitigation: Freeze features by Week 6 and focus on testing
- Risk: Merge conflicts
  - Mitigation: Frequent pull and small PRs
- Risk: RBAC errors
  - Mitigation: Add dedicated integration tests for role access
- Risk: Migration issues
  - Mitigation: Validate migration apply/reset in a clean environment

## 11. Definition of Done
A feature is complete only if:
- Functional behavior matches requirement
- Input validation is implemented
- Authorization is enforced
- Automated tests are added/updated
- Code is reviewed and merged
- Feature is documented in report notes

## 12. Next Action Plan
1. Initialize ASP.NET Core MVC project and Identity setup.
2. Create initial database models and first migration.
3. Implement authentication and RBAC for all four roles.
4. Build Student proposal management module.
5. Build Supervisor blind review and matching module.
6. Build Module Leader and Admin modules.
7. Complete testing suite and prepare final report.
