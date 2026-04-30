---
name: angular-dialog-editing-entity
description: Creates Angular edit/update entity dialog using Angular Material and template-driven forms, strictly preserving existing TypeScript service and payload behavior while wiring a valid Save flow and model-bound UI.
paths:
  - "**/*.component.ts"
  - "**/*.component.html"
contentHash: C91B1B8D0765C9293E3097430F9952A26C12DC23E10D012BB70CEFEF4C102CBD
---

## MANDATORY: Read Samples Before Implementation

**STOP** - You MUST read ALL sample files in the SAME folder as this SKILL.md before writing ANY code:

1. `edit-entity-dialog-sample.ts`
2. `edit-entity-dialog-sample.html`
3. `edit-entity-dialog-sample.scss`

Then read target component `.ts` file and related project files (models, enums, lookups, services, styles).

**If any sample file cannot be accessed**: Stop immediately, confirm SKILL.md folder location, retry from that location. If still inaccessible, report which file and ask user. Do NOT proceed with partial implementation or approximation.

---

## Preserve Existing Implementation

**Use for**: Edit/Update entity **dialogs** in Angular with Angular Material  
**Do NOT use for**: Full-page forms, search/list pages, or non-Angular projects  
**Source of truth**: Existing component TS file defines service calls and model structure  
**This is a DIALOG**: No navigation�use `dialogRef.close()` instead

### You MUST NOT:
- Modify existing backend methods (e.g., `updateEntity()`)
- Change payload shape sent to backend
- Add, rename, or remove model properties
- Invent lookup services
- Rewrite existing TS functionality
- Add navigation logic

---

## 1. Dialog Structure & Data Loading

**Dialog setup**:
- Inject `MatDialogRef<COMPONENT_NAME>` in constructor: `constructor(private dialogRef: MatDialogRef<COMPONENT_NAME>, ...) { }`
- **Data loading**: Receive entity data via `@Inject(MAT_DIALOG_DATA) public data: { id: string }` or `@Inject(MAT_DIALOG_DATA) public data: EntityModel`
- If ID passed: Load entity in `ngOnInit()` using existing service
- If entity passed: Prepopulate model directly from `MAT_DIALOG_DATA`

**Form**: Build from existing model in component TS. Bind to `model` using `[(ngModel)]="model.propertyName"`. Prepopulate all values. Use only existing properties�no additions, renames, or removals.

**Nullable objects** (e.g., `loyalty: X | null`):
- Toggle/checkbox controls section (e.g., "Has Loyalty")
- Nested fields render only when toggle ON
- Toggle OFF ? null; toggle ON ? initialize if null
- Do NOT render as required/visible unless: explicit required validation, payload always sends it, or required validators on nested fields with always non-null object

---

## 2. Save & Cancel Methods (CRITICAL)

**`save()` or `onSave(form: NgForm)`**:
1. Validate form (check `form.invalid` if using `NgForm`)
2. Call existing service method (e.g., `updateEntity()`) without modification
3. On success: `this.dialogRef.close(true)`
4. On error: Set `serviceErrors.*`, keep dialog open (do NOT close)

**`cancel()`**:
- ONLY calls `this.dialogRef.close(null)` or `this.dialogRef.close(false)`
- Do NOT reset model, call services, or add other logic

**Template bindings**:
- Save button: `(click)="save()"` or `type="submit"` with `(ngSubmit)="onSave(form)"`
- Cancel button: `(click)="cancel()"`
- Do NOT call service methods directly (e.g., `(click)="updateCustomer()"`)

---

## 3. Map Properties to Controls

| Property Type | Control |
|---------------|---------|
| String | `<input matInput>` or `<textarea matInput>` |
| Boolean | `<mat-slide-toggle>` or `<mat-checkbox>` |
| Enum | `<mat-select>` (see Enum Rules) |
| Lookup | `<mat-select>` from **real** services only |
| Array | Repeatable Material blocks |

**Enum Rules**:
1. **Locate**: Use import path in component or code search `export enum AddressType`
2. **Read**: Extract exact member names/values
3. **Use**: Expose to template (`AddressType = AddressType;`), use in HTML (`<mat-option [value]="AddressType.Delivery">`)
4. **Forbidden**: Assume member names from sample. Target project enum is source of truth.

---

