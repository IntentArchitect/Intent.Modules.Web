---
name: angular-layout-application-shell
description: Creates the main layout/shell component for an Angular application using Angular Material sidenav pattern, providing a fixed header with navigation toggle, collapsible side menu, and content area with router outlet.
paths:
	- "**/*.component.ts"
	- "**/*.component.html"
	- "**/*.component.scss"
---

## Reference Examples

Before generating any output, read the following files in the same folder as this skill:

1. **`./main-layout-sample.ts`** - MUST be read and used as the TypeScript structure template (imports, component metadata, minimal class implementation) and adapt for the target layout component.
2. **`./main-layout-sample.html`** - MUST be read and used as the HTML layout template. Copy its layout structure (toolbar, sidenav container, drawer, and content area) and adapt navigation items and branding.
3. **`./main-layout-sample.scss`** - MUST be read whenever implementing the component, even if you expect minimal styling.

---

### Styling Rules
- Use existing utility classes from `styles.scss` (e.g., `.app-header`, `.app-sidebar`, `.app-content`, `.home-link`, `.pa-4`, `.mb-4`, etc.)
- SCSS decision rule:
  - Use global utilities where possible.
  - If the sample uses component SCSS for structural layout (full viewport height, header positioning, sidenav container sizing) and global styles don't already guarantee it, copy/adapt the sample SCSS into the component SCSS.
  - Only add to styles.scss when the style is reusable across multiple pages; otherwise keep it component-scope
- If you need a new utility class or pattern that doesn't exist, you may add it to `styles.scss`
- NEVER modify existing styles in `styles.scss` or `theme.scss` - only add new ones if needed
- Copy/adapt sample SCSS into the component SCSS unless you can prove the same classes already exist globally.
- The generated layout must visually match the sample layout.
- SCSS Parity Rule (Required): The main-layout sample relies on component-scoped SCSS for viewport-filling layout. Therefore, when generating a layout, the agent must copy/adapt the sample SCSS into the component SCSS unless those exact styles already exist in styles.scss. The agent must not leave component SCSS empty if the template uses classes that aren't confirmed global.

### 1. Purpose and structure requirements
- The main layout is the **root structural component** that wraps the entire application.
- It provides the shell containing:
  - A fixed top **header/toolbar** with branding and navigation controls
  - A collapsible side **menu/navigation drawer** with application links
  - A **content area** where routed components render via `<router-outlet>`
- Use Angular Material's sidenav container pattern:
  - `<mat-toolbar>` for the header
  - `<mat-sidenav-container>` as the layout wrapper
  - `<mat-sidenav>` for the navigation menu
  - `<mat-sidenav-content>` for the main content area with `<router-outlet>`
- The layout must occupy full viewport height (`height: 100vh`).
- Content area should be scrollable while header and drawer remain fixed.

### 2. Header/Toolbar implementation
- Use `<mat-toolbar color="primary">` fixed at the top.
- Include a menu toggle button that calls `drawer.toggle()`:
  - Example: `<button mat-icon-button (click)="drawer.toggle()"><mat-icon>menu</mat-icon></button>`
- Include a home link or branding element.
  - If a navigation method exists (e.g., `navigateToHome()`), use `(click)="navigateToHome()"`
  - Otherwise, use `routerLink="/"`
- The header should be fixed at the top with appropriate z-index.
- DO NOT add elements that don't exist in the component TS file (e.g., user menus, profile dropdowns, authentication UI).

### 3. Navigation menu implementation
- Use `<mat-sidenav #drawer mode="side" opened>` for the drawer.
  - The `#drawer` template reference variable is required for the toggle button to work
- Inside the drawer, use `<mat-nav-list>` to organize navigation items.
- Render navigation items as specified (see section 7 below).
- The sidenav container should account for header height (e.g., padding-top: 64px).
- DO NOT invent routes or navigation items that are not specified.

### 4. Content area with router outlet
- The `<mat-sidenav-content>` must contain `<router-outlet></router-outlet>`.
- DO NOT add additional markup inside the content area beyond what's needed for layout structure.
- The router outlet is where all page components will render.
- Content should be scrollable while maintaining the fixed header and drawer.

### 5. Responsive behavior
- The drawer should support responsive toggling:
  - Use `mode="side"` with `opened="true"` for desktop (always visible).
  - Optionally use `mode="over"` for mobile (drawer overlays content) if responsive breakpoints are implemented.
  - The toggle button in the header controls drawer visibility.
- If responsive behavior is needed, use Angular CDK's `BreakpointObserver` (only if already present in the TS file).

### 6. TypeScript component configuration
- The component class should be minimal (usually just the decorator and imports).
- Include `RouterOutlet` and `RouterLink` in the imports array.
- Include all necessary Material modules:
  - `MatToolbarModule`
  - `MatSidenavModule`
  - `MatButtonModule`
  - `MatIconModule`
  - `MatListModule`
- Use `:host` styles to control the component's root element behavior (e.g., display: flex, height: 100vh).
- DO NOT add properties or methods unless they are needed for specific functionality defined in the existing TS file.
- If the component needs to call navigation methods, those methods must already exist in the TS file.

