---
name: angular-dialog-adding-entity
description: Creates Angular add/create entity dialog using Angular Material and template-driven forms, strictly preserving existing TypeScript service and payload behavior while wiring a valid Save flow and model-bound UI.
paths:
	- "**/*.component.ts"
	- "**/*.component.html"
---

## Reference Examples

Before generating any output, read the following files in the same folder as this skill:

1. **`./add-entity-dialog-sample.ts`** - MUST be read and used as the TypeScript structure template (imports, component metadata, MatDialogRef injection, `model` wiring, save/cancel flow orchestration, and helper methods) and adapt names and types for the target entity.
2. **`./add-entity-dialog-sample.html`** - MUST be read and used as the HTML layout template. Copy its layout and Angular Material dialog/form composition, then adapt fields, bindings, and labels for the target entity.
3. **`./add-entity-dialog-sample.scss`** - MUST be read whenever implementing the component, even if you expect minimal styling.

---

### Styling Rules
- Use existing utility classes from `styles.scss` (e.g., `.filter-grid`, `.button-row`, `.table-wrapper`, `.ux-gradient-primary`, `.pa-4`, `.mb-4`, etc.)
- SCSS decision rule:
  - Use global utilities where possible.
  - If the sample uses component SCSS for structural layout (grid columns, responsive behavior) and global styles don't already guarantee it, copy/adapt the sample SCSS into the component SCSS.
  - Only add to styles.scss when the style is reusable across multiple pages; otherwise keep it component-scope
- If you need a new utility class or pattern that doesn't exist, you may add it to `styles.scss`
- NEVER modify existing styles in `styles.scss` or `theme.scss` - only add new ones if needed
- Copy/adapt sample SCSS into the component SCSS unless you can prove the same classes already exist globally.
- The generated dialog must visually match the sample layout.
- SCSS Parity Rule (Required): The add-entity-dialog sample relies on component-scoped SCSS for layout. Therefore, when generating a dialog, the agent must copy/adapt the sample SCSS into the component SCSS unless those exact styles already exist in styles.scss. The agent must not leave component SCSS empty if the template uses classes that aren't confirmed global.

### 1. Dialog-specific component structure
- This component is a Material dialog, **not a page**. Use the standard Angular Material dialog pattern.
- The component must inject `MatDialogRef<COMPONENT_NAME>` in the constructor:
  - `constructor(private dialogRef: MatDialogRef<COMPONENT_NAME>, ...) { }`
- If input data is needed, also inject `@Inject(MAT_DIALOG_DATA) public data: SomeDataType`.
- Build an Angular Material form based on the entity model defined in the component TypeScript file.
- Bind every input to a property on `model`.
- Do not add properties that do not exist.
- Do not rename or remove properties.

### 2. Save and Cancel methods (IMPORTANT)
Implement two top-level methods for the template to use:

**`save()` or `onSave(form: NgForm)` method:**
- Validates the form (if using `onSave(form: NgForm)`, check `form.invalid`)
- Calls the existing service method (e.g. `createCustomer()` or `updateCustomer()`)
- On success, calls `this.dialogRef.close(true)` to close the dialog and signal success
- On error, sets an error message (e.g., `serviceErrors.*`) but does NOT close the dialog

**`cancel()` method:**
- Calls `this.dialogRef.close(null)` or `this.dialogRef.close(false)` to close the dialog without saving
- Do NOT reset the model or call any services here
- (IMPORTANT) Never treat `cancel()` as a "reset the form" method. It must only close the dialog.

**Template binding rules:**
- (IMPORTANT) In the HTML, bind the action buttons to `save()` (or `onSave(form)`) and `cancel()`, NOT to raw service methods:
  - Save button: `(click)="save()"` or `type="submit"` with `(ngSubmit)="onSave(form)"`
  - Cancel button: `(click)="cancel()"`
- Do not call service methods directly from the template (e.g., `(click)="createCustomer()"`).
- (IMPORTANT) After a successful save, always close the dialog via `dialogRef.close(...)` so the caller can react (e.g. refresh the list).

### 3. Map property types to correct Angular Material controls
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
- Render a toggle (or checkbox) for nullable objects like "Has Loyalty"
  - Only render the nested fields when enabled
  - Ensure the toggle actually controls nullability:
    - ON: create the object if currently null
    - OFF: set it to null