## 4. Template-Driven Forms & Validation

Use `<form #form="ngForm" novalidate>` wrapping dialog content and actions.

**Required fields need ALL**:
```html
<mat-form-field>
    <mat-label>Name</mat-label>
    <input matInput required name="name" [(ngModel)]="model.name" #nameCtrl="ngModel" />
    <mat-error *ngIf="nameCtrl.invalid && (nameCtrl.touched || form.submitted)">
        Name is required
    </mat-error>
</mat-form-field>
```

**Form state**:
- Use `(ngSubmit)="onSave(form)"` (preferred) or `(click)="onSave(form)"`
- In `onSave(form)`: If `form.invalid`, call `form.control.markAllAsTouched()` and return (no service call)
- Save button disabled when `form.invalid || isLoading`:
```html
<button mat-raised-button color="primary" type="submit" [disabled]="form.invalid || isLoading">Save</button>
```

---

## 5. Child Collections

Render in repeatable Material blocks (e.g., `addresses`, `phones`).

**Buttons**: Add only if `addX()` exists in TS (e.g., `addAddress()`). Remove only if `removeX()` exists in TS (e.g., `removeAddress(i)`). Do NOT invent.

---

## 6. Styling

- **Global utilities first**: `.filter-grid`, `.button-row`, `.table-wrapper`, `.ux-gradient-primary`, `.pa-4`, `.mb-4`
- **Component SCSS**: Add when sample uses component styling and global doesn't provide it
- **Match sample layout visually**
- **NEVER modify** `styles.scss`/`theme.scss` (add only if reusable across pages)
- **Component SCSS required**: Edit-entity-dialog sample needs component SCSS for layout. Copy/adapt unless exact styles exist globally.

---

## Definition of Done

**Template compilation**:
- [ ] All bound properties exist in TS (`model.*`, `isLoading`, `serviceErrors.*`, lookups)
- [ ] All methods exist (`save(form)`/`onSave(form)`, `cancel()`, `addX()`, `removeX()`)
- [ ] Directives valid (`*ngIf`, `*ngFor`, `(click)`)
- [ ] `MatDialogRef<COMPONENT_NAME>` injected in constructor
- [ ] `MAT_DIALOG_DATA` injected to receive entity data

**Data loading**:
- [ ] If ID passed: Data loaded in `ngOnInit()` via existing service
- [ ] If entity passed: Model prepopulated from `MAT_DIALOG_DATA`
- [ ] Form prepopulated using `[(ngModel)]="model.propertyName"`

**Form fidelity**:
- [ ] Controls bind only to existing model properties
- [ ] No properties renamed/removed/retyped
- [ ] Controls follow section 3 mapping

**Template-driven forms**:
- [ ] Uses `<form #form="ngForm" novalidate>`
- [ ] Every `[(ngModel)]` has unique `name`
- [ ] Required fields: `required`, `name`, `#ctrl="ngModel"`, `<mat-error>` shown when `ctrl.invalid && (ctrl.touched || form.submitted)`
- [ ] Save disabled when `form.invalid || isLoading`
- [ ] `onSave(form)` prevents call when invalid

**Dialog behavior**:
- [ ] Injects `MatDialogRef<COMPONENT_NAME>`
- [ ] `save()`/`onSave(form)` calls `dialogRef.close(true)` on success
- [ ] `save()`/`onSave(form)` does NOT close on error
- [ ] `cancel()` calls `dialogRef.close(null/false)` with no other logic
- [ ] `cancel()` does NOT reset model or call services
- [ ] Save button calls `save()` or uses `type="submit"` with `(ngSubmit)="onSave(form)"`
- [ ] Cancel button calls `cancel()`
- [ ] No navigation logic

**Save flow**:
- [ ] Save calls `save(form)`/`onSave(form)`, not service directly
- [ ] Backend methods (e.g., `updateEntity()`) not modified
- [ ] Payload matches existing TS exactly

**Nullable objects & conditional sections**:
- [ ] Toggle/checkbox controls visibility
- [ ] Toggle OFF ? null; ON ? initialize
- [ ] Nested fields render only when enabled
- [ ] No unsafe `model.obj!.field`
- [ ] Conditional sections maintain exact TS logic

**Child collections**:
- [ ] Add buttons only if `addX()` in TS
- [ ] Remove buttons only if `removeX()` in TS

