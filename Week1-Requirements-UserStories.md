# Week 1 Requirements and User Stories (Blind-Match PAS)

## 1. Finalized Requirements

### 1.1 Student
- Must securely log in.
- Must submit a project with Title, Abstract, Technical Stack, and Research Area.
- Must edit or withdraw proposal before matching.
- Must track status: Pending, Under Review, Matched.
- Must view supervisor identity only after confirmed match.

### 1.2 Supervisor
- Must securely log in.
- Must set expertise areas.
- Must view anonymous proposals filtered by expertise.
- Must express interest and confirm match.
- Must view student details only after confirmed match.

### 1.3 Module Leader
- Must securely log in.
- Must manage research areas.
- Must view all allocations.
- Must reassign when required.

### 1.4 System Administrator
- Must create/manage users.
- Must configure ASP.NET Core + SQL Server.
- Must manage EF Core migrations.
- Must enforce role-based access control.

## 2. User Stories and Acceptance Criteria

### Student Stories
1. As a student, I want to create a proposal so I can submit my project idea for supervision.
- Acceptance:
  - Given I am logged in as Student, when I complete valid fields and submit, then proposal is saved as Pending.

2. As a student, I want to update my proposal before matching so I can improve my submission.
- Acceptance:
  - Given proposal status is Pending or Under Review, when I edit and save, then changes are stored.
  - Given proposal is Matched, when I attempt edit, then action is blocked.

3. As a student, I want to withdraw my proposal before matching.
- Acceptance:
  - Given proposal is not Matched, when I withdraw, then proposal is marked Withdrawn.

### Supervisor Stories
4. As a supervisor, I want to view proposals without student identity so I can select fairly.
- Acceptance:
  - Given I am logged in as Supervisor, when viewing blind dashboard, then student name and ID are hidden.

5. As a supervisor, I want to filter proposals by expertise areas.
- Acceptance:
  - Given I selected expertise tags, when dashboard loads, then proposals are filtered by those tags.

6. As a supervisor, I want to confirm a match so I can supervise the project.
- Acceptance:
  - Given a valid project, when I confirm match, then proposal status updates to Matched.
  - Then student and supervisor identities become visible to each other.

### Module Leader Stories
7. As module leader, I want to manage research areas so category options remain valid.
- Acceptance:
  - Given leader role, when creating/updating/deleting area, then list reflects changes.

8. As module leader, I want an oversight dashboard of all allocations.
- Acceptance:
  - Given leader role, when opening dashboard, then all matched pairs are visible.

9. As module leader, I want to reassign a matched project if needed.
- Acceptance:
  - Given valid reassignment, when I submit, then new assignment is saved and logged.

### System Administrator Stories
10. As system administrator, I want to create role-based accounts so users access correct features.
- Acceptance:
  - Given admin role, when creating account with selected role, then account is created and role assigned.

11. As system administrator, I want database schema changes tracked so deployments are consistent.
- Acceptance:
  - Given model changes, when migration is generated and applied, then schema updates correctly.

12. As system administrator, I want unauthorized pages blocked by RBAC.
- Acceptance:
  - Given user role mismatch, when accessing restricted route, then access is denied.

## 3. Week 1 Deliverables Completed
- ASP.NET Core MVC solution initialized.
- SQL Server connection configured for DESKTOP-4LLHASP and Blind-Match PAS database.
- ASP.NET Core Identity enabled with role support.
- Startup role seeding for Student, Supervisor, ModuleLeader, SystemAdministrator.
- Default ModuleLeader account seed configured via appsettings.
