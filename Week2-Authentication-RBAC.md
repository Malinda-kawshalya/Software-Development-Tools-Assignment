# Week 2 Implementation - Authentication and RBAC

## Completed Scope

### 1. Login Flow for All Roles
- Added seeded demo users for each required role:
  - Student: student1@blindmatch.local
  - Supervisor: supervisor1@blindmatch.local
  - Module Leader: moduleleader@blindmatch.local
  - System Administrator: sysadmin@blindmatch.local
- Shared demo password:
  - Pass#12345
- Login route remains ASP.NET Core Identity default:
  - /Identity/Account/Login
- Login links now default to return users to role routing at Dashboard.

### 2. Role-Protected Endpoints
- Added protected controllers and role-specific workspace pages:
  - StudentController [Student]
  - SupervisorController [Supervisor]
  - ModuleLeaderController [ModuleLeader]
  - SystemAdminController [SystemAdministrator]
- Added DashboardController [Authorize] that routes authenticated users to their correct role workspace.

### 3. Unauthorized Access Handling
- Configured application cookie paths:
  - LoginPath: /Identity/Account/Login
  - AccessDeniedPath: /Home/AccessDenied
- Added friendly unauthorized page:
  - /Home/AccessDenied
- Added status code re-execution and friendly status page:
  - /Home/HttpStatus?code={statusCode}

### 4. Modern Frontend Upgrade
- Reworked global layout and navigation with role-aware header.
- Added polished landing page with clear role entry points.
- Added modern typography, layered gradients, card-based UI, and responsive behavior.
- Updated role workspace pages with consistent visual style.

## Quick Manual Test Steps
1. Open app.
2. Click Open Student Portal from home.
3. Login as student1@blindmatch.local / Pass#12345.
4. Verify Student workspace opens.
5. Logout and login as supervisor1@blindmatch.local.
6. Try opening Student portal directly.
7. Verify redirected to Access Denied page.
8. Open My Workspace and verify Supervisor workspace opens.

## Security Note
- Replace demo passwords and demo account settings before production use.