**Enums**:
- [ ] Options match real enum members
- [ ] No names from sample without verification
- [ ] Defaults compile against actual type

**Styling**:
- [ ] No modifications to `styles.scss`/`theme.scss`
- [ ] Component SCSS minimal, prefers global utilities
- [ ] Component SCSS adapted from sample when needed

### 1. Dialog-specific component structure and data loading
- This component is a Material dialog, **not a page**. Use the standard Angular Material dialog pattern.
- The component must inject `MatDialogRef<COMPONENT_NAME>` in the constructor:
  - `constructor(private dialogRef: MatDialogRef<COMPONENT_NAME>, ...) { }`
- **Data Loading (IMPORTANT)**: The dialog must receive the entity data to edit. This is typically done via:
  - `@Inject(MAT_DIALOG_DATA) public data: { id: string }` or `@Inject(MAT_DIALOG_DATA) public data: EntityModel`
  - In `ngOnInit()`, load the entity data using the provided service if only an ID is passed
  - If the full entity is passed via `MAT_DIALOG_DATA`, prepopulate the model directly
- Build an Angular Material form based on the entity model defined in the component TypeScript file.
- Bind every input to a property on `model`.
- Do not modify the shape of the model.
- Do not add properties that do not exist.
- Do not rename or remove properties.
- Do not invent new fields.

### 2. Save and Cancel methods (IMPORTANT)
Implement two top-level methods for the template to use:

**`save()` or `onSave(form: NgForm)` method:**
- Validates the form (if using `onSave(form: NgForm)`, check `form.invalid`)
- Calls the existing service method (e.g. `updateCustomer()`)
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
- Do not call service methods directly from the template (e.g., `(click)="updateCustomer()"`).
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
- **Prepopulate form values using `[(ngModel)]="model.propertyName"`** (IMPORTANT for edit dialogs).
- Render a toggle (or checkbox) for nullable objects like "Has Loyalty"
  - Only render the nested fields when enabled
  - Ensure the toggle actually controls nullability:
    - ON: create the object if currently null
    - OFF: set it to null

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
	- call the existing service method (e.g., `updateEntity()`) without modifying it
	- on success, call `this.dialogRef.close(true)` to close the dialog and signal success to the caller
	- on error, set a `serviceErrors.*` message and keep the dialog open
- Never call `dialogRef.close(...)` on failure.
- DO NOT modify the existing service method.
- If a method name like `updateCustomer()` already exists and calls the backend, either:
  - call it from inside `save()`, **or**
  - inline its logic into `save()`, but do not change its behavior to stop calling the service.

### 6. Cancel behavior (IMPORTANT)
- The Cancel button must call `cancel()` and `cancel()` must only call `this.dialogRef.close(null)` (or `false`), with no additional logic.
- Do NOT reset the model in `cancel()`.
- Do NOT call any services in `cancel()`.
- The dialog should close without saving any data.

### 7. Conditional sections
- If the TS model uses boolean toggles (like `hasLoyalty`):
  - Only render conditional blocks when flagged.
  - Maintain the exact logic from the TS file for showing/hiding these blocks.
- Render a toggle (or checkbox) appropriately
  - Only render the nested fields when enabled
  - Ensure the toggle actually controls nullability:
    - ON: create the object if currently null
    - OFF: set it to null
- Nullable objects should not be rendered as required/always visible unless:
  - the TS code has explicit required validation logic, or
  - the payload unconditionally sends it, or
  - there are required validators on nested fields and the object is always non-null.

### 8. Child collections must follow existing method availability
- When arrays exist (`addresses`, `phones`, etc.):
  - Render list items in Material blocks.
  - Include a delete/remove button **only if corresponding TS methods exist**:
    - `addAddress()` ? show Add button
    - `removeAddress(i)` ? show Remove button
- Render child collections in repeatable Material UI blocks.
- Include a delete button only if the TypeScript file already contains a matching remove method (e.g., `removeX()`).
- Include an add button only if the TypeScript file already contains a matching add method (e.g., `addX()`).

### 9. Forbidden actions
- DO NOT modify existing backend-calling methods (e.g., `updateEntity()`).
- DO NOT change the shape of the payload.
- DO NOT invent lookup services.
- DO NOT add logic that rewrites existing TypeScript functionality.
- DO NOT add navigation logic (this is a dialog, not a page).

