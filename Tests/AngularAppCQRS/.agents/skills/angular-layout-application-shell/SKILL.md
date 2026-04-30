---
name: angular-layout-application-shell
description: Creates the main layout/shell component for an Angular application using Angular Material sidenav pattern, providing a fixed header with navigation toggle, collapsible side menu, and content area with router outlet.
paths:
  - "**/*.component.ts"
  - "**/*.component.html"
  - "**/*.component.scss"
contentHash: ED0CABBED172CBE96DC580C1A523301629CA88F4B66B5DF486A02F833B22DBBC
---

MANDATORY FIRST STEP � READ SAMPLE FILES BEFORE ANY OTHER IMPLEMENTATION WORK

## Reference Examples

You MUST read all of the following skill asset files using the skill asset/file access mechanism before generating output. These sample files are in the same folder as this SKILL file:

1. **`main-layout-sample.ts`** � TypeScript structure template (imports, component metadata, minimal class implementation)
2. **`main-layout-sample.html`** � HTML layout template (toolbar, sidenav container, drawer, content area structure)
3. **`main-layout-sample.scss`** � Styling template (always read, even for minimal styling)

---

## CRITICAL: Preserve Existing Implementation & Structural Constraints

- **Use this skill for**: Main layout/shell components in Angular applications
- **Do NOT use for**: Page components, dialogs, or feature-specific layouts
- **Source of truth**: The existing component TypeScript file defines all methods, properties, and navigation behavior
- **Layout is purely structural**: Focus on shell structure, not application features

### You MUST NOT:
- Add authentication logic, user menus, or profile dropdowns (unless they exist in TS file)
- Create navigation items for routes that don't exist or aren't specified
- Modify or add service calls (layout is purely structural)
- Add footer, breadcrumbs, or other UI elements (unless specified in TS file or context)
- Change fundamental structure (toolbar + sidenav + content)
- Add properties or methods to component class (unless required by existing functionality)
- Modify existing navigation methods in the TypeScript file

---

## 1. Purpose & Structure Requirements

The main layout is the **root structural component** wrapping the entire application.

### Core structure (Angular Material sidenav pattern):
- **Header/Toolbar**: Fixed top bar with branding and navigation controls (`<mat-toolbar>`)
- **Side Menu/Navigation**: Collapsible drawer with application links (`<mat-sidenav>`)
- **Content Area**: Where routed components render (`<mat-sidenav-content>` with `<router-outlet>`)

### Layout requirements:
- Use `<mat-sidenav-container>` as the layout wrapper
- Layout must occupy full viewport height (`height: 100vh`)
- Content area scrollable while header and drawer remain fixed
- Header fixed at top with appropriate z-index
- Sidenav container accounts for header height (e.g., `padding-top: 64px`)

---

## 2. Header/Toolbar Implementation

Use `<mat-toolbar color="primary">` fixed at the top.

### Required elements:
- **Menu toggle button** that calls `drawer.toggle()`:
  ```html
  <button mat-icon-button (click)="drawer.toggle()">
    <mat-icon>menu</mat-icon>
  </button>
  ```
- **Home link/branding**:
  - If navigation method exists in TS (e.g., `navigateToHome()`): use `(click)="navigateToHome()"`
  - Otherwise: use `routerLink="/"`

**Do NOT** add elements not in component TS file (e.g., user menus, profile dropdowns, auth UI).

---

## 3. Navigation Menu Implementation

Use `<mat-sidenav #drawer mode="side" opened>` for the drawer.

### Structure:
- `#drawer` template reference variable is required for toggle button
- Inside drawer, use `<mat-nav-list>` to organize items
- Render navigation items per specification (see Navigation Items section below)

### Responsive behavior:
- **Desktop**: `mode="side"` with `opened="true"` (always visible)
- **Mobile** (optional): `mode="over"` (drawer overlays content)
- Toggle button controls drawer visibility
- If responsive behavior needed, use Angular CDK's `BreakpointObserver` (only if already in TS file)

**Do NOT** invent routes or navigation items.


---

## 4. Navigation Items Rendering

Navigation items are provided as input or specified in context.

### For each navigation item:

| Element | Implementation |
|---------|---------------|
| Container | `<mat-list-item>` or `<a mat-list-item>` |
| Icon | `<mat-icon matListItemIcon>iconName</mat-icon>` |
| Label | `<span matListItemTitle>Display Text</span>` |

### Binding rules:
- **If navigation method exists in TS** (e.g., `navigateToCustomers()`):
  - Use `(click)="navigationMethod()"`
- **If no navigation method exists**:
  - Use `routerLink="/path"` with `routerLinkActive="active"`

### Dynamic menu items:
- If items dynamically loaded from service in component TS:
  - Use existing service method without modification
  - Bind to data returned by that service

