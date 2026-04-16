---
name: angular-page-adding-entity
description: Creates Angular add/create entity pages using Angular Material and template-driven forms, strictly preserving existing TypeScript service and payload behavior while wiring a valid Save flow and model-bound UI.
paths:
	- "**/*.component.ts"
	- "**/*.component.html"
---

## Reference Examples

Before generating any output, you MUST read all of the following files in the same folder as this skill:

1. **`./add-entity-sample.ts`** - MUST be read and used as the TypeScript structure template (imports, component metadata, `model` wiring, save flow orchestration, and helper methods) and adapt names and types for the target entity.
2. **`./add-entity-sample.html`** - MUST be read and used as the HTML layout template. Copy its layout and Angular Material form composition, then adapt fields, bindings, and labels for the target entity.
3. **`./add-entity-sample.scss`** - MUST be read whenever implementing the component, even if you expect minimal styling.

---

### Styling Rules
- Use existing utility classes from `styles.scss` (e.g., `.filter-grid`, `.button-row`, `.table-wrapper`, `.ux-gradient-primary`, `.pa-4`, `.mb-4`, etc.)
- SCSS decision rule:
  - Use global utilities where possible.
  - If the sample uses component SCSS for structural layout (grid columns, responsive behavior) and global styles don’t already guarantee it, copy/adapt the sample SCSS into the component SCSS.
  - Only add to styles.scss when the style is reusable across multiple pages; otherwise keep it component-scope
- If you need a new utility class or pattern that doesn't exist, you may add it to `styles.scss`
- NEVER modify existing styles in `styles.scss` or `theme.scss` - only add new ones if needed
- Copy/adapt sample SCSS into the component SCSS unless you can prove the same classes already exist globally.
- The generated page must visually match the sample layout.
- SCSS Parity Rule (Required): The add-entity sample relies on component-scoped SCSS for layout. Therefore, when generating an add page, the agent must copy/adapt the sample SCSS into the component SCSS unless those exact styles already exist in styles.scss. The agent must not leave component SCSS empty if the template uses classes that aren’t confirmed global.

### 1. Form must be generated from the existing model only
- Build an Angular Material form based on the entity model defined in the component TypeScript file.
- Bind every input to a property on `model`.
- Do not add properties that do not exist.
- Do not rename or remove properties.
- Render a toggle (or checkbox) like “Has Loyalty”
  - Only render the nested fields when enabled
  - Ensure the toggle actually controls nullability:
    - ON: create the object if currently null
    - OFF: set it to null
- Nullable objects should not be rendered as required/always visible unless:
  - the TS code has explicit required validation logic, or
  - the payload unconditionally sends it, or
  - there are required validators on nested fields and the object is always non-null.


### 2. Map property types to correct Angular Material controls
- For each property:
	- Strings -> `<input matInput>`
	- Booleans -> `<mat-slide-toggle>`
	- Enums -> `<mat-select>`
    - When rendering enum fields (e.g., AddressType, Status, etc.):
      - The enum must be treated as the source of truth from the actual imported enum definition in the target project, not from the skill sample.
      - Before generating template options or default enum values, the agent must read the enum definition file (or otherwise verify the enum members via project search).
      - The agent must not assume enum member names from any sample code (e.g., Deliver vs Delivery).
	- Lookups -> `<mat-select>` with service-loaded options only if such services exist
	- Arrays -> repeatable Material blocks

### 3. Template-driven validation is required
- Use template-driven Angular forms (`ngForm`).
- Required fields must include all of the following:
	- `required`
	- `name="xxx"`
	- `#xxxCtrl="ngModel"`
	- `<mat-error>` with validation messages when invalid
- Save button must be disabled when:
	- the form is invalid, or
	- `isLoading` is true

### 4. Save behavior must use existing service flow
- The Save button must call a `save()` method.
- The `save()` method must:
	- perform validation via `form.invalid`
	- call the existing service method without modifying it
	- on success, navigate using an existing navigation method (e.g., `navigateToEntitySearch()`)

### 5. Child collections must follow existing method availability
- Render child collections in repeatable Material UI blocks.
- Include a delete button only if the TypeScript file already contains a matching remove method (e.g., `removeX()`).
- Include an add button only if the TypeScript file already contains a matching add method (e.g., `addX()`).