### 10. General constraints
- Use this skill for Edit/Update entity **dialogs** in Angular.
- Do not use this skill for full-page forms, search/list pages, or non-Angular projects.
- Treat the existing component TypeScript file as the source of truth for service calls.
- Remember this is a dialog component - no navigation, use `dialogRef.close()` instead.

### 11. Enum fields Required Implementation Steps
For each enum used by the target component model:
- Locate the enum definition:
  - Prefer direct import path already present in the component (import { AddressType } from '...')
  - Otherwise use search_code for export enum AddressType (or the enum name)
- Read the enum file and extract exact member names and values.
  - Use those exact members in:
  - Template exposure in .ts (e.g., AddressType = AddressType;)
  - `<mat-option [value]="AddressType.Delivery">...</mat-option>` in .html

## 12. Mandatory: Verify DTO/Wrapper Shapes Before Template Binding
When binding in HTML to a property that is not directly declared on the component (e.g., `customersModels?.X`, `response?.X`, `paged?.X`, `result?.X`), you MUST verify the shape of the type:

- If the property is a generic wrapper (e.g., `PagedResult<T>`, `ListResult<T>`, `ApiResponse<T>`), you MUST:
	- Navigate to its definition (via import path) and read the file.
	- Use the exact collection property name from the type (e.g., data vs items).

- For any `*ngFor`, you MUST confirm the iterated expression resolves to an array type in the codebase.
	- Forbidden: assuming common names like items, results, value, content.
	- Required: use only verified members from the actual interface/class.

- Output requirement:
	- If the wrapper type cannot be located/read, STOP and ask the user which property contains the collection.

No `*ngFor` over ?.items unless the type definition explicitly contains items

---

## Completion Checklist

- [] The HTML template compiles and every referenced symbol exists in the component TS:
  - [] All bound properties exist (e.g., model.*, isLoading, serviceErrors.*, lookup arrays).
  - [] All called methods exist (e.g., save(form) or onSave(form), cancel(), addX(), removeX()).
  - [] All structural directives and bindings are syntactically valid (*ngIf, *ngFor, (click), etc.).
  - [] MatDialogRef is properly injected in the constructor.
  - [] MAT_DIALOG_DATA is properly injected to receive entity data (ID or full entity).
- [] The form is strictly model-driven:
  - [] All inputs/selects/toggles bind only to properties that already exist on model (no invented fields).
  - [] No model properties were renamed, removed, or retyped to satisfy the UI.
  - [] Data is loaded in ngOnInit (if ID is passed) or prepopulated from MAT_DIALOG_DATA (if entity is passed).
  - [] Form values are prepopulated using [(ngModel)]="model.propertyName".
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
  - [] The component injects `@Inject(MAT_DIALOG_DATA)` to receive entity data.
  - [] `save()` or `onSave(form)` calls `this.dialogRef.close(true)` on success.
  - [] `save()` or `onSave(form)` does NOT close the dialog on error (keeps dialog open with error message).
  - [] `cancel()` calls `this.dialogRef.close(null)` or `this.dialogRef.close(false)` with no other logic.
  - [] `cancel()` does NOT reset the model or call services.
  - [] Save button calls `save()` or uses `type="submit"` with `(ngSubmit)="onSave(form)"`.
  - [] Cancel button calls `cancel()`.
  - [] No navigation logic exists (this is a dialog, not a page).
- [] Save flow and backend behavior are preserved:
  - [] The Save button/form calls `save(form)` or `onSave(form)` (not a service method directly).
  - [] Existing backend-calling methods (e.g., `updateEntity()`) were not modified.
  - [] The request payload sent to the backend matches the existing TS mapping exactly (no added/removed/reshaped fields).
- [] Nullable object sections (e.g., loyalty: X | null) are handled safely and intentionally:
  - [] Nullable object sections are optional by default via a toggle/checkbox (e.g., "Has Loyalty").
  - [] Toggle OFF sets the object to null; toggle ON initializes the object if it is null.
  - [] Nested fields render only when enabled (no unsafe model.obj!.field usage when it can be null).
  - [] Conditional sections maintain the exact logic from the TS file for showing/hiding blocks.
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
