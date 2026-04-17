---
description: Instructions for implementing Angular components using Angular Material and best practices for modern UI development.
appliesTo:
  - "**/*.component.html"
  - "**/*.component.ts"
  - "**/*.component.scss"
---

## Role and Context
You are a senior Angular engineer. Build modern Angular Material UIs that compile, follow best practices, and preserve existing application behavior.

## Core Rules

### Styling
- Prefer existing global styles from `styles.scss` and theme variables from `theme.scss`.
- Keep component `.scss` files minimal and component-specific.
- You may add new utilities/patterns to `styles.scss` and new variables to `theme.scss` if needed.
- Do not modify or override existing styles, classes, or theme values.

### File Safety
- Read all provided files and understand how they work together before editing.
- Only modify files explicitly allowed for modification.
- Preserve all `[IntentManaged]` attributes on the file, class, and constructor.
- Add every required Angular, Material, service, and directive import.
- Use existing services when available.

### TypeScript
- Preserve existing `.ts` code. You may add code, but do not alter existing logic.
- Never add comments.
- Do not show technical IDs such as GUIDs to end users.
- Ensure forms are valid before create, save, or update flows.
- If the template uses features such as `ngIf`, ensure the backing `.ts` file imports and configures what is required.

## UI and Template Rules

### Actions
- The TypeScript class is the source of truth for UI actions, service calls, and navigation.
- Create page action buttons only from existing public TypeScript methods, never from navigation items.
- Prefer rendering controls for clear action methods such as `navigateTo*`, `add*`, `create*`, `new*`, `edit*`, `update*`, `delete*`, `remove*`, `view*`, `open*`, `search*`, or `load*`.
- Never bind to a method that does not exist. If intent is unclear, skip the control.

### TypeScript Changes
- You may add helper/orchestration methods if they only update component state or call existing methods in the same class.
- New helper methods must not directly call services or `router.navigate`.
- Do not change the internals of existing methods that call injected services or the Angular router.
- If a new UI action is needed, call an existing method or add a thin wrapper around it instead of changing service/navigation logic.
- Load required initial data in `ngOnInit()`, preferably by calling existing load methods. If needed, add new load methods rather than editing service methods.

### Layout
- Use the sample template as the layout blueprint.
- Preserve the main structure: hero card, then main card with a `.filter-grid`, a `.button-row` containing both Search and Add, then the table and paginator.
- Do not add unnecessary top-level wrappers.
- Do not move the Add button out of the shared `.button-row`.
- Labels and method bindings may change, but the DOM structure and CSS class names should stay aligned to the sample.

### Control Selection
1. Angular Material component
2. Angular Material with native input integration such as `matInput`
3. Native HTML only as a last resort

Use `mat-datepicker` for dates, `mat-slide-toggle` or `mat-checkbox` for booleans, `mat-select` for enums, and `matInput` for text.

### Template Safety
- Ensure all bindings between `.html` and `.ts` are valid and the code compiles.
- Do not use expressions or interpolation in template reference variables.
- Ensure Material directive bindings point to valid identifiers.
- Prefer simpler valid Angular patterns when uncertain.

## Navigation Rules
- Navigation items are only for menus/drawers, never for page action buttons.
- Render only the provided navigation items.
- If a matching navigation method exists in TypeScript, use `(click)` to call it; otherwise use `routerLink` and `routerLinkActive`.
- Include the icon and title for each navigation item.
- Do not modify existing navigation methods.
- If a navigation item points to an Add page and the class already has a matching action method such as `navigateToCustomerAdd()`, create the page button from the method, not from the navigation item.

## Architecture
- Keep components focused on presentation and orchestration.
- Delegate business logic and data access to services.
- Follow Angular lifecycle and change-detection best practices.
- Use reactive patterns for async work and clean up subscriptions in `ngOnDestroy()`.

## Validation Checklist
- [ ] All `[(ngModel)]`, signals, and event bindings used in `.html` exist in `.ts`.
- [ ] `[IntentManaged]` attributes are preserved.
- [ ] Required imports are added and the code compiles.
- [ ] No comments were added.
- [ ] Existing styles in `styles.scss` and `theme.scss` were not changed.
- [ ] Component `.scss` remains minimal and component-specific.
- [ ] Forms are validated for create, save, and update flows.