- Nullable objects should not be rendered as required/always visible unless:
  - the TS code has explicit required validation logic, or
  - the payload unconditionally sends it, or
  - there are required validators on nested fields and the object is always non-null.

### 4. Template-driven validation is required
- Use **template-driven forms** with `FormsModule` and `ngForm`.
- Wrap the dialog content and actions in a single `<form #form="ngForm" novalidate>` element.
- Use either `(ngSubmit)="onSave(form)"` on the form with a `type="submit"` Save button, or `(click)="onSave(form)"` with `form` passed as argument. **Preferred: `(ngSubmit)="onSave(form)"`**.
- Required fields must include all of the following:
	- `required`
	- `name="xxx"`
	- `#xxxCtrl="ngModel"`
	- `[(ngModel)]="model.xxx"`
	- `<mat-error>` with validation messages bound to `xxxCtrl.invalid && (xxxCtrl.touched || form.submitted)`

Example:
```html
<mat-form-field>
    <mat-label>Name</mat-label>
    <input matInput required name="name" [(ngModel)]="model.name" #nameCtrl="ngModel" />
    <mat-error *ngIf="nameCtrl.invalid && (nameCtrl.touched || form.submitted)">
        Name is required
    </mat-error>
</mat-form-field>
```

- In `onSave(form: NgForm)`:
  - If `form.invalid`, call `form.control.markAllAsTouched()` and **return without calling any service**.
  - Only call the backend service if the form is valid.
- Save button must be disabled when:
	- the form is invalid, or
	- `isLoading` is true

Example:
```html
<button
    mat-raised-button
    color="primary"
    type="submit"
    [disabled]="form.invalid || isLoading">
    Save
</button>
```

### 5. Save behavior must use existing service flow
- The Save button must call `save()` or `onSave(form)` method.
- The save method must:
	- perform validation via `form.invalid` (if using NgForm)
	- call the existing service method (e.g., `createEntity()`) without modifying it
	- on success, call `this.dialogRef.close(true)` to close the dialog and signal success to the caller
	- on error, set a `serviceErrors.*` message and keep the dialog open
- Never call `dialogRef.close(...)` on failure.
- If a method name like `createCustomer()` or `updateCustomer()` already exists and calls the backend, either:
  - call it from inside `save()`, **or**
  - inline its logic into `save()`, but do not change its behavior to stop calling the service.

### 6. Cancel behavior (IMPORTANT)
- The Cancel button must call `cancel()` and `cancel()` must only call `this.dialogRef.close(null)` (or `false`), with no additional logic.
- Do NOT reset the model in `cancel()`.
- Do NOT call any services in `cancel()`.
- The dialog should close without saving any data.

### 7. Child collections must follow existing method availability
- Render child collections in repeatable Material UI blocks.
- Include a delete button only if the TypeScript file already contains a matching remove method (e.g., `removeX()`).
- Include an add button only if the TypeScript file already contains a matching add method (e.g., `addX()`).

### 8. Forbidden actions
- DO NOT modify existing backend-calling methods (e.g., `createEntity()`).
- DO NOT change the shape of the payload.
- DO NOT invent lookup services.
- DO NOT add logic that rewrites existing TypeScript functionality.
- DO NOT add navigation logic (this is a dialog, not a page).

### 9. General constraints
- Use this skill for Add/Create entity **dialogs** in Angular.
- Do not use this skill for full-page forms, search/list pages, or non-Angular projects.
- Treat the existing component TypeScript file as the source of truth for service calls.
- Remember this is a dialog component - no navigation, use `dialogRef.close()` instead.

### 10. Enum fields Required Implementation Steps
For each enum used by the target component model:
- Locate the enum definition:
  - Prefer direct import path already present in the component (import { AddressType } from '...')
  - Otherwise use search_code for export enum AddressType (or the enum name)
- Read the enum file and extract exact member names and values.
  - Use those exact members in:
  - Default initialization in .ts (e.g., AddressType.Delivery)
  - `<mat-option [value]="AddressType.Delivery">...</mat-option>` in .html

## Completion Checklist