### 7. Navigation items rendering rules
- Navigation items are provided as input or specified in the main context.
- For each navigation item:
  - Render as a `<mat-list-item>` or `<a mat-list-item>`
  - Include `<mat-icon matListItemIcon>` with the appropriate icon
  - Include `<span matListItemTitle>` with the display text
  - If a navigation method exists in the TS file (e.g., `navigateToCustomers()`):
    - Use `(click)="navigationMethod()"`
  - If no navigation method exists:
    - Use `routerLink="/path"` with `routerLinkActive="active"`
- DO NOT add navigation items that are not specified.
- DO NOT modify existing navigation methods in the TypeScript file.
- If menu items are dynamically loaded from a service in the component's TS file:
  - Use the existing service method without modification.
  - Bind the menu items to the data returned by that service.

### 8. Forbidden actions
- DO NOT add authentication logic, user menus, or profile dropdowns unless they exist in the TS file.
- DO NOT create navigation items for routes that don't exist or aren't specified.
- DO NOT modify or add service calls (the layout is purely structural).
- DO NOT add footer, breadcrumbs, or other UI elements unless specified in the TS file or context.
- DO NOT change the fundamental structure (toolbar + sidenav + content).
- DO NOT add properties or methods to the component class unless required by existing functionality.

### 9. General constraints
- Use this skill for main layout/shell components in Angular applications.
- Do not use this skill for page components, dialogs, or feature-specific layouts.
- The layout should be minimal and focused purely on structure.
- All navigation items must come from the provided context or existing TS file.
- Treat the existing component TypeScript file as the source of truth for any methods or properties.

## Completion Checklist

- [] The HTML template follows the Angular Material sidenav pattern:
  - [] `<mat-toolbar>` is used for the header.
  - [] `<mat-sidenav-container>` wraps the entire layout.
  - [] `<mat-sidenav #drawer>` is used for the navigation menu with proper template reference.
  - [] `<mat-sidenav-content>` contains the `<router-outlet>`.
- [] Header/Toolbar is correctly implemented:
  - [] Toolbar uses `color="primary"` and is fixed at the top.
  - [] Menu toggle button exists with `(click)="drawer.toggle()"` and `<mat-icon>menu</mat-icon>`.
  - [] Home link/branding element uses either existing navigation method or `routerLink="/"`.
  - [] No invented elements (user menus, auth UI) that don't exist in the TS file.
- [] Navigation menu is correctly implemented:
  - [] `<mat-sidenav #drawer mode="side" opened>` structure is used.
  - [] `<mat-nav-list>` organizes navigation items.
  - [] All navigation items are rendered from specified source (not invented).
  - [] Each navigation item includes `<mat-icon matListItemIcon>` and `<span matListItemTitle>`.
- [] Navigation item bindings are correct:
  - [] Items with existing navigation methods use `(click)="navigationMethod()"`.
  - [] Items without navigation methods use `routerLink="/path"` and `routerLinkActive="active"`.
  - [] No navigation methods were invented or modified.
- [] Content area is properly configured:
  - [] `<mat-sidenav-content>` contains only `<router-outlet></router-outlet>` and minimal structural markup.
  - [] No additional components or features added beyond what's specified.
- [] TypeScript component is correctly configured:
  - [] All required imports are present (`RouterOutlet`, `RouterLink`, Material modules).
  - [] Component class is minimal with no unnecessary properties/methods.
  - [] Required Material modules included: `MatToolbarModule`, `MatSidenavModule`, `MatButtonModule`, `MatIconModule`, `MatListModule`.
  - [] No properties or methods added unless they exist in the original TS file.
- [] Responsive behavior (if applicable):
  - [] Drawer toggle button controls visibility.
  - [] Desktop mode uses `mode="side"` with `opened="true"`.
  - [] Mobile mode (if implemented) uses `mode="over"`.
- [] Layout styling is correct:
  - [] Layout occupies full viewport height (100vh).
  - [] Header is fixed at the top with appropriate z-index.
  - [] Sidenav container accounts for header height.
  - [] Content area is scrollable while header and drawer remain fixed.
  - [] `:host` styles control root element behavior.
- [] Forbidden actions were avoided:
  - [] No authentication logic, user menus, or profile dropdowns added (unless they exist in TS).
  - [] No navigation items created for routes that don't exist.
  - [] No service calls added or modified.
  - [] No footer, breadcrumbs, or extra UI elements added (unless specified).
  - [] Fundamental structure (toolbar + sidenav + content) preserved.
- [] Styling rules are respected:
  - [] Existing styles in `styles.scss` and `theme.scss` were not modified (only additive changes allowed if necessary).
  - [] Component SCSS includes layout-specific styles (viewport height, positioning, etc.).
  - [] Global utility classes are used where appropriate (`.app-header`, `.app-sidebar`, `.app-content`, etc.).
  - [] If the sample template uses component SCSS for layout, the target component SCSS includes an adapted version.
- [] All bindings between HTML and TS are correct:
  - [] All referenced methods exist in the component TS.
  - [] All template reference variables are properly used (`#drawer`).
  - [] Code compiles with all necessary imports.