**Do NOT**:
- Add navigation items not specified
- Modify existing navigation methods
- Create items for non-existent routes

---

## 5. Content Area with Router Outlet

The `<mat-sidenav-content>` MUST contain `<router-outlet></router-outlet>`.

### Requirements:
- Content area is where all page components render
- Do NOT add additional markup beyond what's needed for layout structure
- Content should be scrollable while header/drawer remain fixed

---

## 6. TypeScript Component Configuration

The component class should be **minimal** (usually just decorator and imports).

### Required imports (standalone component):
```typescript
imports: [
  RouterOutlet,
  RouterLink,
  RouterLinkActive,
  MatToolbarModule,
  MatSidenavModule,
  MatButtonModule,
  MatIconModule,
  MatListModule
]
```

### Styling:
- Use `:host` styles to control root element behavior (e.g., `display: flex`, `height: 100vh`)

**Do NOT**:
- Add properties or methods unless needed for existing functionality
- Create navigation methods�they must already exist in TS file

---

## 7. Styling Requirements

- **Use global utilities first**: `.app-header`, `.app-sidebar`, `.app-content`, `.home-link`, `.pa-4`, `.mb-4`, etc.
- **Add component SCSS** for viewport-filling layout when global styles don't provide it
- **The generated layout must visually match the sample layout**
- **Add new utilities to `styles.scss`** only when reusable across multiple pages
- **NEVER modify** existing styles in `styles.scss` or `theme.scss`�only add new ones
- **Component SCSS required**: The main-layout sample relies on component SCSS for viewport-filling layout. Copy/adapt sample SCSS unless those exact styles exist globally. Do not leave component SCSS empty if template uses classes that aren't confirmed global.

---

## Definition of Done

### HTML structure:
- [ ] `<mat-toolbar>` used for header
- [ ] `<mat-sidenav-container>` wraps entire layout
- [ ] `<mat-sidenav #drawer>` used for navigation menu with proper template reference
- [ ] `<mat-sidenav-content>` contains `<router-outlet>`

### Header/Toolbar:
- [ ] Toolbar uses `color="primary"` and is fixed at top
- [ ] Menu toggle button exists with `(click)="drawer.toggle()"` and `<mat-icon>menu</mat-icon>`
- [ ] Home link/branding uses either existing navigation method or `routerLink="/"`
- [ ] No invented elements (user menus, auth UI) not in TS file

### Navigation menu:
- [ ] `<mat-sidenav #drawer mode="side" opened>` structure used
- [ ] `<mat-nav-list>` organizes navigation items
- [ ] All navigation items rendered from specified source (not invented)
- [ ] Each item includes `<mat-icon matListItemIcon>` and `<span matListItemTitle>`

### Navigation bindings:
- [ ] Items with existing navigation methods use `(click)="navigationMethod()"`
- [ ] Items without methods use `routerLink="/path"` and `routerLinkActive="active"`
- [ ] No navigation methods invented or modified

### Content area:
- [ ] `<mat-sidenav-content>` contains only `<router-outlet>` and minimal structural markup
- [ ] No additional components or features beyond what's specified

### TypeScript configuration:
- [ ] All required imports present (`RouterOutlet`, `RouterLink`, `RouterLinkActive`, Material modules)
- [ ] Component class minimal with no unnecessary properties/methods
- [ ] Material modules included: `MatToolbarModule`, `MatSidenavModule`, `MatButtonModule`, `MatIconModule`, `MatListModule`
- [ ] No properties or methods added unless they exist in original TS file

### Responsive behavior:
- [ ] Drawer toggle button controls visibility
- [ ] Desktop mode uses `mode="side"` with `opened="true"`
- [ ] Mobile mode (if implemented) uses `mode="over"`

### Layout styling:
- [ ] Layout occupies full viewport height (100vh)
- [ ] Header fixed at top with appropriate z-index
- [ ] Sidenav container accounts for header height
- [ ] Content area scrollable while header/drawer remain fixed
- [ ] `:host` styles control root element behavior

### Forbidden actions avoided:
- [ ] No auth logic, user menus, or profile dropdowns (unless in TS)
- [ ] No navigation items for non-existent routes
- [ ] No service calls added or modified
- [ ] No footer, breadcrumbs, or extra UI (unless specified)
- [ ] Fundamental structure (toolbar + sidenav + content) preserved

### Styling:
- [ ] Existing `styles.scss` and `theme.scss` not modified (only additions if needed)
- [ ] Component SCSS includes layout-specific styles (viewport height, positioning)
- [ ] Global utility classes used where appropriate
- [ ] Component SCSS adapted from sample when sample uses component styling

### Compilation:
- [ ] All referenced methods exist in component TS
- [ ] All template reference variables properly used (`#drawer`)
- [ ] Code compiles with all necessary imports