### 6. Forbidden actions
- DO NOT modify existing backend-calling methods (e.g., `createEntity()`).
- DO NOT change the shape of the payload.
- DO NOT invent lookup services.
- DO NOT add logic that rewrites existing TypeScript functionality.

### 7. General constraints
- Use this skill for Add/Create entity screens in Angular.
- Do not use this skill for search/list pages, detail/edit/view forms, or non-Angular projects.
- Treat the existing component TypeScript file as the source of truth for service calls and navigation behavior.

### 8. Enum fields Required Implementation Steps
For each enum used by the target component model:
- Locate the enum definition:
  - Prefer direct import path already present in the component (import { AddressType } from '...')
  - Otherwise use search_code for export enum AddressType (or the enum name)
- Read the enum file and extract exact member names and values.
  - Use those exact members in:
  - Default initialization in .ts (e.g., AddressType.Delivery) <mat-option [value]="AddressType.Delivery">...</mat-option> in .html

## Completion Checklist

 - [] The HTML template compiles and every referenced symbol exists in the component TS:
  - [] All bound properties exist (e.g., model.*, isLoading, serviceErrors.*, lookup arrays).
  - [] All called methods exist (e.g., save(form), addX(), removeX(), navigation methods).
  - [] All structural directives and bindings are syntactically valid (*ngIf, *ngFor, (click), etc.).
- [] The form is strictly model-driven:
  - [] All inputs/selects/toggles bind only to properties that already exist on model (no invented fields).
  - [] No model properties were renamed, removed, or retyped to satisfy the UI.
- [] All controls follow the Angular Material mapping rules:
  - [] Strings use <input matInput> (or <textarea matInput> when appropriate).
  - [] Booleans use <mat-slide-toggle> or <mat-checkbox>.
  - [] Enums use <mat-select> with explicit enum values.
  - [] Lookups use <mat-select> populated only from options loaded by existing services (no invented services/options).
  - [] Arrays render as repeatable blocks bound to each item.
 - Template-driven forms and validation are correctly implemented:
  - [] The form uses template-driven forms: <form #form="ngForm">.
  - [] Every [(ngModel)] has a name="..." attribute, and names are unique (including inside *ngFor blocks).
  - [] Required fields include all of: required, name="...", #ctrl="ngModel", and a <mat-error> shown when invalid and touched.
  - [] The Save button is disabled when isLoading is true or form.invalid is true.
  - [] save(form) prevents saving when form.invalid is true (no backend call when invalid).
- [] Save flow and backend behavior are preserved:
  - [] The Save button calls save(form) (not a service method directly).
  - [] Existing backend-calling methods (e.g., createEntity()) were not modified.
  - [] The request payload sent to the backend matches the existing TS mapping exactly (no added/removed/reshaped fields).
  - [] Post-save navigation uses an existing navigation method (no new router.navigate(...) logic introduced).
- []Nullable object sections (e.g., loyalty: X | null) are handled safely and intentionally:
  - [] Nullable object sections are optional by default via a toggle/checkbox (e.g., “Has Loyalty”).
  - [] Toggle OFF sets the object to null; toggle ON initializes the object if it is null.
  - [] Nested fields render only when enabled (no unsafe model.obj!.field usage when it can be null).
  - [] Any newly added TS methods only manipulate component state and do not call services or the router.
- [] Child collection actions match method availability:
  - [] “Add” buttons exist only if an addX() method already exists in TS.
  - [] “Remove/Delete” buttons exist only if a removeX(...) method already exists in TS.
  - [] Collection UI does not introduce validation/name collisions when items are added/removed.
- [] Styling rules are respected:
  - [] Existing styles.scss and theme.scss were not modified (only additive changes allowed if necessary).
  - [] Component SCSS is minimal and only includes truly component-specific layout/styling, preferring global utility classes where possible.
- [] Any non-utility CSS classes used in the template are defined either globally or in the component SCSS.
- [] If the sample template uses component SCSS for grid/layout, the target component SCSS includes an adapted version.
- [] Every enum option rendered in the template matches a real enum member defined in the codebase.
- [] No enum member names were copied from the sample without verification.
- [] Any enum default values in TS compile against the actual enum type.