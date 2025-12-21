## 🏗️ MAIN LAYOUT COMPONENT RULES (Angular Material + Best Practice)

### Purpose
- The main layout is the root structural component that wraps the entire application.
- It provides the shell containing:
  - A fixed top **header/toolbar** with branding and navigation controls
  - A collapsible side **menu/navigation drawer** with application links
  - A **content area** where routed components render via <router-outlet>

### Structure requirements
- Use Angular Material's sidenav container pattern:
  - <mat-toolbar> for the header
  - <mat-sidenav-container> as the layout wrapper
  - <mat-sidenav> for the navigation menu
  - <mat-sidenav-content> for the main content area with <router-outlet>

### Header/Toolbar rules
- Use <mat-toolbar color="primary"> fixed at the top.
- Include a menu toggle button that calls drawer.toggle():
  - <button mat-icon-button (click)="drawer.toggle()"><mat-icon>menu</mat-icon></button>
- Include a home link or branding element using outerLink="/".
- DO NOT add elements that don't exist in the component TS file.

### Navigation menu rules
- Use <mat-sidenav #drawer mode="side" opened> for the drawer.
- Inside the drawer, use <mat-nav-list> to organize navigation items.
- Render navigation items as specified in the main prompt.
- DO NOT invent routes or navigation items that are not specified.

### Content area rules
- The <mat-sidenav-content> must contain <router-outlet></router-outlet>.
- DO NOT add additional markup inside the content area beyond what's needed for layout structure.
- The router outlet is where all page components will render.

### Responsive behavior
- The drawer should support responsive toggling:
  - Use mode="side" with opened="true" for desktop (always visible).
  - Optionally use mode="over" for mobile (drawer overlays content).
  - The toggle button in the header controls drawer visibility.

### Styling guidelines
- The layout must occupy full viewport height (height: 100vh).
- The header should be fixed at the top with appropriate z-index.
- The sidenav container should account for header height (e.g., padding-top: 64px).
- Content area should be scrollable while header and drawer remain fixed.
- Use :host styles to control the component's root element behavior.

### Forbidden actions
- DO NOT add authentication logic, user menus, or profile dropdowns unless they exist in the TS file.
- DO NOT create navigation items for routes that don't exist.
- DO NOT modify or add service calls (the layout is purely structural).
- DO NOT add footer, breadcrumbs, or other UI elements unless specified in the TS file.
- DO NOT change the fundamental structure (toolbar + sidenav + content).

### Navigation items source of truth
- Navigation items are provided in the main prompt.
- If menu items are dynamically loaded from a service in the component's TS file:
  - Use the existing service method without modification.
  - Bind the menu items to the data returned by that service.
- DO NOT add, remove, or modify navigation items beyond what is provided.

### TypeScript component rules
- The component class should be minimal (usually just the decorator).
- Include RouterOutlet and RouterLink in the imports.
- Include all necessary Material modules:
  - MatToolbarModule
  - MatSidenavModule
  - MatButtonModule
  - MatIconModule
  - MatListModule
- DO NOT add properties or methods unless they are needed for specific functionality defined in the existing TS file.