- [] The HTML template compiles and every referenced symbol exists in the component TS:
  - [] All bound properties exist (e.g., model.*, isLoading, serviceErrors.*, lookup arrays).
  - [] All called methods exist (e.g., save(form) or onSave(form), cancel(), addX(), removeX()).
  - [] All structural directives and bindings are syntactically valid (*ngIf, *ngFor, (click), etc.).
  - [] MatDialogRef is properly injected in the constructor.
  - [] If dialog data is needed, MAT_DIALOG_DATA is properly injected.
- [] The form is strictly model-driven:
  - [] All inputs/selects/toggles bind only to properties that already exist on model (no invented fields).
  - [] No model properties were renamed, removed, or retyped to satisfy the UI.
- [] All controls follow the Angular Material mapping rules:
  - [] Strings use `<input matInput>` (or `<textarea matInput>` when appropriate).
  - [] Booleans use `<mat-slide-toggle>` or `<mat-checkbox>`.
  - [] Enums use `<mat-select>` with explicit enum values.
  - [] Lookups use `<mat-select>` populated only from options loaded by existing services (no invented services/options).
  - [] Arrays render as repeatable blocks bound to each item.
- [] Template-driven forms and validation are correctly implemented:
  - [] The form uses template-driven forms: `<form #form="ngForm" novalidate>`.
  - [] Every `[(ngModel)]` has a `name="..."` attribute, and names are unique (including inside *ngFor blocks).
  - [] Required fields include all of: `required`, `name="..."`, `#ctrl="ngModel"`, and a `<mat-error>` shown when `ctrl.invalid && (ctrl.touched || form.submitted)`.
  - [] The Save button is disabled when `isLoading` is true or `form.invalid` is true.
  - [] `save(form)` or `onSave(form)` prevents saving when `form.invalid` is true (no backend call when invalid).
- [] Dialog-specific behavior is correct:
  - [] The component injects `MatDialogRef<COMPONENT_NAME>`.
  - [] `save()` or `onSave(form)` calls `this.dialogRef.close(true)` on success.
  - [] `save()` or `onSave(form)` does NOT close the dialog on error (keeps dialog open with error message).
  - [] `cancel()` calls `this.dialogRef.close(null)` or `this.dialogRef.close(false)` with no other logic.
  - [] `cancel()` does NOT reset the model or call services.
  - [] Save button calls `save()` or uses `type="submit"` with `(ngSubmit)="onSave(form)"`.
  - [] Cancel button calls `cancel()`.
  - [] No navigation logic exists (this is a dialog, not a page).
- [] Save flow and backend behavior are preserved:
  - [] The Save button/form calls `save(form)` or `onSave(form)` (not a service method directly).
  - [] Existing backend-calling methods (e.g., `createEntity()`) were not modified.
  - [] The request payload sent to the backend matches the existing TS mapping exactly (no added/removed/reshaped fields).
- [] Nullable object sections (e.g., loyalty: X | null) are handled safely and intentionally:
  - [] Nullable object sections are optional by default via a toggle/checkbox (e.g., "Has Loyalty").
  - [] Toggle OFF sets the object to null; toggle ON initializes the object if it is null.
  - [] Nested fields render only when enabled (no unsafe model.obj!.field usage when it can be null).
  - [] Any newly added TS methods only manipulate component state and do not call services directly (except the save method).
- [] Child collection actions match method availability:
  - [] "Add" buttons exist only if an `addX()` method already exists in TS.
  - [] "Remove/Delete" buttons exist only if a `removeX(...)` method already exists in TS.
  - [] Collection UI does not introduce validation/name collisions when items are added/removed.
- [] Styling rules are respected:
  - [] Existing styles.scss and theme.scss were not modified (only additive changes allowed if necessary).
  - [] Component SCSS is minimal and only includes truly component-specific layout/styling, preferring global utility classes where possible.
  - [] Any non-utility CSS classes used in the template are defined either globally or in the component SCSS.
  - [] If the sample template uses component SCSS for grid/layout, the target component SCSS includes an adapted version.
- [] Every enum option rendered in the template matches a real enum member defined in the codebase.
  - [] No enum member names were copied from the sample without verification.
  - [] Any enum default values in TS compile against the actual enum type.
