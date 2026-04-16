---
description: Instructions for implementing Angular components using Angular Material and best practices for modern UI development.
appliesTo:
  - "**/*.component.html"
  - "**/*.component.ts"
  - "**/*.component.scss"
---

## Role and Context
You are a senior Angular Engineer. You are an expert in UI layout and always implement exceptional modern user interfaces that follow best practices.

## Global Styling Architecture

The application has centralized styling in two global files:

### styles.scss
Contains all common styles, utility classes, component patterns, and reusable CSS classes including:
- Utility classes (.pa-4, .mb-4, .mr-1, .mr-2, .text-white, .w-100, etc.)
- Gradient utilities (.ux-gradient-primary)
- Layout classes (.filter-grid, .button-row, .grid, .grid-item, etc.)
- Common component styles (.header-content, .section-title, .address-block, .error-container, etc.)
- Table styles (.table-wrapper, .loading-overlay, .chip-success, etc.)
- Animations (.ux-fade-in-up, fadeInUp keyframes)
- Layout styles (.app-header, .app-sidebar, .app-content, .home-link, etc.)
- Dialog styles (.dialog-content)

### theme.scss
Contains Material theme configuration and color variables:
- Primary, accent, and warn color palettes
- Extracted theme colors ($primary-color, $accent-color, etc.)

### Styling Rules (CRITICAL)
1. **Use global styles first**: Always check if a style already exists in styles.scss before creating new styles.
2. **Component SCSS files should be minimal**: Only add component-specific styles that are truly unique to that component.
3. **Adding new global styles**:
   - You MAY add NEW utility classes, patterns, or styles to styles.scss if they don't exist.
   - You MAY add NEW color variables to theme.scss if needed.
   - You MUST NOT modify, override, or change any existing styles in styles.scss or theme.scss.
   - New additions should follow the existing patterns and naming conventions.
4. **Preserve existing styling**: Never change values of existing CSS classes, variables, or theme colors.

## Code File Modification Rules
1. PRESERVE all [IntentManaged] Attributes on the existing component's constructor, class or file.
2. Add import statements for all modules, components, and services that you use.
3. (CRITICAL) Read and understand the code in all provided input files. Understand how these code files interact with one another.
4. If services to provide data are available, use them.
5. (CRITICAL) CHECK AND ENSURE AND CORRECT all bindings between the `.html` and `.ts`. The code must compile!
6. **Only modify files listed in "Files Allowed To Modify". All other input files are read-only.**

## TypeScript File Rules (IMPORTANT)
- PRESERVE existing code in the `.ts` file. You may add code, but you are not allowed to change the existing code (IMPORTANT) in the `.ts` file!
- (IMPORTANT) NEVER ADD COMMENTS, not even code comments from templates or examples
- Don't display technical ids to end users like Guids
- If there are forms ensure that they are valid when doing saves, creates, updates etc. (IMPORTANT)
- When adding components or concepts like `ngIf` to the `html` ensure in the backing `ts` file you add and configure the corresponding imports. (CRITICAL)

## UI Action Rules (VERY IMPORTANT)

(CRITICAL) Action buttons in the page content should ONLY be created from TypeScript methods, NEVER from Navigation Items. Navigation Items are only for side navigation menus/drawers.

1. Treat the TypeScript class as the source of truth for any logic that calls services or performs navigation.

2. When generating the template:
   - First, scan the class methods.
   - For any public method whose name clearly represents a UI action (e.g. starts with: navigateTo, add, create, new, edit, update, delete, remove, view, open, search, load), you SHOULD render a corresponding control in the UI.

   Examples:
   - If the class has `navigateToCustomerAdd()`, render an "Add Customer" button calling it:
       `<button mat-raised-button ... (click)="navigateToCustomerAdd()">Add Customer</button>`
   - If the class has `editCustomer(id: string)`, render an Edit action per row:
       `<button ... (click)="editCustomer(row.id)">Edit</button>`
   - If the class has `onDeleteCustomer(id: string)`, render a Delete action per row.

3. DO NOT bind to or reference methods that do not exist in the class.
   - Never invent method names in the template.
   - If you are unsure whether a method is meant to be a UI action, it is safer to skip the control.

## TypeScript Modification Rules (VERY IMPORTANT)

4. You MAY add **new helper methods in the .ts file** if needed, as long as they:
   - only manipulate component state, or
   - only call existing methods in the same class.
   - do NOT directly call services or `router.navigate`.

5. DO NOT change the implementation (internal logic) of any existing methods that:
   - directly call injected services (e.g. `this.someService...`)
   - or call the Angular router (e.g. `this.router.navigate(...)`).

Allowed:
- Calling existing service/navigation wrapper methods from lifecycle hooks and event handlers (e.g. add `ngOnInit()` calls like `this.loadCategories()` or `this.loadCustomerById(id)`).
- Adding new orchestration methods such as `initPageData()`, `onSearch()`, `save()`, etc., as long as they only *call* existing methods and do not rewrite service payloads/signatures.

Not allowed:
- Editing the body of existing service/navigation wrapper methods (changing request payload mapping, error handling, endpoints, routing paths, etc.).

6. If a desired UI action would require changing an existing service / navigation method, prefer to:
   - call that existing method from the template, OR
   - create a small wrapper method that calls it,
   instead of editing the existing method's internals.

### Lifecycle Wiring Rule (IMPORTANT)
- If the screen requires initial data (lookups, entity-by-id, etc.), the component must load it in `ngOnInit()`.
- Prefer calling existing methods like `loadCategories()`, `loadEntityById(id)`, `loadSubCategories(...)`.
- If those methods do not exist, create *new* load methods rather than editing service methods.

## Layout Rules (IMPORTANT)

- Use the provided sample template as the layout blueprint.
- Preserve the overall structure: hero card, then main card with:
  - a filter section using .filter-grid
  - a button row using .button-row that contains both Search and Add buttons
  - the data table and paginator below.
- Do NOT introduce new top-level wrappers (e.g. extra <div> around the card) unless strictly necessary.
- Do NOT move the "Add" button into the header section or separate row; keep it in the same .button-row as the Search button.
- You may change method names, labels, and bindings, but keep the DOM hierarchy and CSS class names the same as the sample.

## UI Component Preference Order

When selecting controls, use the following priority:

1. Angular Material component (preferred)
2. Angular Material + native input integration (`matInput`)
3. Native HTML controls (only as a last resort)

Examples:
- Dates ? `mat-datepicker` (NOT `<input type="date">`)
- Booleans ? `mat-slide-toggle` or `mat-checkbox`
- Enums ? `mat-select`
- Text ? `<input matInput>`

## Compilation Safety Check (IMPORTANT)

Before producing Angular templates:
- Ensure no template reference variables (`#ref`) contain expressions or interpolation.
- Ensure all Material directive bindings reference valid identifiers, not strings or expressions.
- If unsure, prefer a simpler, valid Angular pattern over a dynamic one.

## Navigation Rendering Rules

Navigation items are ONLY for navigation drawers/menus (side navigation), NOT for action buttons in the page content.

- If navigation items are provided:
  - Render each item as a menu link in the navigation drawer ONLY.
  - DO NOT create standalone buttons or actions in the page content based on navigation items.
  - Check the TypeScript file for existing navigation methods (e.g. `navigateToCustomers()`, `navigateToOrders()`).
  - If a navigation method exists for a route:
    - Use `(click)="navigationMethod()"` instead of `routerLink`.
  - If no navigation method exists:
    - Use `routerLink="/path"` with `routerLinkActive="active"`.
  - Each navigation item should include:
    - `<mat-icon matListItemIcon>` with the appropriate icon
    - `<span matListItemTitle>` with the display text
- DO NOT add navigation items that are not listed.
- DO NOT modify existing navigation methods in the TypeScript file.
- (CRITICAL) Navigation items should NEVER be rendered as buttons in the main page content. If a navigation item points to an "Add" route (e.g., "Add Customer"), and the TypeScript file has a corresponding method (e.g., `navigateToCustomerAdd()`), create the button based on the UI ACTION RULES (from the TypeScript method), NOT from the navigation item. Navigation items are for navigation menus only.

## Architectural Guidelines

- Follow the Single Responsibility Principle. Components should focus on presentation logic and delegate business logic to services.
- Use Dependency Injection to inject required services into the component's constructor.
- Components should orchestrate UI interactions and delegate data operations to services.
- Keep component logic minimal - complex business logic should live in services.
- Use reactive patterns with RxJS observables for asynchronous operations.
- Follow Angular best practices for change detection and lifecycle hooks.
- Ensure proper cleanup of subscriptions in `ngOnDestroy()` to prevent memory leaks.

## Validation Checklist (perform before output)
- [ ] All `[(ngModel)]` and event handlers in `.html` are defined in `.ts`.
- [ ] `[IntentManaged]` attributes preserved.
- [ ] Code compiles with added `import` statements.
- [ ] No Comments were added to the code.
- [ ] Existing styles in `styles.scss` and `theme.scss` have NOT been modified (only new styles may be added).
- [ ] Component `.scss` files are minimal and only contain component-specific styles.
- [ ] All bindings between `.html` and `.ts` are correct and the code compiles.
- [ ] Forms are validated before save/create/update operations.
- [ ] Required imports are added to the TypeScript file for all used components and directives.
